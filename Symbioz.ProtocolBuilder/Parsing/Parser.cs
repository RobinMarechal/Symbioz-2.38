using Symbioz.ProtocolBuilder.Parsing.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Symbioz.ProtocolBuilder.Parsing {
    public class Parser {
        public static readonly string ClassPatern = @"public class (\w+)\s";
        public static readonly string ClassHeritagePattern = @"extends (?:[\w_]+\.)*(\w+)";
        public static readonly string ConstructorPattern = @"(?<acces>public|protected|private|internal)\s*function\s*(?<name>{0})\((?<argument>[^,)]+,?)*\)";

        public static readonly string ConstFieldPattern =
            @"(?<acces>public|protected|private|internal)\s*(?<static>static)?\s*const\s*(?<name>\w+):(?<type>[\w_\.]+(?:<(?:\w+\.)*(?<generictype>[\w_<>]+)>)?)(?<value>\s*=\s*.*)?;";

        public static readonly string FieldPattern = @"(?<acces>public|protected|private|internal)\s*(?<static>static)?\s*var\s*(?<name>[\w\d@]+):(?<type>[\w\d_\.<>]+)(?<value>\s*=\s*.*)?;";

        public static readonly string MethodPattern =
            @"((?<acces>public|protected|private|internal)|(?<override>override)\s)+\s*function\s*(?<prop>get|set)?\s+(?<name>\w+)\((?<argument>[^,)]+,?)*\)\s*:\s*(?:\w+\.)*(?<returntype>\w+)";

        private string m_fileText;
        private string[] m_fileLines;

        private Dictionary<int, int> m_brackets;

        public string Filename { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> BeforeParsingRules { get; set; }

        public string[] IgnoredLines { get; set; }

        public ClassInfo Class { get; internal set; }

        public EnumInfo EnumInfo { get; internal set; }

        public List<MethodInfo> Constructors { get; internal set; }

        public List<FieldInfo> Fields { get; internal set; }

        public List<MethodInfo> Methods { get; internal set; }

        public List<PropertyInfo> Properties { get; internal set; }

        public bool IgnoreMethods { get; set; }


        public Parser(string filename) {
            this.Filename = filename;
            this.BeforeParsingRules = new Dictionary<string, string>();
            this.IgnoredLines = new string[0];
        }

        public Parser(string filename, IEnumerable<KeyValuePair<string, string>> beforeParsingRules, string[] ignoredLines) {
            this.Filename = filename;
            this.BeforeParsingRules = beforeParsingRules;
            this.IgnoredLines = ignoredLines;
        }

        public void ParseFile() {
            this.m_fileLines = File.ReadAllLines(this.Filename).Where(entry => !this.IsLineIgnored(entry)).Select(entry => ApplyRules(this.BeforeParsingRules, entry.Trim())).ToArray();
            this.m_fileText = string.Join("\r\n", this.m_fileLines);
            this.m_brackets = FindBracketsIndexesByLines(this.m_fileLines, '{', '}');

            this.Class = new ClassInfo {
                Name = this.GetMatch(ClassPatern),
                Heritage = this.GetMatch(ClassHeritagePattern),
                AccessModifier = AccessModifiers.Public,
                // we don't mind about this
                ClassModifier = ClassInfo.ClassModifiers.None
            };

            if (this.Class.Name == string.Empty) {
                throw new InvalidCodeFileException("This file does not contain a class");
            }

            this.Constructors = new List<MethodInfo>();

            this.ParseFields();

            if (!this.IgnoreMethods) {
                this.ParseConstructor();
                this.ParseMethods();
            }
        }

        private void ParseConstructor() {
            Match matchConstructor = Regex.Match(this.m_fileText,
                                                 string.Format(ConstructorPattern,
                                                               this.Class.Name),
                                                 RegexOptions.Multiline);

            if (matchConstructor.Success) {
                var ctor = this.BuildMethodInfoFromMatch(matchConstructor, true);
                ctor.Statements = this.BuildMethodElementsFromMatch(matchConstructor).ToList();

                this.Constructors.Add(ctor);
            }
        }

        private void ParseFields() {
            this.Fields = new List<FieldInfo>();

            Match matchConst = Regex.Match(this.m_fileText,
                                           ConstFieldPattern,
                                           RegexOptions.Multiline);
            while (matchConst.Success) {
                var field = new FieldInfo {
                    Modifiers =
                        (AccessModifiers)
                        Enum.Parse(typeof(AccessModifiers),
                                   matchConst.Groups["acces"].Value,
                                   true),
                    Name = matchConst.Groups["name"].Value,
                    Type =
                        matchConst.Groups["generictype"].Value == string.Empty
                            ? matchConst.Groups["type"].Value
                            : "List<" + matchConst.Groups["generictype"].Value + ">",
                    Value = matchConst.Groups["value"].Value.Replace("=", "").Trim(),
                    IsConst = true,
                    IsStatic = matchConst.Groups["static"].Value != string.Empty,
                };
                this.Fields.Add(field);

                matchConst = matchConst.NextMatch();
            }

            Match matchVar = Regex.Match(this.m_fileText,
                                         FieldPattern,
                                         RegexOptions.Multiline);
            while (matchVar.Success) {
                var field = new FieldInfo {
                    Modifiers =
                        (AccessModifiers)
                        Enum.Parse(typeof(AccessModifiers),
                                   matchVar.Groups["acces"].Value,
                                   true),
                    Name = matchVar.Groups["name"].Value,
                    Type = matchVar.Groups["type"].Value.Split('.').Last(),
                    Value = matchVar.Groups["value"].Value.Replace("=", "").Trim(),
                    IsStatic = matchConst.Groups["static"].Value != string.Empty
                };
                this.Fields.Add(field);

                matchVar = matchVar.NextMatch();
            }
        }

        private void ParseMethods() {
            this.Methods = new List<MethodInfo>();
            this.Properties = new List<PropertyInfo>();

            Match matchMethods = Regex.Match(this.m_fileText,
                                             MethodPattern,
                                             RegexOptions.Multiline);
            while (matchMethods.Success) {
                // do not support properties
                if (!string.IsNullOrEmpty(matchMethods.Groups["prop"].Value)) {
                    matchMethods = matchMethods.NextMatch();

                    continue;
                }

                MethodInfo method = this.BuildMethodInfoFromMatch(matchMethods, false);
                method.Statements = this.BuildMethodElementsFromMatch(matchMethods).ToList();
                if (method.Statements.Count == 1 && method.Statements.First() is AssignationStatement) { }

                this.Methods.Add(method);

                matchMethods = matchMethods.NextMatch();
            }
        }

        private MethodInfo BuildMethodInfoFromMatch(Match match, bool constructor) {
            var method = new MethodInfo {
                AccessModifier =
                    (AccessModifiers)
                    Enum.Parse(typeof(AccessModifiers),
                               match.Groups["acces"].Value,
                               true),
                Name = match.Groups["name"].Value,
                Modifiers = match.Groups["override"].Value == "override"
                                ? new List<MethodInfo.MethodModifiers>(new[] {MethodInfo.MethodModifiers.Override})
                                : new List<MethodInfo.MethodModifiers>(new[] {MethodInfo.MethodModifiers.None}),
                ReturnType = constructor ? "" : match.Groups["returntype"].Value,
            };

            var args = new List<Argument>();
            foreach (object capture in match.Groups["argument"].Captures) {
                var arg = new Argument();

                string argStr = capture.ToString().Trim().Replace(",", "");

                arg.Name = argStr.Split(':').First().Trim();

                if (argStr.Contains("<")) {
                    string generictype = argStr.Split('<').Last().Split('>').First().Split('.').Last();

                    arg.Type = "List<" + generictype + ">";
                }
                else
                    arg.Type = argStr.Split(':').Last().Split('.').Last().Trim();

                if (arg.Type.Contains("=")) {
                    arg.DefaultValue = arg.Type.Split('=').Last().Trim();
                    arg.Type = arg.Type.Split('=').First().Trim();
                }
                else if (!string.IsNullOrEmpty(args.LastOrDefault().DefaultValue)) {
                    arg.DefaultValue = "null";
                }

                args.Add(arg);
            }

            method.Arguments = args.ToArray();

            if (!string.IsNullOrEmpty(match.Groups["prop"].Value)) {
                PropertyInfo property;

                IEnumerable<PropertyInfo> propertiesExisting;
                if ((propertiesExisting = this.Properties.Where(entry => entry.Name == method.Name)).Count() > 0) {
                    property = propertiesExisting.First();
                }
                else {
                    property = new PropertyInfo {
                        Name = method.Name,
                        AccessModifier = method.AccessModifier,
                    };
                }

                if (match.Groups["prop"].Value == "get") {
                    property.MethodGet = method;
                    property.PropertyType = method.ReturnType;
                }
                else if (match.Groups["prop"].Value == "set") {
                    property.MethodSet = method;
                }
            }

            return method;
        }

        private IEnumerable<IStatement> BuildMethodElementsFromMatch(Match match) {
            int bracketOpen =
                Array.FindIndex(this.m_fileLines, (entry) => entry.Contains(match.Groups[0].Value));
            if (!this.m_fileLines[bracketOpen].EndsWith("{"))
                bracketOpen++;
            int bracketClose = this.m_brackets[bracketOpen];

            var methodlines = new string[(bracketClose - 1) - bracketOpen];

            Array.Copy(this.m_fileLines, bracketOpen + 1, methodlines, 0, (bracketClose - 1) - bracketOpen);

            return this.ParseMethodExecutions(methodlines);
        }

        private static Dictionary<int, int> FindBracketsIndexesByLines(string[] lines, char startDelimter, char endDelemiter) {
            var elementsStack = new Stack<int>();
            var result = new Dictionary<int, int>();

            for (int i = 0; i < lines.Length; i++) {
                if (lines[i].Contains(startDelimter))
                    elementsStack.Push(i);

                if (lines[i].Contains(endDelemiter)) {
                    int index = elementsStack.Pop();

                    result.Add(index, i);
                }
            }

            if (elementsStack.Count > 0)
                foreach (int i in elementsStack) {
                    throw new Exception(string.Format("Bracket '{0}' at index ", startDelimter) + i + " is not closed");
                }

            return result;
        }

        private IEnumerable<IStatement> ParseMethodExecutions(IEnumerable<string> lines) {
            var result = new List<IStatement>();

            int controlsequenceDepth = 0;
            foreach (string line in lines.Select(entry => entry.Trim())) {
                if (this.IsLineIgnored(line))
                    continue;

                if (line == "{")
                    continue;

                if (line == "}") {
                    if (controlsequenceDepth > 0) {
                        result.Add(new ControlStatementEnd());
                        controlsequenceDepth--;
                    }

                    continue;
                }


                IStatement statement;
                if (this.Class.Name == "FightCommonInformations") { }

                if (Regex.IsMatch(line, ControlStatement.Pattern)) {
                    statement = ControlStatement.Parse(line);
                    controlsequenceDepth++;
                }

                else if (Regex.IsMatch(line, AssignationStatement.Pattern)) {
                    statement = AssignationStatement.Parse(line);
                }

                else if (Regex.IsMatch(line, InvokeExpression.Pattern)) {
                    statement = InvokeExpression.Parse(line);
                    if (!string.IsNullOrEmpty((statement as InvokeExpression).ReturnVariableAssignation)
                        && string.IsNullOrEmpty((statement as InvokeExpression).Preffix)
                        && this.Fields.Count(entry => entry.Name == ((InvokeExpression) statement).ReturnVariableAssignation) > 0) {
                        (statement as InvokeExpression).Preffix = "("
                                                                  + this.Fields.Where(entry =>
                                                                                          entry.Name == ((InvokeExpression) statement).ReturnVariableAssignation)
                                                                        .First()
                                                                        .Type
                                                                  + ")"; // cast
                    }

                    // cast to generic type
                    if (!string.IsNullOrEmpty((statement as InvokeExpression).Target)
                        && (statement as InvokeExpression).Name == "Add"
                        && this.Fields.Count(entry => entry.Name == ((InvokeExpression) statement).Target.Split('.').Last()) > 0) {
                        string generictype = this.Fields.Where(entry => entry.Name == ((InvokeExpression) statement).Target.Split('.').Last()).First().Type.Split('<').Last().Split('>').First();

                        (statement as InvokeExpression).Args[0] = "(" + generictype + ")" + (statement as InvokeExpression).Args[0];
                    }
                }

                else
                    statement = new UnknownStatement {
                        Value = line
                    };

                result.Add(statement);
            }

            return result;
        }

        private string GetMatch(string pattern, int index = 1) {
            //      return m_fileLines.ToList().Find(x => x.Contains("public class")).Split(null).Last(); // USED FOR ENUMS
            var matchedLine = this.m_fileLines.ToList().Find(entry => Regex.IsMatch(entry, pattern, RegexOptions.None));

            if (matchedLine == null)
                return "";

            Match match = Regex.Match(matchedLine, pattern, RegexOptions.Multiline);

            return match.Groups[index].Value;
        }

        private static string ApplyRules(IEnumerable<KeyValuePair<string, string>> rules, string str) {
            if (rules == null)
                return str;

            if (string.IsNullOrEmpty(str))
                return str;

            foreach (var rule in rules) {
                var replace = Regex.Replace(str, rule.Key, rule.Value);

                str = replace;
            }

            return str;
        }

        private bool IsLineIgnored(string line) {
            return this.IgnoredLines != null && this.IgnoredLines.Any(rule => Regex.IsMatch(line, rule));
        }
    }

    public class InvalidCodeFileException : Exception {
        public InvalidCodeFileException(string thisFileDoesNotContainAClass)
            : base(thisFileDoesNotContainAClass) { }
    }
}