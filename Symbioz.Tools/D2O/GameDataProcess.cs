using SSync.IO;
using System.Collections.Generic;


namespace Symbioz.Tools.D2O {
    public class GameDataProcess {
        #region Attributs

        private readonly BigEndianReader m_Reader;
        private List<string> m_QueryableField;
        private Dictionary<string, int> m_SearchFieldIndex;
        private Dictionary<string, int> m_SearchFieldType;
        private Dictionary<string, int> m_SearchFieldCount;

        #endregion

        #region Constructeurs

        public GameDataProcess(BigEndianReader reader) {
            this.m_Reader = reader;

            this.ParseStream();
        }

        #endregion

        #region Méthodes privées

        private void ParseStream() {
            this.m_QueryableField = new List<string>();
            this.m_SearchFieldIndex = new Dictionary<string, int>();
            this.m_SearchFieldType = new Dictionary<string, int>();
            this.m_SearchFieldCount = new Dictionary<string, int>();

            int position = this.m_Reader.ReadInt();
            int seachIndex = this.m_Reader.Position + position + 4;

            while (position > 0) {
                int bytesAvaible = this.m_Reader.BytesAvailable;
                string field = this.m_Reader.ReadUTF();

                this.m_QueryableField.Add(field);
                this.m_SearchFieldIndex.Add(field, this.m_Reader.ReadInt() + seachIndex);
                this.m_SearchFieldType.Add(field, this.m_Reader.ReadInt());
                this.m_SearchFieldCount.Add(field, this.m_Reader.ReadInt());

                position -= bytesAvaible - this.m_Reader.BytesAvailable;
            }
        }

        #endregion
    }
}