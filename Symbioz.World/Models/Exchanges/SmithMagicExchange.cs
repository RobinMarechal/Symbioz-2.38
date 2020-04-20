using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Entities;
using Symbioz.World.Models.Entities.Jobs;
using Symbioz.World.Models.Items;
using Symbioz.World.Records.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Exchanges {
    public class SmithMagicExchange : AbstractCraftExchange {
        public static ItemTypeEnum RuneType = ItemTypeEnum.RUNE_DE_FORGEMAGIE;

        private CharacterItemRecord RuneItem { get; set; }

        private EffectInteger RuneEffect {
            get { return this.RuneItem.FirstEffect<EffectInteger>(); }
        }

        private CharacterItemRecord Item {
            get { return this.CraftedItems.GetItems().First(); }
        }

        public SmithMagicExchange(Character character, uint skillId, JobsTypeEnum jobType)
            : base(character, skillId, jobType) { }

        public override void MoveItem(uint uid, int quantity) {
            CharacterItemRecord item = this.Character.Inventory.GetItem(uid);

            if (item.Template.TypeEnum == RuneType) {
                if (quantity > 0) {
                    this.RuneItem = item;
                }
                else {
                    this.RuneItem = null;
                }
            }

            {
                base.MoveItem(uid, quantity);
            }
        }

        public override void Ready(bool ready, ushort step) {
            for (int i = 0; i < this.RuneItem.Quantity; i++) {
                if (this.RuneEffect != null) {
                    this.Item.AddEffectInteger(this.RuneEffect.EffectEnum, this.RuneEffect.Value);
                    this.OnSucces();
                    this.Character.Inventory.OnItemModified(this.Item);
                    this.Character.Inventory.RemoveItem(this.RuneItem.UId, 1);
                }
                else {
                    this.OnFail();
                }
            }
        }

        private void OnSucces() {
            this.Character.Client.Send(new ExchangeCraftResultMagicWithObjectDescMessage((sbyte) CraftResultEnum.CRAFT_SUCCESS,
                                                                                         this.Item.GetObjectItemNotInContainer(),
                                                                                         1));
        }

        private void OnFail() {
            this.Character.Client.Send(new ExchangeCraftResultMessage((sbyte) CraftResultEnum.CRAFT_FAILED));
        }

        public override void SetCount(int count) { }

        public override void MoveKamas(int quantity) {
            throw new NotImplementedException();
        }
    }
}