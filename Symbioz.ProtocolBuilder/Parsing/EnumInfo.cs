using System;
using System.Collections.Generic;

namespace Symbioz.ProtocolBuilder.Parsing {
    public class EnumElement {
        public EnumElement(string key, string value) {
            this.Key = key;
            this.Value = value;
        }

        public string Key;
        public string Value;
    }

    public class EnumInfo {
        public EnumInfo() {
            this.Elements = new List<EnumElement>();
        }

        public AccessModifiers AccessModifier { get; set; }

        public string Namespace { get; set; }

        public string Name { get; set; }

        public List<EnumElement> Elements { get; set; }

        public string CustomAttribute { get; set; }
    }
}