using SSync.IO;
using System;
using System.Collections.Generic;


namespace Symbioz.Tools.D2O {
    public class GameDataField {
        #region Attributs

        private object m_Value;
        private Func<string, BigEndianReader, int, object> m_ReadData;
        private List<Func<string, BigEndianReader, int, object>> m_ListReadMethods;
        private const int m_NullIdentifier = -1431655766;
        private Dictionary<string, Dictionary<int, GameDataClassDefinition>> m_Classes;
        private List<string> m_ListType;

        #endregion

        #region Propriétés

        public string Name { get; set; }

        public object Value {
            get { return this.m_Value; }
            set { this.m_Value = value; }
        }

        public Func<string, BigEndianReader, int, object> ReadData {
            get { return this.m_ReadData; }
            set { this.m_ReadData = value; }
        }

        #endregion

        #region Constructeurs

        public GameDataField(string fieldName) {
            this.Name = fieldName;
        }

        #endregion

        #region Méthodes publiques

        public void ReadType(BigEndianReader reader) {
            this.m_ReadData = this.GetReadMethod(reader.ReadInt(), reader);
        }

        public void Read(string fieldName, BigEndianReader reader, int size = 0) {
            this.m_Value = this.m_ReadData(fieldName, reader, size);
        }

        public void SetClasses(Dictionary<string, Dictionary<int, GameDataClassDefinition>> classes) {
            this.m_Classes = classes;
        }

        #endregion

        #region Méthodes privée

        private Func<string, BigEndianReader, int, object> GetReadMethod(int methodID, BigEndianReader reader) {
            switch (methodID) {
                case -1:
                    return ReadInteger;
                case -2:
                    return ReadBoolean;
                case -3:
                    return ReadString;
                case -4:
                    return ReadNumber;
                case -5:
                    return ReadI18N;
                case -6:
                    return ReadUnsignedInteger;
                case -99:
                    if (this.m_ListReadMethods == null) {
                        this.m_ListReadMethods = new List<Func<string, BigEndianReader, int, object>>();
                        this.m_ListType = new List<string>();
                    }

                    this.m_ListType.Add(reader.ReadUTF());
                    this.m_ListReadMethods.Add(this.GetReadMethod(reader.ReadInt(), reader));

                    return this.ReadList;
                default:
                    if (methodID > 0)
                        return this.ReadObject;

                    throw new Exception("Unknown type \'" + methodID + "\'.");
            }
        }

        private object ReadList(string fieldName, BigEndianReader reader, int dimension = 0) {
            int listCount = reader.ReadInt();

            List<object> result = new List<object>();

            for (int index = 0; index < listCount; index++)
                result.Add(this.m_ListReadMethods[dimension](fieldName, reader, dimension + 1));

            return result;
        }

        private object ReadObject(string fieldName, BigEndianReader reader, int dimension = 0) {
            int typeID = reader.ReadInt();

            if (typeID == m_NullIdentifier)
                return null;

            Dictionary<int, GameDataClassDefinition> className = this.m_Classes[fieldName];

            return className[typeID].Read(fieldName, reader);
        }

        private static object ReadInteger(string fieldName, BigEndianReader reader, int dimension = 0) {
            return reader.ReadInt();
        }

        private static object ReadBoolean(string fieldName, BigEndianReader reader, int dimension = 0) {
            return reader.ReadBoolean();
        }

        private static object ReadString(string fieldName, BigEndianReader reader, int dimension = 0) {
            string result = reader.ReadUTF();

            if (result == "null")
                return null;

            return result;
        }

        private static object ReadNumber(string fieldName, BigEndianReader reader, int dimension = 0) {
            return reader.ReadDouble();
        }

        private static object ReadI18N(string fieldName, BigEndianReader reader, int dimension = 0) {
            return reader.ReadInt();
        }

        private static object ReadUnsignedInteger(string fieldName, BigEndianReader reader, int dimension = 0) {
            return reader.ReadUInt();
        }

        #endregion
    }
}