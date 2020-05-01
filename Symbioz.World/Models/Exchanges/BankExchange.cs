using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.World.Models.Entities;
using Symbioz.World.Models.Items;
using Symbioz.World.Records.Characters;
using Symbioz.World.Records.Items;

namespace Symbioz.World.Models.Exchanges {
    public class BankExchange : AbstractTradeExchange {
        public override ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.BANK;

        public const uint StorageMaxSlot = 300;

        private ItemCollection<BankItemRecord> m_items;

        public BankExchange(Character character, List<BankItemRecord> bankItems) : base(character) {
            this.m_items = new ItemCollection<BankItemRecord>(bankItems);
            this.m_items.OnItemAdded += this.m_items_OnItemAdded;
            this.m_items.OnItemRemoved += this.m_items_OnItemRemoved;
            this.m_items.OnItemStacked += this.m_items_OnItemStacked;
            this.m_items.OnItemUnstacked += this.m_items_OnItemUnstacked;
        }

        void m_items_OnItemUnstacked(BankItemRecord arg1, uint arg2) {
            arg1.UpdateElement();
            this.Character.Client.Send(new StorageObjectUpdateMessage(arg1.GetObjectItem()));
        }

        void m_items_OnItemStacked(BankItemRecord arg1, uint arg2) {
            arg1.UpdateElement();
            this.Character.Client.Send(new StorageObjectUpdateMessage(arg1.GetObjectItem()));
        }

        void m_items_OnItemRemoved(BankItemRecord obj) {
            obj.RemoveElement();
            this.Character.Client.Send(new StorageObjectRemoveMessage(obj.UId));
        }

        void m_items_OnItemAdded(BankItemRecord obj) {
            obj.AddElement();
            this.Character.Client.Send(new StorageObjectUpdateMessage(obj.GetObjectItem()));
        }

        public override void Open() {
            this.Character.Client.Send(new ExchangeStartedWithStorageMessage((sbyte) this.ExchangeType, StorageMaxSlot));
            this.Character.Client.Send(new StorageInventoryContentMessage(this.m_items.GetObjectsItems(), this.Character.Client.AccountInformations.BankKamas));
        }

        
        public override IEnumerable<ItemStack> GetAllPresentItems() {
            return this.m_items.GetItems().ToList().ConvertAll(x => new ItemStack(x.UId, x.Quantity));
        }

        public override void MoveItem(uint uid, int quantity) {
            if (quantity > 0) {
                CharacterItemRecord item = this.Character.Inventory.GetItem(uid);

                if (item != null && item.Quantity >= quantity && item.CanBeExchanged()) {
                    BankItemRecord bankItem = item.ToBankItemRecord(this.Character.Client.Account.Id);
                    bankItem.Quantity = (uint) quantity;
                    this.Character.Inventory.RemoveItem(item.UId, (uint) quantity);
                    this.m_items.AddItem(bankItem);
                }
            }
            else {
                BankItemRecord item = this.m_items.GetItem(uid);
                uint removedQuantity = (uint) Math.Abs(quantity);

                if (item != null && item.Quantity >= removedQuantity) {
                    CharacterItemRecord characterItemRecord = item.ToCharacterItemRecord(this.Character.Id);
                    characterItemRecord.Quantity = removedQuantity;
                    this.m_items.RemoveItem(uid, removedQuantity);
                    this.Character.Inventory.AddItem(characterItemRecord);
                }
            }
        }

        public override void MoveKamas(int quantity) {
            if (quantity < 0) {
                if (this.Character.Client.AccountInformations.BankKamas >= Math.Abs(quantity))
                    this.Character.AddKamas(Math.Abs(quantity));
                else
                    return;
            }
            else {
                if (!this.Character.RemoveKamas(quantity))
                    return;
            }

            this.Character.Client.AccountInformations.BankKamas += (uint) quantity;
            this.Character.Client.AccountInformations.UpdateElement();
            this.Character.Client.Send(new StorageKamasUpdateMessage((int) this.Character.Client.AccountInformations.BankKamas));
        }

        public override void Ready(bool ready, ushort step) {
            throw new NotImplementedException();
        }
    }
}