using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.DofusConfig.LineParser {
    public struct ConfigLineField {
        public string m_key;

        public string m_value;

        public int LineIndex { get; private set; }

        public string Line;

        public string Key {
            get { return this.m_key; }
            set {
                this.m_key = value;
                this.Update();
            }
        }

        public string Value {
            get { return this.m_value; }
            set {
                this.m_value = value;
                this.Update();
            }
        }

        private DofusConfig Config { get; set; }

        public ConfigLineField(DofusConfig config, string line, int lineIndex) {
            this.Line = line;
            this.LineIndex = lineIndex;
            this.Config = config;
            this.m_key = this.Line.Split('\"')[1];
            this.m_value = this.Line.Split('>')[1].Split('<')[0];
        }

        private void Update() {
            this.Line = string.Format("\t<entry key=\"{0}\">{1}</entry>", this.Key, this.Value);
            this.Config.Lines[this.LineIndex] = this.Line;
        }

        internal static bool IsValid(string line) {
            if (line.Contains("</entry>") && line.Contains("key=")) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}