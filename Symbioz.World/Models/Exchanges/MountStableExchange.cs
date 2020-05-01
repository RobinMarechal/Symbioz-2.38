using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Types;
using Symbioz.World.Models.Entities;
using Symbioz.World.Providers.Items;
using Symbioz.World.Records;
using Symbioz.World.Records.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Exchanges {
    public class MountStableExchange : Exchange {
        public override ExchangeTypeEnum ExchangeType {
            get { return ExchangeTypeEnum.MOUNT_STABLE; }
        }

        public MountStableExchange(Character character)
            : base(character) { }

        public override void Open() {
            this.Character.Client.Send(new PaddockPropertiesMessage(new PaddockInformations(10, 10)));
            this.Character.Client.Send(new ExchangeStartOkMountMessage(new MountClientData[0], new MountClientData[0]));
        }

        public void HandleMountStable(sbyte actionType, uint[] ridesId) {
            if (actionType == 15) // Equiper la monture
            {
                if (this.Character.Inventory.HasMountEquiped) {
                    this.UnequipMount(this.Character.Inventory.Mount.UId);
                }

                this.EquipMount(ridesId[0]);
            }

            if (actionType == 13) // Obtenir un certificat de la monture
            {
                this.UnequipMount(ridesId[0]);
            }
        }

        private void EquipMount(uint itemUId) {
            CharacterItemRecord item = this.Character.Inventory.GetItem(itemUId);
            CharacterMountRecord mount = this.Character.Inventory.GetMount(item);
            this.Character.Inventory.SetMount(mount, item);
        }

        private void UnequipMount(long mountUId) {
            CharacterMountRecord mount = this.Character.Inventory.GetMount(mountUId);
            CharacterItemRecord item = mount.CreateCertificate(this.Character);
            this.Character.Inventory.AddItem(item);
            this.Character.Inventory.UnsetMount();
        }

        public override void MoveItem(uint uid, int quantity) {
            throw new NotImplementedException();
        }

        public override void Ready(bool ready, ushort step) {
            throw new NotImplementedException();
        }

        public override void MoveKamas(int quantity) {
            throw new NotImplementedException();
        }
    }
}