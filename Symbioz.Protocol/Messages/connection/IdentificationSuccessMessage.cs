using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IdentificationSuccessMessage : Message {
        public const ushort Id = 22;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool hasRights;
        public bool wasAlreadyConnected;
        public string login;
        public string nickname;
        public int accountId;
        public sbyte communityId;
        public string secretQuestion;
        public double accountCreation;
        public double subscriptionElapsedDuration;
        public double subscriptionEndDate;
        public byte havenbagAvailableRoom;


        public IdentificationSuccessMessage() { }

        public IdentificationSuccessMessage(bool hasRights,
                                            bool wasAlreadyConnected,
                                            string login,
                                            string nickname,
                                            int accountId,
                                            sbyte communityId,
                                            string secretQuestion,
                                            double accountCreation,
                                            double subscriptionElapsedDuration,
                                            double subscriptionEndDate,
                                            byte havenbagAvailableRoom) {
            this.hasRights = hasRights;
            this.wasAlreadyConnected = wasAlreadyConnected;
            this.login = login;
            this.nickname = nickname;
            this.accountId = accountId;
            this.communityId = communityId;
            this.secretQuestion = secretQuestion;
            this.accountCreation = accountCreation;
            this.subscriptionElapsedDuration = subscriptionElapsedDuration;
            this.subscriptionEndDate = subscriptionEndDate;
            this.havenbagAvailableRoom = havenbagAvailableRoom;
        }


        public override void Serialize(ICustomDataOutput writer) {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.hasRights);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.wasAlreadyConnected);
            writer.WriteByte(flag1);
            writer.WriteUTF(this.login);
            writer.WriteUTF(this.nickname);
            writer.WriteInt(this.accountId);
            writer.WriteSByte(this.communityId);
            writer.WriteUTF(this.secretQuestion);
            writer.WriteDouble(this.accountCreation);
            writer.WriteDouble(this.subscriptionElapsedDuration);
            writer.WriteDouble(this.subscriptionEndDate);
            writer.WriteByte(this.havenbagAvailableRoom);
        }

        public override void Deserialize(ICustomDataInput reader) {
            byte flag1 = reader.ReadByte();
            this.hasRights = BooleanByteWrapper.GetFlag(flag1, 0);
            this.wasAlreadyConnected = BooleanByteWrapper.GetFlag(flag1, 1);
            this.login = reader.ReadUTF();
            this.nickname = reader.ReadUTF();
            this.accountId = reader.ReadInt();

            if (this.accountId < 0)
                throw new Exception("Forbidden value on accountId = " + this.accountId + ", it doesn't respect the following condition : accountId < 0");
            this.communityId = reader.ReadSByte();

            if (this.communityId < 0)
                throw new Exception("Forbidden value on communityId = " + this.communityId + ", it doesn't respect the following condition : communityId < 0");
            this.secretQuestion = reader.ReadUTF();
            this.accountCreation = reader.ReadDouble();

            if (this.accountCreation < 0 || this.accountCreation > 9007199254740990)
                throw new Exception("Forbidden value on accountCreation = " + this.accountCreation + ", it doesn't respect the following condition : accountCreation < 0 || accountCreation > 9007199254740990");
            this.subscriptionElapsedDuration = reader.ReadDouble();

            if (this.subscriptionElapsedDuration < 0 || this.subscriptionElapsedDuration > 9007199254740990)
                throw new Exception("Forbidden value on subscriptionElapsedDuration = "
                                    + this.subscriptionElapsedDuration
                                    + ", it doesn't respect the following condition : subscriptionElapsedDuration < 0 || subscriptionElapsedDuration > 9007199254740990");
            this.subscriptionEndDate = reader.ReadDouble();

            if (this.subscriptionEndDate < 0 || this.subscriptionEndDate > 9007199254740990)
                throw new Exception("Forbidden value on subscriptionEndDate = " + this.subscriptionEndDate + ", it doesn't respect the following condition : subscriptionEndDate < 0 || subscriptionEndDate > 9007199254740990");
            this.havenbagAvailableRoom = reader.ReadByte();

            if (this.havenbagAvailableRoom < 0 || this.havenbagAvailableRoom > 255)
                throw new Exception("Forbidden value on havenbagAvailableRoom = " + this.havenbagAvailableRoom + ", it doesn't respect the following condition : havenbagAvailableRoom < 0 || havenbagAvailableRoom > 255");
        }
    }
}