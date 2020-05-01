using Symbioz.World.Network;
using Symbioz.World.Records.Interactives;
using Symbioz.World.Records.Maps;

namespace Symbioz.World.Handlers.RolePlay.Commands.Utils {
    public class InteractiveElementsUtils {
        public static void CreateInteractiveElement(NewElementData data, WorldClient client) {
            if (!data.InteractiveSkillAlreadyExists()) {
                // Create Paddock record in InteractiveSkills table.
                new InteractiveSkillRecord(data.ActionType, data.Value1, data.Value2, data.ElementId, data.SkillId).AddInstantElement();
            }

            // Define associated InteractiveElement as a Paddock (ElementType = 120) 
            var iElement = InteractiveElementRecord.InteractiveElements.Find(el => el.ElementId == data.ElementId && el.MapId == data.MapId);
            iElement.ElementType = data.ElementType;
            iElement.UpdateInstantElement();

            var map = MapRecord.Maps.Find(rec => rec.Id == data.MapId);
            map.Instance.Reload();

            client.Character.Reply($"Element {data.ElementId} successfully added on map ({map.X},{map.Y}), CellId={iElement.CellId}, "
                                   + $"ActionType={data.ActionType}, ElementType={data.ElementType}, Value1={data.Value1}, Value2={data.Value2}.");
        }
    }
}