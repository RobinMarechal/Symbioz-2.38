using SSync.Messages;
using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.World.Models.Dialogs.DialogBox;
using Symbioz.World.Models.Entities;
using Symbioz.World.Models.Exchanges;
using Symbioz.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Protocol.Selfmade.Messages;
using Symbioz.World.Models.Items;
using Symbioz.World.Records.Characters;
using Symbioz.World.Records.Items;

namespace Symbioz.World.Handlers.RolePlay.Exchanges {
    public class ExchangesHandler {
        [MessageHandler]
        public static void HandleExchangeCraftCount(ExchangeCraftCountRequestMessage message, WorldClient client) {
            if (client.Character.IsInExchange(ExchangeTypeEnum.CRAFT)) {
                client.Character.GetDialog<AbstractCraftExchange>().SetCount(message.count);
            }
        }

        [MessageHandler]
        public static void HandleExchangeSetCraftRecipe(ExchangeSetCraftRecipeMessage message, WorldClient client) {
            if (client.Character.IsInExchange(ExchangeTypeEnum.CRAFT)) {
                client.Character.GetDialog<CraftExchange>().SetRecipe(message.objectGID);
            }
        }

        [MessageHandler]
        public static void HandleBidHouseSearch(ExchangeBidHouseSearchMessage message, WorldClient client) {
            if (client.Character.IsInExchange(ExchangeTypeEnum.BIDHOUSE_BUY)) {
                client.Character.GetDialog<BuyExchange>().ShowList(message.genId);
            }
        }

        [MessageHandler]
        public static void HandleExchangeBidHouseBuy(ExchangeBidHouseBuyMessage message, WorldClient client) {
            if (client.Character.IsInExchange(ExchangeTypeEnum.BIDHOUSE_BUY)) {
                client.Character.GetDialog<BuyExchange>().Buy(message.uid, message.qty, message.price);
            }
        }

        [MessageHandler]
        public static void HandleExchangeBidhouseList(ExchangeBidHouseListMessage message, WorldClient client) {
            if (client.Character.IsInExchange(ExchangeTypeEnum.BIDHOUSE_BUY)) {
                client.Character.GetDialog<BuyExchange>().ShowList(message.id);
            }
        }

        [MessageHandler]
        public static void HandleExchangeBidhouseTypes(ExchangeBidHouseTypeMessage message, WorldClient client) {
            if (client.Character.IsInExchange(ExchangeTypeEnum.BIDHOUSE_BUY)) {
                client.Character.GetDialog<BuyExchange>().ShowTypes(message.type);
            }
        }

        [MessageHandler]
        public static void HandleExchangeObjectModifyPriced(ExchangeObjectModifyPricedMessage message, WorldClient client) {
            if (client.Character.IsInExchange(ExchangeTypeEnum.BIDHOUSE_SELL)) {
                client.Character.GetDialog<SellExchange>().ModifyItemPriced(message.objectUID, message.quantity, message.price);
            }
        }

        [MessageHandler]
        public static void HandleExchangeObjectMovePriced(ExchangeObjectMovePricedMessage message, WorldClient client) {
            if (client.Character.IsInExchange(ExchangeTypeEnum.BIDHOUSE_SELL)) {
                client.Character.GetDialog<SellExchange>().MoveItemPriced(message.objectUID, message.quantity, message.price);
            }
        }

        [MessageHandler]
        public static void HandleExchangeObjectMove(ExchangeObjectMoveMessage message, WorldClient client) {
            client.Character.GetDialog<Exchange>().MoveItem(message.objectUID, message.quantity);
        }

        #region TradeExchanges

        [MessageHandler]
        public static void HandleExchangeObjectTransfertListFromInv(ExchangeObjectTransfertListFromInvMessage message, WorldClient client) {
            AbstractTradeExchange dialog = client.Character.GetDialog<AbstractTradeExchange>();
            foreach (uint itemId in message.ids) {
                uint quantity = client.Character.Inventory.GetItem(itemId).Quantity;
                dialog.MoveItem(itemId, (int) quantity);
            }
        }

        [MessageHandler]
        public static void HandleExchangeObjectTransfertAllFromInv(ExchangeObjectTransfertAllFromInvMessage message, WorldClient client) {
            AbstractTradeExchange dialog = client.Character.GetDialog<AbstractTradeExchange>();
            foreach (CharacterItemRecord item in client.Character.Inventory.GetItems()) {
                dialog.MoveItem(item.UId, (int) item.Quantity);
            }
        }

        [MessageHandler]
        public static void HandleExchangeObjectTransfertExistingFromInv(ExchangeObjectTransfertExistingFromInvMessage message, WorldClient client) {
            AbstractTradeExchange dialog = client.Character.GetDialog<AbstractTradeExchange>();
            Models.Entities.Inventory characterInventory = client.Character.Inventory;
            IEnumerable<ItemStack> transferableItems = from item in dialog.GetAllPresentItems()
                                                       let characterItemRecord = characterInventory.GetItem(item.ItemUId)
                                                       where characterItemRecord != null && item.Quantity > 0
                                                       select item;
            foreach (ItemStack item in transferableItems) {
                dialog.MoveItem(item.ItemUId, (int) item.Quantity);
            }
        }

        [MessageHandler]
        public static void HandleExchangeObjectTransfertAllToInv(ExchangeObjectTransfertAllToInvMessage message, WorldClient client) {
            client.Character.GetDialog<AbstractTradeExchange>().RemoveAllItems();
        }

        [MessageHandler]
        public static void HandleExchangeObjectTransfertExistingToInv(ExchangeObjectTransfertExistingToInvMessage message, WorldClient client) {
            AbstractTradeExchange dialog = client.Character.GetDialog<AbstractTradeExchange>();
            IEnumerable<ItemStack> allPresentItems = dialog.GetAllPresentItems();
            List<uint> characterItems = client.Character.Inventory.GetItems().ToList().ConvertAll(x => x.UId);
            foreach (ItemStack itemStack in allPresentItems) {
                if (characterItems.Contains(itemStack.ItemUId)) {
                    dialog.MoveItem(itemStack.ItemUId, -1 * (int) itemStack.Quantity);
                }
            }
        }

        [MessageHandler]
        public static void HandleExchangeObjectTransfertListToInv(ExchangeObjectTransfertListToInvMessage message, WorldClient client) {
            AbstractTradeExchange dialog = client.Character.GetDialog<AbstractTradeExchange>();
            IEnumerable<ItemStack> allPresentItems = dialog.GetAllPresentItems().Where(x => message.ids.Contains(x.ItemUId));
            foreach (ItemStack itemStack in allPresentItems) {
                dialog.MoveItem(itemStack.ItemUId, -1 * (int) itemStack.Quantity);
            }
        }

        #endregion ezfz

        [MessageHandler]
        public static void HandleExchangeReady(ExchangeReadyMessage message, WorldClient client) {
            if (client.Character.GetDialog<Exchange>() != null)
                client.Character.GetDialog<Exchange>().Ready(message.ready, message.step);
        }

        [MessageHandler]
        public static void HandleExchangeObjectMoveKamas(ExchangeObjectMoveKamaMessage message, WorldClient client) {
            if (client.Character.Record.Kamas >= message.quantity && client.Character.GetDialog<Exchange>() != null) {
                client.Character.GetDialog<Exchange>().MoveKamas(message.quantity);
            }
        }

        [MessageHandler]
        public static void HandleExchangePlayerRequest(ExchangePlayerRequestMessage message, WorldClient client) {
            Character target = client.Character.Map.Instance.GetEntity<Character>((long) message.target);

            if (target == null) {
                client.Character.OnExchangeError(ExchangeErrorEnum.REQUEST_IMPOSSIBLE);

                return;
            }

            if (target.Busy) {
                client.Character.OnExchangeError(ExchangeErrorEnum.REQUEST_CHARACTER_OCCUPIED);

                return;
            }

            if (target.Map == null || target.Record.MapId != client.Character.Record.MapId) {
                client.Character.OnExchangeError(ExchangeErrorEnum.REQUEST_IMPOSSIBLE);

                return;
            }

            if (!target.Map.Position.AllowExchangesBetweenPlayers) {
                client.Character.OnExchangeError(ExchangeErrorEnum.REQUEST_IMPOSSIBLE);

                return;
            }

            switch ((ExchangeTypeEnum) message.exchangeType) {
                case ExchangeTypeEnum.PLAYER_TRADE:
                    target.OpenRequestBox(new PlayerTradeRequest(client.Character, target));

                    break;
                default:
                    client.Send(new ExchangeErrorMessage((sbyte) ExchangeErrorEnum.REQUEST_IMPOSSIBLE));

                    break;
            }
        }

        [MessageHandler]
        public static void HandleExchangeAccept(ExchangeAcceptMessage message, WorldClient client) {
            if (client.Character.RequestBox is PlayerTradeRequest)
                client.Character.RequestBox.Accept();
        }

        [MessageHandler]
        public static void HandleExchangeBuy(ExchangeBuyMessage message, WorldClient client) {
            if (client.Character.IsInExchange(ExchangeTypeEnum.NPC_SHOP)) {
                client.Character.GetDialog<NpcShopExchange>().Buy((ushort) message.objectToBuyId, message.quantity);
            }
        }

        [MessageHandler]
        public static void HandleExchangeSell(ExchangeSellMessage message, WorldClient client) {
            if (client.Character.IsInExchange(ExchangeTypeEnum.NPC_SHOP)) {
                client.Character.GetDialog<NpcShopExchange>().Sell(message.objectToSellId, message.quantity);
            }
        }
    }
}