using SSync.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Symbioz.Tools.D2O {
    public class GameDataFileAccessor {
        #region Attributs

        private Dictionary<string, BigEndianReader> m_Streams;
        private Dictionary<string, Dictionary<int, int>> m_Indexes;
        private Dictionary<string, Dictionary<int, GameDataClassDefinition>> m_Classes;
        private Dictionary<string, int> m_Counter;
        private Dictionary<string, int> m_StreamStartIndex;
        private Dictionary<string, GameDataProcess> m_GameDataProcessor;

        #endregion

        #region Constructeurs

        public GameDataFileAccessor() { }

        #endregion

        #region Méthodes publiques

        public void Init(string filePath) {
            FileInfo fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
                throw new Exception("Game data file \'" + fileInfo.Name + "\' not readable.");

            if (this.m_Streams == null) this.m_Streams = new Dictionary<string, BigEndianReader>();

            if (this.m_Indexes == null) this.m_Indexes = new Dictionary<string, Dictionary<int, int>>();

            if (this.m_Classes == null) this.m_Classes = new Dictionary<string, Dictionary<int, GameDataClassDefinition>>();

            if (this.m_Counter == null) this.m_Counter = new Dictionary<string, int>();

            if (this.m_StreamStartIndex == null) this.m_StreamStartIndex = new Dictionary<string, int>();

            if (this.m_GameDataProcessor == null) this.m_GameDataProcessor = new Dictionary<string, GameDataProcess>();

            string fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);

            BigEndianReader reader;

            if (!this.m_Streams.ContainsKey(fileName)) {
                byte[] fileContent = new byte[fileInfo.Length];
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileStream.Read(fileContent, 0, fileContent.Length);
                fileStream.Dispose();

                reader = new BigEndianReader(fileContent);

                this.m_Streams.Add(fileName, reader);
                this.m_StreamStartIndex.Add(fileName, 7);
            }
            else {
                reader = this.m_Streams[fileName];
                reader.Seek(0);
            }

            int startIndex = 0;

            if (Encoding.UTF8.GetString(reader.ReadBytes(3)) != "D2O") {
                reader.Seek(0);

                if (reader.ReadUTF() != "AKSF")
                    throw new Exception("Malformated game data file.");

                reader.ReadShort();
                int position = reader.ReadInt();

                reader.Seek(position + reader.Position);

                startIndex = reader.Position + 7;

                this.m_StreamStartIndex[fileName] = startIndex;

                if (Encoding.UTF8.GetString(reader.ReadBytes(3)) != "D2O")
                    throw new Exception("Malformated game data file.");
            }

            int startPosition = reader.ReadInt();

            reader.Seek(startIndex + startPosition);

            int indexCount = reader.ReadInt();
            int count = 0;
            Dictionary<int, int> indexes = new Dictionary<int, int>();

            for (int index = 0; index < indexCount; index += 8) {
                indexes.Add(reader.ReadInt(), startIndex + reader.ReadInt());
                count++;
            }

            this.m_Indexes.Add(fileName, indexes);

            this.m_Counter.Add(fileName, count);

            int classCount = reader.ReadInt();
            Dictionary<int, GameDataClassDefinition> classes = new Dictionary<int, GameDataClassDefinition>();

            for (int index = 0; index < classCount; index++) this.ReadClassDefinition(reader.ReadInt(), reader, classes);

            this.m_Classes.Add(fileName, classes);

            if (reader.BytesAvailable != 0) this.m_GameDataProcessor.Add(fileName, new GameDataProcess(reader));

            foreach (KeyValuePair<string, Dictionary<int, GameDataClassDefinition>> classInfo in this.m_Classes) {
                Dictionary<int, GameDataClassDefinition> classDefinitions = classInfo.Value;

                foreach (KeyValuePair<int, GameDataClassDefinition> classDefinition in classDefinitions)
                    classDefinition.Value.SetFields(this.m_Classes);
            }
        }

        public GameDataProcess GetGameDataProcess(string fileName) {
            return this.m_GameDataProcessor[fileName];
        }

        public GameDataClassDefinition GetGameDataClassDefinition(string fileName, int key) {
            Dictionary<int, GameDataClassDefinition> classes = this.m_Classes[fileName];

            return classes[key];
        }

        public int GetCount(string fileName) {
            return this.m_Counter[fileName];
        }

        public object GetObject(string fileName, int key) {
            if ((this.m_Indexes == null) || (!this.m_Indexes.ContainsKey(fileName)))
                return null;

            Dictionary<int, int> indexes = this.m_Indexes[fileName];

            if (!indexes.ContainsKey(key))
                return null;

            this.m_Streams[fileName].Seek(indexes[key]);

            Dictionary<int, GameDataClassDefinition> classes = this.m_Classes[fileName];

            return classes[this.m_Streams[fileName].ReadInt()].Read(fileName, this.m_Streams[fileName]);
        }

        public object[] GetObjects(string fileName) {
            if ((this.m_Counter == null) || (!this.m_Counter.ContainsKey(fileName)))
                return null;

            Dictionary<int, GameDataClassDefinition> classes = this.m_Classes[fileName];
            BigEndianReader reader = this.m_Streams[fileName];
            reader.Seek(this.m_StreamStartIndex[fileName]);

            object[] result = new object[this.m_Counter[fileName]];

            for (int index = 0; index < result.Length; index++)
                result[index] = classes[reader.ReadInt()].Read(fileName, reader);

            return result;
        }

        public void Close() {
            foreach (KeyValuePair<string, BigEndianReader> keyPair in this.m_Streams)
                keyPair.Value.Dispose();

            this.m_Streams = null;
            this.m_Indexes = null;
            this.m_Classes = null;
        }

        #endregion

        #region Méthodes privées

        private void ReadClassDefinition(int key, BigEndianReader reader, Dictionary<int, GameDataClassDefinition> classes) {
            GameDataClassDefinition gameDataClassDefinition = new GameDataClassDefinition(this, reader.ReadUTF(), reader.ReadUTF());

            int fieldCount = reader.ReadInt();

            for (int index = 0; index < fieldCount; index++)
                gameDataClassDefinition.AddField(reader.ReadUTF(), reader);

            classes.Add(key, gameDataClassDefinition);
        }

        #endregion
    }
}