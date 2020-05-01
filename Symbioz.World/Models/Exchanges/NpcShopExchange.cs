using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Selfmade.Enums;
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
    public class NpcShopExchange : Exchange
    {
        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.NPC_SHOP;
            }
        }

        public Npc Npc
        {
            get;
            set;
        }

        public ItemRecord[] ItemToSell
        {
            get;
            set;
        }

        public ushort TokenId
        {
            get;
            set;
        }

        private bool LevelPrice
        {
            get;
            set;
        }
        public NpcShopExchange(Character character, Npc npc, ItemRecord[] itemToSell, ushort tokenId, bool levelPrice)
            : base(character)
        {
            this.Npc = npc;
            this.ItemToSell = itemToSell;
            this.TokenId = tokenId;
            this.LevelPrice = levelPrice;
        }

        public override void Open()
        {
            ObjectItemToSellInNpcShop[] items = Array.ConvertAll<ItemRecord, ObjectItemToSellInNpcShop>(this.ItemToSell, x => x.GetObjectItemToSellInNpcShop(this.LevelPrice));
            this.Character.Client.Send(new ExchangeStartOkNpcShopMessage(this.Npc.Id, this.TokenId, items));
        }

        public void Buy(ushort gid, uint quantity)
        {
            ItemRecord template = this.ItemToSell.FirstOrDefault(x => x.Id == gid);

            if (template != null)
            {
                if (this.TokenId == 0)
                {
                    int cost = (int)(template.GetPrice(this.LevelPrice) * quantity);

                    if (!this.Character.RemoveKamas(cost))
                        return;
                }
                else
                {
                    CharacterItemRecord tokenItem = this.Character.Client.Character.Inventory.GetFirstItem(this.TokenId, (uint)(template.GetPrice(this.LevelPrice) * quantity));

                    if (tokenItem == null)
                    {
                        this.Character.Client.Character.ReplyError("Vous ne possedez pas asser de token.");
                        return;
                    }
                    else
                    {
                        this.Character.Inventory.RemoveItem(tokenItem.UId, (uint)(quantity * template.GetPrice(this.LevelPrice)));
                    }
                }

                this.Character.Inventory.AddItem(gid, quantity, this.TokenId != 0);
                this.Character.Client.Send(new ExchangeBuyOkMessage());
            }

        }
        public void Sell(uint uid, uint quantity)
        {
            CharacterItemRecord item = this.Character.Inventory.GetItem(uid);

            if (item != null && item.CanBeExchanged() && item.Quantity >= quantity)
            {
                int gained = (int)(((double)item.Template.GetPrice(this.LevelPrice) / (double)10) * quantity);

                if (gained >= item.Template.GetPrice(this.LevelPrice))
                {
                    return;
                }

                gained = gained == 0 ? 1 : gained;

                this.Character.Inventory.RemoveItem(uid, quantity);

                if (this.TokenId == 0)
                {
                    this.Character.AddKamas(gained);
                }
                else
                {
                    this.Character.Inventory.AddItem(this.TokenId, (uint)gained);
                }

                this.Character.Client.Send(new ExchangeSellOkMessage());
            }
        }
        public override void MoveItem(uint uid, int quantity)
        {
            throw new NotImplementedException();
        }

        public override void Ready(bool ready, ushort step)
        {
            throw new NotImplementedException();
        }

        public override void MoveKamas(int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
