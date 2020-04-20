using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class JobCrafterDirectoryEntryMessage : Message {
        public const ushort Id = 6044;

        public override ushort MessageId {
            get { return Id; }
        }

        public JobCrafterDirectoryEntryPlayerInfo playerInfo;
        public JobCrafterDirectoryEntryJobInfo[] jobInfoList;
        public EntityLook playerLook;


        public JobCrafterDirectoryEntryMessage() { }

        public JobCrafterDirectoryEntryMessage(JobCrafterDirectoryEntryPlayerInfo playerInfo, JobCrafterDirectoryEntryJobInfo[] jobInfoList, EntityLook playerLook) {
            this.playerInfo = playerInfo;
            this.jobInfoList = jobInfoList;
            this.playerLook = playerLook;
        }


        public override void Serialize(ICustomDataOutput writer) {
            this.playerInfo.Serialize(writer);
            writer.WriteUShort((ushort) this.jobInfoList.Length);
            foreach (var entry in this.jobInfoList) {
                entry.Serialize(writer);
            }

            this.playerLook.Serialize(writer);
        }

        public override void Deserialize(ICustomDataInput reader) {
            this.playerInfo = new JobCrafterDirectoryEntryPlayerInfo();
            this.playerInfo.Deserialize(reader);
            var limit = reader.ReadUShort();
            this.jobInfoList = new JobCrafterDirectoryEntryJobInfo[limit];
            for (int i = 0; i < limit; i++) {
                this.jobInfoList[i] = new JobCrafterDirectoryEntryJobInfo();
                this.jobInfoList[i].Deserialize(reader);
            }

            this.playerLook = new EntityLook();
            this.playerLook.Deserialize(reader);
        }
    }
}