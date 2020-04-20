using System.Collections.Generic;
using System.IO;
using SSync.IO;

namespace Symbioz.Tools.ELE {
    public class Elements {
        public byte Version { get; set; }

        public Dictionary<int, EleGraphicalData> GraphicalData { get; set; }

        public Dictionary<int, bool> GfxJpgMap { get; set; }

        public Elements() {
            this.GraphicalData = new Dictionary<int, EleGraphicalData>();
            this.GfxJpgMap = new Dictionary<int, bool>();
            this.Indexes = new Dictionary<int, int>();
        }

        private Dictionary<int, int> Indexes;

        private BigEndianReader Reader;

        public static Elements ReadFromStream(BigEndianReader reader) {
            Elements instance = new Elements();
            instance.Reader = reader;
            reader.ReadByte(); // header
            instance.Version = reader.ReadByte();
            uint count = reader.ReadUInt();
            int edId;
            ushort skypLen = 0;
            for (int i = 0; i < count; i++) {
                if (instance.Version >= 9) {
                    skypLen = reader.ReadUShort();
                }

                edId = reader.ReadInt();

                if (instance.Version <= 8) {
                    instance.Indexes[edId] = reader.Position;
                    instance.ReadElement(edId);
                }
                else {
                    instance.Indexes[edId] = reader.Position;
                    reader.Seek((skypLen - 4), SeekOrigin.Current);
                }
            }

            if (instance.Version >= 8) {
                int gfxCount = reader.ReadInt();
                for (int i = 0; i < gfxCount; i++) {
                    instance.GfxJpgMap.Add(reader.ReadInt(), true);
                }
            }

            return instance;
        }

        public EleGraphicalData ReadElement(int elementId) {
            this.Reader.Seek(this.Indexes[elementId]);
            //  var loc2 = this.Reader.ReadByte();
            var loc3 = EleGraphicalData.readElement(this, this.Reader, elementId);

            return loc3;
        }
    }
}