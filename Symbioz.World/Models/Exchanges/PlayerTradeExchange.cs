using SSync.Messages;
using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Entities;
using Symbioz.World.Models.Items;
using Symbioz.World.Records;
using Symbioz.World.Records.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Exchanges {
    public class PlayerTradeExchange : Exchange {
        public override ExchangeTypeEnum ExchangeType {
            get { return ExchangeTypeEnum.PLAYER_TRADE; }
        }

        private ItemCollection<CharacterItemRecord> ExchangedItems = new ItemCollection<CharacterItemRecord>();

        private bool IsReady = false;

        private int MovedKamas = 0;

        public Character SecondTrader { get; set; }

        public PlayerTradeExchange(Character character, Character secondCharacter)
            : base(character) {
            this.SecondTrader = secondCharacter;

            this.ExchangedItems.OnItemAdded += this.ExchangedItems_OnItemAdded;
            this.ExchangedItems.OnItemRemoved += this.ExchangedItems_OnItemRemoved;
            this.ExchangedItems.OnItemStacked += this.ExchangedItems_OnItemStacked;
            this.ExchangedItems.OnItemUnstacked += this.ExchangedItems_OnItemUnstacked;
        }

        #region Events

        void ExchangedItems_OnItemUnstacked(CharacterItemRecord arg1, uint arg2) {
            this.OnObjectModified(arg1);
        }

        void ExchangedItems_OnItemStacked(CharacterItemRecord arg1, uint arg2) {
            this.OnObjectModified(arg1);
        }

        void ExchangedItems_OnItemRemoved(CharacterItemRecord obj) {
            this.Character.Client.Send(new ExchangeObjectRemovedMessage(false, obj.UId));
            this.SecondTrader.Client.Send(new ExchangeObjectRemovedMessage(true, obj.UId));
        }

        void ExchangedItems_OnItemAdded(CharacterItemRecord obj) {
            this.Character.Client.Send(new ExchangeObjectAddedMessage(false, obj.GetObjectItem()));
            this.SecondTrader.Client.Send(new ExchangeObjectAddedMessage(true, obj.GetObjectItem()));
        }

        private void OnObjectModified(CharacterItemRecord obj) {
            this.Character.Client.Send(new ExchangeObjectModifiedMessage(false, obj.GetObjectItem()));
            this.SecondTrader.Client.Send(new ExchangeObjectModifiedMessage(true, obj.GetObjectItem()));
        }

        #endregion


        public override void Open() {
            this.Send(new ExchangeStartedWithPodsMessage((sbyte) this.ExchangeType,
                                                         this.Character.Id,
                                                         this.Character.Inventory.CurrentWeight,
                                                         this.Character.Inventory.TotalWeight,
                                                         this.SecondTrader.Id,
                                                         this.SecondTrader.Inventory.CurrentWeight,
                                                         this.SecondTrader.Inventory.TotalWeight));
        }

        public void Send(Message message) {
            this.Character.Client.Send(message);
            this.SecondTrader.Client.Send(message);
        }

        public override void Close() {
            this.SecondTrader.Client.Send(new ExchangeLeaveMessage((sbyte) this.DialogType, this.Succes));
            this.SecondTrader.Dialog = null;

            this.Character.Client.Send(new ExchangeLeaveMessage((sbyte) this.DialogType, this.Succes));
            this.Character.Dialog = null;
        }

        private bool CanMoveItem(CharacterItemRecord item, int quantity) {
            if (item.PositionEnum != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED || !item.CanBeExchanged())
                return false;

            CharacterItemRecord exchanged = null;

            exchanged = this.ExchangedItems.GetItem(item.GId, item.Effects);

            if (exchanged != null && exchanged.UId != item.UId)
                return false;

            exchanged = this.ExchangedItems.GetItem(item.UId);

            if (exchanged == null) {
                return true;
            }

            if (exchanged.Quantity + quantity > item.Quantity)
                return false;
            else
                return true;
        }

        public override void MoveItem(uint uid, int quantity) {
            if (!this.IsReady) {
                CharacterItemRecord item = this.Character.Inventory.GetItem(uid);

                if (item != null && this.CanMoveItem(item, quantity)) {
                    if (this.SecondTrader.GetDialog<PlayerTradeExchange>().IsReady) {
                        this.SecondTrader.GetDialog<PlayerTradeExchange>().Ready(false, 0);
                    }

                    if (quantity > 0) {
                        if (item.Quantity >= quantity) {
                            this.ExchangedItems.AddItem(item, (uint) quantity);
                        }
                    }
                    else {
                        this.ExchangedItems.RemoveItem(item.UId, (uint) (Math.Abs(quantity)));
                    }
                }
            }
        }

        public override void Ready(bool ready, ushort step) {
            this.IsReady = ready;

            this.Send(new ExchangeIsReadyMessage(this.Character.Id, this.IsReady));

            if (this.IsReady && this.SecondTrader.GetDialog<PlayerTradeExchange>().IsReady) {
                foreach (var item in this.ExchangedItems.GetItems()) {
                    item.CharacterId = this.SecondTrader.Id;
                    this.SecondTrader.Inventory.AddItem((CharacterItemRecord) item.CloneWithoutUID());
                    this.Character.Inventory.RemoveItem(item.UId, item.Quantity);
                }

                foreach (var item in this.SecondTrader.GetDialog<PlayerTradeExchange>().ExchangedItems.GetItems()) {
                    item.CharacterId = this.Character.Id;
                    this.Character.Inventory.AddItem((CharacterItemRecord) item.CloneWithoutUID());
                    this.SecondTrader.Inventory.RemoveItem(item.UId, item.Quantity);
                }

                this.SecondTrader.AddKamas(this.MovedKamas);
                this.Character.RemoveKamas(this.MovedKamas);

                this.Character.AddKamas(this.SecondTrader.GetDialog<PlayerTradeExchange>().MovedKamas);
                this.SecondTrader.RemoveKamas(this.SecondTrader.GetDialog<PlayerTradeExchange>().MovedKamas);

                this.Succes = true;
                this.Close();
            }
        }

        public override void MoveKamas(int quantity) {
            if (quantity <= this.Character.Record.Kamas) {
                if (this.IsReady) {
                    this.Ready(false, 0);
                }

                if (this.SecondTrader.GetDialog<PlayerTradeExchange>().IsReady) {
                    this.SecondTrader.GetDialog<PlayerTradeExchange>().Ready(false, 0);
                }


                this.SecondTrader.Client.Send(new ExchangeKamaModifiedMessage(true, (uint) quantity));
                this.MovedKamas = quantity;
            }
        }
    }
}