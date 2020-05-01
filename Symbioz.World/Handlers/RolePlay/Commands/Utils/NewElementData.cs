using Symbioz.World.Records.Interactives;

namespace Symbioz.World.Handlers.RolePlay.Commands.Utils {
    public class NewElementData {
        public int MapId { get; }
        public int ElementId { get; }
        public string ActionType { get; }
        public int ElementType { get; }
        public ushort SkillId { get; }
        public string Value1 { get; }
        public string Value2 { get; }

        public NewElementData(int mapId, int elementId, int elementType, string actionType, ushort skillId, string value1 = "", string value2 = "") {
            this.MapId = mapId;
            this.ElementId = elementId;
            this.ActionType = actionType;
            this.ElementType = elementType;
            this.SkillId = skillId;
            this.Value1 = value1;
            this.Value2 = value2;
        }

        public bool InteractiveSkillAlreadyExists() {
            return InteractiveSkillRecord.InteractiveSkills.Exists(rec => rec.ActionType.Equals(this.ActionType)
                                                                          && rec.ElementId == this.ElementId
                                                                          && rec.SkillId == this.SkillId);
        }
    }
}