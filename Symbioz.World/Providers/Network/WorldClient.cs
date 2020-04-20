using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using SSync;
using SSync.Messages;
using SSync.Transition;
using Symbioz.Core;
using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Selfmade.Messages;
using Symbioz.Protocol.Selfmade.Types;
using Symbioz.Protocol.Types;
using Symbioz.World.Models.Entities;
using Symbioz.World.Records;
using Symbioz.World.Records.Breeds;
using Symbioz.World.Records.Characters;

namespace Symbioz.World.Network {
    public class WorldClient : SSyncClient {
        public AccountData Account { get; set; }

        public AccountInformationsRecord AccountInformations { get; set; }

        public bool InGame {
            get { return this.Character != null; }
        }

        public Character Character { get; set; }

        public List<CharacterRecord> Characters { get; set; }

        public bool HasStartupActions {
            get { return this.StartupActions.Count > 0; }
        }

        public List<StartupActionRecord> StartupActions { get; private set; }

        public WorldClient(Socket socket)
            : base(socket) {
            this.OnMessageHandleFailed += this.WorldClient_OnMessageHandleFailed;
            this.Send(new HelloGameMessage());
            WorldServer.Instance.AddClient(this);
        }

        void WorldClient_OnMessageHandleFailed(Message message) {
            if (this.Character != null && message != null) this.Character.ReplyError("Impossible d'executer l'action (" + message + ").");
        }

        public override void OnClosed() {
            try {
                WorldServer.Instance.RemoveClient(this);
                if (this.Character != null) this.Character.OnDisconnected();
                base.OnClosed();
            }
            catch (Exception ex) {
                Logger.Write<WorldClient>("Cannot disconnect client..." + ex, ConsoleColor.Red);
            }
        }

        public void OnAccountReceived() {
            this.AccountInformations = AccountInformationsRecord.Load(this.Account.Id);
            this.Characters = CharacterRecord.GetCharactersByAccountId(this.Account.Id);
            this.StartupActions = StartupActionRecord.GetStartupActions(this.Account.Id);
            this.Send(new AuthenticationTicketAcceptedMessage());
            this.Send(new AccountCapabilitiesMessage(true,
                                                     true,
                                                     this.Account.Id,
                                                     BreedRecord.AvailableBreedsFlags,
                                                     BreedRecord.AvailableBreedsFlags,
                                                     1));
            this.Send(new TrustStatusMessage(true, true));
            this.Send(new ServerSettingsMessage("fr", 1, 0, 0));
            this.Send(new ServerOptionalFeaturesMessage(new sbyte[] {1, 2, 3, 4}));
        }

        public CharacterRecord GetAccountCharacter(long id) {
            return Enumerable.FirstOrDefault(this.Characters, x => x.Id == id);
        }

        public void SendRaw(byte[] rawData) {
            this.Send(new RawDataMessage(rawData));
        }

        public void SendCharactersList() {
            CharacterBaseInformations[] characters = this.Characters.ConvertAll(x => x.GetCharacterBaseInformations()).ToArray();


            CharacterToRemodelInformations[] characterToRemodel = this.Characters.FindAll(x => x.RemodelingMaskEnum != CharacterRemodelingEnum.CHARACTER_REMODELING_NOT_APPLICABLE)
                                                                      .ConvertAll(x => x.GetCharacterToRemodelInformations())
                                                                      .ToArray();

            if (Enumerable.Any(characterToRemodel)) {
                this.Send(new CharactersListWithRemodelingMessage(characters, this.HasStartupActions, characterToRemodel));
            }
            else {
                this.Send(new CharactersListMessage(characters, this.HasStartupActions));
            }
        }


        public bool Ban() {
            bool result = false;

            if (this.Account != null) {
                MessagePool.SendRequest<BanConfirmMessage>(TransitionServerManager.Instance.AuthServer,
                                                           new BanRequestMessage {
                                                               AccountId = this.Account.Id
                                                           },
                                                           delegate {
                                                               result = true;
                                                               this.Disconnect();
                                                           },
                                                           delegate { });
            }

            return result;
        }
    }
}