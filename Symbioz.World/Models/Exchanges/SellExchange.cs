using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Types;
using Symbioz.World.Models.Entities;
using Symbioz.World.Records.Characters;
using Symbioz.World.Records.Items;
using Symbioz.World.Records.Npcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Exchanges
{
    public class SellExchange : Exchange
    {
        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.BIDHOUSE_SELL;
            }
        }

        private BidShopRecord BidShop
        {
            get;
            set;
        }

        private Npc Npc
        {
            get;
            set;
        }

        private List<BidShopItemRecord> SelledItems
        {
            get;
            set;
        }

        public SellExchange(Character character, Npc npc, BidShopRecord bidShop)
            : base(character)
        {
            this.BidShop = bidShop;
            this.Npc = npc;

        }
        public void MoveItemPriced(uint uid, int quantity, uint price)
        {
            CharacterItemRecord item = this.Character.Inventory.GetItem(uid);

            if (item != null && item.Quantity >= quantity && item.CanBeExchanged())
            {
                BidShopItemRecord selledItem = item.ToBidShopItemRecord(this.BidShop.Id, this.Character.Client.Account.Id, price);
                selledItem.Quantity = (uint)quantity;
                this.Character.Inventory.RemoveItem(item.UId, (uint)quantity);
                this.AddSelledItem(selledItem);
            }
        }
        public void ModifyItemPriced(uint uid, int quantity, uint price)
        {
            BidShopItemRecord item = this.GetSelledItem(uid);

            if (item != null)
            {
                item.Price = price;
                item.Quantity = (uint)quantity;
                item.UpdateElement();
                this.Open();
            }
        }
        public void AddSelledItem(BidShopItemRecord item)
        {
            item.AddElement();
            this.SelledItems.Add(item);
            this.Character.Client.Send(new ExchangeBidHouseItemAddOkMessage(item.GetObjectItemToSellInBid()));

        }
        public override void MoveItem(uint uid, int quantity)
        {
            if (quantity < 0)
            {
                BidShopItemRecord item = this.GetSelledItem(uid);

                if (item != null && item.Quantity >= Math.Abs(quantity))
                {
                    item.RemoveElement();
                    this.SelledItems.Remove(item);
                    this.Character.Inventory.AddItem(item.ToCharacterItemRecord(this.Character.Id));
                    this.Character.Client.Send(new ExchangeBidHouseItemRemoveOkMessage((int)uid));
                }
            }
        }
        public BidShopItemRecord GetSelledItem(uint uid)
        {
            return this.SelledItems.Find(x => x.UId == uid);
        }
        public override void Ready(bool ready, ushort step)
        {
            throw new NotImplementedException();
        }

        public override void MoveKamas(int quantity)
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            this.SelledItems = BidShopItemRecord.GetSellerItems(this.BidShop.Id, this.Character.Client.Account.Id);
            this.Character.Client.Send(new ExchangeStartedBidSellerMessage(this.BidShop.GetBuyerDescriptor((int) this.Npc.Id),
                                                                           this.SelledItems.ConvertAll<ObjectItemToSellInBid>(x => x.GetObjectItemToSellInBid()).ToArray()));
        }

    }
}
