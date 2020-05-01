using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Core.DesignPattern.StartupEngine;
using Symbioz.World.Network;

namespace Symbioz.World.Handlers.RolePlay.Commands {
    class CommandsHandler {
        public const string COMMANDS_PREFIX = ".";

        public static Dictionary<Command, Delegate> Commands = new Dictionary<Command, Delegate>();

        [StartupInvoke("InGame Commands", StartupInvokePriority.Eighth)]
        public static void LoadChatCommands() {
            foreach (var item in Program.WorldAssembly.GetTypes()) {
                foreach (var subItem in item.GetMethods()) {
                    var attribute = subItem.GetCustomAttributes(typeof(ChatCommand), false).FirstOrDefault() as ChatCommand;
                    if (attribute != null) {
                        Delegate del = Delegate.CreateDelegate(typeof(Action<string, WorldClient>), subItem);
                        Commands.Add(new Command(attribute.Name, attribute.Role), del);
                    }
                }
            }
        }

        public static void Handle(string content, WorldClient client) {
            var comInfo = content.Split(null).ToList()[0];
            foreach (var cmdKey in Commands.Keys) {
                var cmd = Commands.ToList().Find(x => x.Key.Value.ToLower() == comInfo.Split('.')[1].ToLower());
                if (cmd.Key == null) {
                    client.Character.Reply("La commande " + comInfo.Split('.')[1] + " n'éxiste pas");

                    return;
                }

                if (client.Account.Role < cmd.Key.MinimumRoleRequired) {
                    client.Character.Reply("Vous n'avez pas les droits pour executer cette commande");

                    break;
                }
                else if (cmdKey != null) {
                    var action = Commands.First(x => x.Key.Value.ToLower() == comInfo.Split('.')[1].ToLower());
                    var param = content.Split(null).ToList();
                    param.Remove(param[0]);
                    if (param.Count > 0) {
                        try {
                            action.Value.DynamicInvoke(string.Join(" ", param), client);
                        }
                        catch (Exception ex) {
                            client.Character.ReplyError($"Impossible d'executer la commande ({ex.GetType()})");
                            Console.Error.WriteLine(ex.ToString());
                        }
                    }
                    else {
                        try {
                            action.Value.DynamicInvoke(null, client);
                        }
                        catch (Exception ex) {
                            client.Character.ReplyError($"Impossible d'executer la commande ({ex.GetType()})");
                            Console.Error.WriteLine(ex.ToString());
                        }
                    }

                    break;
                }
            }
        }
    }
}