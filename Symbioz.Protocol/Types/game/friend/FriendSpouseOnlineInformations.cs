// Generated on 04/27/2016 01:13:16

using System;
using System.Collections.Generic;
using System.Linq;
using SSync.IO;

namespace Symbioz.Protocol.Types {
    public class FriendSpouseOnlineInformations : FriendSpouseInformations {
        public const short Id = 93;

        public override short TypeId {
            get { return Id; }
        }

        public int mapId;
        public ushort subAreaId;


        public FriendSpouseOnlineInformations() { }

        public FriendSpouseOnlineInformations(int spouseAccountId,
                                              ulong spouseId,
                                              string spouseName,
                                              byte spouseLevel,
                                              sbyte breed,
                                              sbyte sex,
                                              EntityLook spouseEntityLook,
                                              BasicGuildInformations guildInfo,
                                              sbyte alignmentSide,
                                              int mapId,
                                              ushort subAreaId)
            : base(spouseAccountId, spouseId, spouseName, spouseLevel, breed, sex, spouseEntityLook, guildInfo, alignmentSide) {
            this.mapId = mapId;
            this.subAreaId = subAreaId;
        }


        public override void Serialize(ICustomDataOutput writer) {
            base.Serialize(writer);
            writer.WriteInt(this.mapId);
            writer.WriteVarUhShort(this.subAreaId);
        }

        public override void Deserialize(ICustomDataInput reader) {
            base.Deserialize(reader);
            this.mapId = reader.ReadInt();

            if (this.mapId < 0)
                throw new Exception("Forbidden value on mapId = " + this.mapId + ", it doesn't respect the following condition : mapId < 0");
            this.subAreaId = reader.ReadVarUhShort();

            if (this.subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + this.subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
        }
    }
}