using System.Collections.Generic;
using Symbioz.ORM;

namespace Symbioz.World.Records.Interactives {
    [Table("Interactives")]
    public class InteractiveRecord : ITable {
        public static List<InteractiveRecord> Interactives = new List<InteractiveRecord>();

        [Primary]
        public int Id;

        public string Name;

        public int ActionId;

        public InteractiveRecord(int id, string name, int actionId) {
            this.Id = id;
            this.Name = name;
            this.ActionId = actionId;
        }
        
        

        public static InteractiveRecord GetInteractive(ushort id) {
            return Interactives.Find(x => x.Id == id);
        }
    }
}