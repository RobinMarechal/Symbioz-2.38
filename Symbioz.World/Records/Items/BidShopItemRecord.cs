using Symbioz.ORM;
using Symbioz.Protocol.Types;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Items;
using Symbioz.World.Providers.Items;
using Symbioz.World.Records.Characters;
using Symbioz.World.Records.Npcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Items {
    [Table("BidShopItems"), Resettable]
    public class BidShopItemRecord : AbstractItem, ITable {
        public static List<BidShopItemRecord> BidShopItems = new List<BidShopItemRecord>();

        public int BidShopId;

        public int AccountId;

        public uint Price;

        public BidShopItemRecord(int bidshopId,
                                 int accountId,
                                 uint price,
                                 uint uid,
                                 ushort gid,
                                 byte position,
                                 uint quantity,
                                 List<Effect> effects,
                                 ushort appearanceId) {
            this.UId = uid;
            this.GId = gid;
            this.Position = position;
            this.Quantity = quantity;
            this.Effects = effects;
            this.AppearanceId = appearanceId;
            this.BidShopId = bidshopId;
            this.AccountId = accountId;
            this.Price = price;
        }

        public ObjectItemToSellInBid GetObjectItemToSellInBid() {
            return new ObjectItemToSellInBid(this.GId,
                                             this.Effects.ConvertAll<ObjectEffect>(x => x.GetObjectEffect()).ToArray(),
                                             this.UId,
                                             this.Quantity,
                                             this.Price,
                                             50);
        }

        public BidExchangerObjectInfo GetBidExchangerObjectInfo(int[] prices) {
            return new BidExchangerObjectInfo(this.UId, this.Effects.ConvertAll<ObjectEffect>(x => x.GetObjectEffect()).ToArray(), prices);
        }

        public static List<BidShopItemRecord> GetSellerItems(int bidshopId, long accountId) {
            return BidShopItems.FindAll(x => x.BidShopId == bidshopId && x.AccountId == accountId);
        }

        public static List<BidShopItemRecord> GetBidShopItems(int bidshopId) {
            return BidShopItems.FindAll(x => x.BidShopId == bidshopId);
        }

        public override AbstractItem CloneWithUID() {
            return new BidShopItemRecord(this.BidShopId, this.AccountId, this.Price, this.UId, this.GId, this.Position, this.Quantity, new List<Effect>(this.Effects), this.AppearanceId);
        }

        public override AbstractItem CloneWithoutUID() {
            return new BidShopItemRecord(this.BidShopId,
                                         this.AccountId,
                                         this.Price,
                                         ItemUIdPopper.PopUID(),
                                         this.GId,
                                         this.Position,
                                         this.Quantity,
                                         new List<Effect>(this.Effects),
                                         this.AppearanceId);
        }
    }
}