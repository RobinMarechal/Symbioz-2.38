using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Protocol.Types;
using SSync.IO;
using SSync.Messages;

namespace Symbioz.Protocol.Messages {
    public class IdentificationMessage : Message {
        public const ushort Id = 4;

        public override ushort MessageId {
            get { return Id; }
        }

        public bool autoconnect;
        public bool useCertificate;
        public bool useLoginToken;
        public VersionExtended version;
        public string lang;
        public sbyte[] credentials;
        public short serverId;
        public long sessionOptionalSalt;
        public ushort[] failedAttempts;


        public IdentificationMessage() { }

        public IdentificationMessage(bool autoconnect,
                                     bool useCertificate,
                                     bool useLoginToken,
                                     VersionExtended version,
                                     string lang,
                                     sbyte[] credentials,
                                     short serverId,
                                     long sessionOptionalSalt,
                                     ushort[] failedAttempts) {
            this.autoconnect = autoconnect;
            this.useCertificate = useCertificate;
            this.useLoginToken = useLoginToken;
            this.version = version;
            this.lang = lang;
            this.credentials = credentials;
            this.serverId = serverId;
            this.sessionOptionalSalt = sessionOptionalSalt;
            this.failedAttempts = failedAttempts;
        }


        public override void Serialize(ICustomDataOutput writer) {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, this.autoconnect);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, this.useCertificate);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, this.useLoginToken);
            writer.WriteByte(flag1);
            this.version.Serialize(writer);
            writer.WriteUTF(this.lang);
            writer.WriteVarUhShort((ushort) this.credentials.Length);
            foreach (var entry in this.credentials) {
                writer.WriteSByte(entry);
            }

            writer.WriteShort(this.serverId);
            writer.WriteVarLong(this.sessionOptionalSalt);
            writer.WriteUShort((ushort) this.failedAttempts.Length);
            foreach (var entry in this.failedAttempts) {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader) {
            byte flag1 = reader.ReadByte();
            this.autoconnect = BooleanByteWrapper.GetFlag(flag1, 0);
            this.useCertificate = BooleanByteWrapper.GetFlag(flag1, 1);
            this.useLoginToken = BooleanByteWrapper.GetFlag(flag1, 2);
            this.version = new VersionExtended();
            this.version.Deserialize(reader);
            this.lang = reader.ReadUTF();
            var limit = reader.ReadVarUhShort();
            this.credentials = new sbyte[limit];
            for (int i = 0; i < limit; i++) {
                this.credentials[i] = reader.ReadSByte();
            }

            this.serverId = reader.ReadShort();
            this.sessionOptionalSalt = reader.ReadVarLong();

            if (this.sessionOptionalSalt < -9007199254740990 || this.sessionOptionalSalt > 9007199254740990)
                throw new Exception("Forbidden value on sessionOptionalSalt = "
                                    + this.sessionOptionalSalt
                                    + ", it doesn't respect the following condition : sessionOptionalSalt < -9007199254740990 || sessionOptionalSalt > 9007199254740990");
            limit = reader.ReadUShort();
            this.failedAttempts = new ushort[limit];
            for (int i = 0; i < limit; i++) {
                this.failedAttempts[i] = reader.ReadVarUhShort();
            }
        }
    }
}