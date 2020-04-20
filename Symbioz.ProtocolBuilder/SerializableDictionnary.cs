using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Symbioz.ProtocolBuilder {
    [Serializable]
    public class SerializableDictionary<TKey, TVal> : Dictionary<TKey, TVal>, IXmlSerializable, ISerializable {
        #region Constants

        private const string ItemNodeName = "Item";
        private const string KeyNodeName = "Key";
        private const string ValueNodeName = "Value";

        #endregion

        #region Constructors

        public SerializableDictionary() { }

        public SerializableDictionary(IDictionary<TKey, TVal> dictionary)
            : base(dictionary) { }

        public SerializableDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer) { }

        public SerializableDictionary(int capacity)
            : base(capacity) { }

        public SerializableDictionary(IDictionary<TKey, TVal> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer) { }

        public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer) { }

        #endregion

        private XmlSerializer keySerializer;
        private XmlSerializer valueSerializer;

        protected SerializableDictionary(SerializationInfo info, StreamingContext context) {
            int itemCount = info.GetInt32("ItemCount");
            for (int i = 0; i < itemCount; i++) {
                var kvp =
                    (KeyValuePair<TKey, TVal>)
                    info.GetValue(String.Format("Item{0}", i), typeof(KeyValuePair<TKey, TVal>));
                this.Add(kvp.Key, kvp.Value);
            }
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("ItemCount", this.Count);
            int itemIdx = 0;
            foreach (var kvp in this) {
                info.AddValue(String.Format("Item{0}", itemIdx), kvp, typeof(KeyValuePair<TKey, TVal>));
                itemIdx++;
            }
        }

        #endregion

        #region IXmlSerializable Members

        void IXmlSerializable.WriteXml(XmlWriter writer) {
            foreach (var kvp in this) {
                writer.WriteStartElement(ItemNodeName);
                writer.WriteStartElement(KeyNodeName);
                this.KeySerializer.Serialize(writer, kvp.Key);
                writer.WriteEndElement();
                writer.WriteStartElement(ValueNodeName);
                this.ValueSerializer.Serialize(writer, kvp.Value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }

        void IXmlSerializable.ReadXml(XmlReader reader) {
            if (reader.IsEmptyElement) {
                return;
            }

            // Move past container
            if (!reader.Read()) {
                throw new XmlException("Error in Deserialization of Dictionary");
            }

            while (reader.NodeType != XmlNodeType.EndElement) {
                reader.ReadStartElement(ItemNodeName);
                reader.ReadStartElement(KeyNodeName);
                var key = (TKey) this.KeySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement(ValueNodeName);
                var value = (TVal) this.ValueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadEndElement();
                this.Add(key, value);
                reader.MoveToContent();
            }

            reader.ReadEndElement(); // Read End Element to close Read of containing node
        }

        XmlSchema IXmlSerializable.GetSchema() {
            return null;
        }

        #endregion

        #region Private Properties

        protected XmlSerializer ValueSerializer {
            get {
                if (this.valueSerializer == null) {
                    this.valueSerializer = new XmlSerializer(typeof(TVal));
                }

                return this.valueSerializer;
            }
        }

        private XmlSerializer KeySerializer {
            get {
                if (this.keySerializer == null) {
                    this.keySerializer = new XmlSerializer(typeof(TKey));
                }

                return this.keySerializer;
            }
        }

        #endregion
    }
}