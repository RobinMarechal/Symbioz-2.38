namespace Symbioz.World.Models.Items {
    public class DropItem {
        public int Id { get; private set; }

        public ushort Quantity { get; set; }

        public Records.Characters.CharacterItemRecord Record { get; set; }

        public ushort CellId { get; set; }

        public Maps.Instances.AbstractMapInstance Map { get; private set; }

        private Core.ActionTimer Timer;

        public bool PickedUp { get; private set; }

        public DropItem(Records.Characters.CharacterItemRecord record, ushort quantity, ushort cellid, Maps.Instances.AbstractMapInstance map) {
            this.Id = map.PopNextDropItemId();
            this.Record = record;
            this.Map = map;
            this.CellId = cellid;
            this.Quantity = quantity;
            this.Timer = new Core.ActionTimer(900000, this.Remove, false);
            this.PickedUp = false;
        }

        public void OnPickUp(Entities.Character character) {
            if (!this.PickedUp) {
                this.PickedUp = true;
                var record = this.Record.CloneWithoutUID().ToCharacterItemRecord(character.Id);
                character.Inventory.AddItem(record, this.Quantity);
                this.Remove();
            }
        }

        public static DropItem Create(Records.Characters.CharacterItemRecord record, ushort quantity, ushort cellid, Maps.Instances.AbstractMapInstance map) {
            return new DropItem(record, quantity, cellid, map);
        }

        private void Remove() {
            this.Map.RemoveDropItem(this);
            this.Timer.Stop();
            this.Timer = null;
        }
    }
}