using Symbioz.World.Models.Entities;
using Symbioz.World.Network;

namespace Symbioz.World.Handlers.RolePlay.Commands.Utils {
    public class ReplyUtils {
        public static void SendReply(WorldClient client, string text) {
            foreach (string line in text.Split('\n')) {
                client.Character.Reply(line);
            }
        }
        
        
    }
}