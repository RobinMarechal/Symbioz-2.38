﻿// Copyright 2009 - 2010 Sina Iravanian - <sina@sinairv.com>
//
// This source file(s) may be redistributed, altered and customized
// by any means PROVIDING the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------

using System;
using System.Xml.Linq;

namespace YAXLib {
    /// <summary>
    /// a wrapper class around user-defined types, for quick acess to their YAXLib related attributes
    /// </summary>
    internal class UdtWrapper {
        /// <summary>
        /// the underlying type for this instance of <c>TypeWrapper</c>
        /// </summary>
        private readonly Type m_udtType = typeof(object);

        /// <summary>
        /// boolean value indicating whether this instance is a wrapper around a collection type
        /// </summary>
        private readonly bool m_isTypeCollection;

        /// <summary>
        /// boolean value indicating whether this instance is a wrapper around a dictionary type
        /// </summary>
        private readonly bool m_isTypeDictionary;

        /// <summary>
        /// The collection attribute instance
        /// </summary>
        private YAXCollectionAttribute m_collectionAttributeInstance = null;

        /// <summary>
        /// the dictionary attribute instance
        /// </summary>
        private YAXDictionaryAttribute m_dictionaryAttributeInstance = null;

        /// <summary>
        /// reference to an instance of <c>EnumWrapper</c> in case that the current instance is an enum.
        /// </summary>
        private EnumWrapper m_enumWrapper;

        /// <summary>
        /// value indicating whether the serialization options has been explicitly adjusted
        /// using attributes for the class
        /// </summary>
        private bool m_isSerializationOptionSetByAttribute;

        /// <summary>
        /// Alias for the type
        /// </summary>
        private XName m_alias = null;

        /// <summary>
        /// the namespace associated with this element
        /// </summary>
        private XNamespace m_namespace = XNamespace.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdtWrapper"/> class.
        /// </summary>
        /// <param name="udtType">The underlying type to create the wrapper around.</param>
        /// <param name="callerSerializer">reference to the serializer 
        /// instance which is building this instance.</param>
        public UdtWrapper(Type udtType, YAXSerializer callerSerializer) {
            this.m_isTypeDictionary = false;
            this.m_udtType = udtType;
            this.m_isTypeCollection = ReflectionUtils.IsCollectionType(this.m_udtType);
            this.m_isTypeDictionary = ReflectionUtils.IsIDictionary(this.m_udtType);

            this.Alias = StringUtils.RefineSingleElement(ReflectionUtils.GetTypeFriendlyName(this.m_udtType));
            this.Comment = null;
            this.FieldsToSerialize = YAXSerializationFields.PublicPropertiesOnly;
            this.IsAttributedAsNotCollection = false;

            this.SetYAXSerializerOptions(callerSerializer);

            foreach (var attr in this.m_udtType.GetCustomAttributes(true)) {
                if (attr is YAXBaseAttribute) this.ProcessYAXAttribute(attr);
            }
        }

        /// <summary>
        /// Gets the alias of the type.
        /// </summary>
        /// <value>The alias of the type.</value>
        public XName Alias {
            get { return this.m_alias; }

            private set {
                if (this.Namespace.IsEmpty())
                    this.m_alias = this.Namespace + value.LocalName;
                else {
                    this.m_alias = value;
                    if (this.m_alias.Namespace.IsEmpty()) this.m_namespace = this.m_alias.Namespace;
                }
            }
        }

        /// <summary>
        /// Gets an array of comments for the underlying type.
        /// </summary>
        /// <value>The array of comments for the underlying type.</value>
        public string[] Comment { get; private set; }

        /// <summary>
        /// Gets the fields to be serialized.
        /// </summary>
        /// <value>The fields to be serialized.</value>
        public YAXSerializationFields FieldsToSerialize { get; private set; }

        /// <summary>
        /// Gets the serialization options.
        /// </summary>
        /// <value>The serialization options.</value>
        public YAXSerializationOptions SerializationOption { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is attributed as not collection.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is attributed as not collection; otherwise, <c>false</c>.
        /// </value>
        public bool IsAttributedAsNotCollection { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has comment.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has comment; otherwise, <c>false</c>.
        /// </value>
        public bool HasComment {
            get { return this.Comment != null && this.Comment.Length > 0; }
        }

        /// <summary>
        /// Gets the underlying type corresponding to this wrapper.
        /// </summary>
        /// <value>The underlying type corresponding to this wrapper.</value>
        public Type UnderlyingType {
            get { return this.m_udtType; }
        }

        /// <summary>
        /// Gets a value indicating whether the underlying type is a known-type
        /// </summary>
        public bool IsKnownType {
            get { return KnownTypes.IsKnowType(this.m_udtType); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance wraps around an enum.
        /// </summary>
        /// <value><c>true</c> if this instance wraps around an enum; otherwise, <c>false</c>.</value>
        public bool IsEnum {
            get { return this.m_udtType.IsEnum; }
        }

        /// <summary>
        /// Gets the enum wrapper, provided that this instance wraps around an enum.
        /// </summary>
        /// <value>The enum wrapper, provided that this instance wraps around an enum.</value>
        public EnumWrapper EnumWrapper {
            get {
                if (this.IsEnum) {
                    if (this.m_enumWrapper == null) this.m_enumWrapper = new EnumWrapper(this.m_udtType);

                    return this.m_enumWrapper;
                }

                return null;
            }
        }

        /// <summary>
        /// Determines whether serialization of null objects is not allowd.
        /// </summary>
        /// <returns>
        /// <c>true</c> if serialization of null objects is not allowd; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNotAllowdNullObjectSerialization {
            get { return (this.SerializationOption & YAXSerializationOptions.DontSerializeNullObjects) == YAXSerializationOptions.DontSerializeNullObjects; }
        }

        /// <summary>
        /// Determines whether cycling referrences must be ignored, or an exception needs to be thrown
        /// </summary>
        public bool ThrowUponSerializingCyclingReferences {
            get { return (this.SerializationOption & YAXSerializationOptions.ThrowUponSerializingCyclingReferences) == YAXSerializationOptions.ThrowUponSerializingCyclingReferences; }
        }

        /// <summary>
        /// Determines whether properties with no setters should be serialized
        /// </summary>
        public bool DontSerializePropertiesWithNoSetter {
            get { return (this.SerializationOption & YAXSerializationOptions.DontSerializePropertiesWithNoSetter) == YAXSerializationOptions.DontSerializePropertiesWithNoSetter; }
        }


        /// <summary>
        /// Gets a value indicating whether this instance wraps around a collection type.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance wraps around a collection type; otherwise, <c>false</c>.
        /// </value>
        public bool IsCollectionType {
            get { return this.m_isTypeCollection; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance wraps around a dictionary type.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance wraps around a dictionary type; otherwise, <c>false</c>.
        /// </value>
        public bool IsDictionaryType {
            get { return this.m_isTypeDictionary; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is treated as collection.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is treated as collection; otherwise, <c>false</c>.
        /// </value>
        public bool IsTreatedAsCollection {
            get { return (!this.IsAttributedAsNotCollection && this.IsCollectionType); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is treated as dictionary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is treated as dictionary; otherwise, <c>false</c>.
        /// </value>
        public bool IsTreatedAsDictionary {
            get { return !this.IsAttributedAsNotCollection && this.IsDictionaryType; }
        }

        /// <summary>
        /// Gets the collection attribute instance.
        /// </summary>
        /// <value>The collection attribute instance.</value>
        public YAXCollectionAttribute CollectionAttributeInstance {
            get { return this.m_collectionAttributeInstance; }
        }

        /// <summary>
        /// Gets the dictionary attribute instance.
        /// </summary>
        /// <value>The dictionary attribute instance.</value>
        public YAXDictionaryAttribute DictionaryAttributeInstance {
            get { return this.m_dictionaryAttributeInstance; }
        }

        /// <summary>
        /// Gets or sets the type of the custom serializer.
        /// </summary>
        /// <value>The type of the custom serializer.</value>
        public Type CustomSerializerType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has custom serializer.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has custom serializer; otherwise, <c>false</c>.
        /// </value>
        public bool HasCustomSerializer {
            get { return this.CustomSerializerType != null; }
        }

        public bool PreservesWhitespace { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has a custom namespace
        /// defined for it through the <see cref="YAXNamespaceAttribute"/> attribute.
        /// </summary>
        public bool HasNamespace {
            get { return this.Namespace.IsEmpty(); }
        }

        /// <summary>
        /// Gets the namespace associated with this element.
        /// </summary>
        /// <remarks>
        /// If <see cref="HasNamespace"/> is <c>false</c> then this should
        /// be inherited from any parent elements.
        /// </remarks>
        public XNamespace Namespace {
            get { return this.m_namespace; }

            private set {
                this.m_namespace = value;
                // explicit namespace definition overrides namespace definitions in SerializeAs attributes.
                this.m_alias = this.m_namespace + this.m_alias.LocalName;
            }
        }

        /// <summary>
        /// Gets the namespace prefix associated with this element
        /// </summary>
        /// <remarks>
        /// If <see cref="HasNamespace"/> is <c>false</c> then this should
        /// be inherited from any parent elements.
        /// If this is <c>null</c>, then it should be assumed that the specified
        /// <see cref="Namespace"/> (if it is present) is the default namespace.
        /// 
        /// It should also be noted that if a namespace is not provided for the
        /// entire document (default namespace) and yet a default namespace is
        /// provided for one element that an exception should be thrown (since
        /// setting a default namespace for that element would make it apply to
        /// the whole document).
        /// </remarks>
        public string NamespacePrefix { get; private set; }


        /// <summary>
        /// Sets the serializer options.
        /// </summary>
        /// <param name="caller">The caller serializer.</param>
        public void SetYAXSerializerOptions(YAXSerializer caller) {
            if (!this.m_isSerializationOptionSetByAttribute) {
                this.SerializationOption = caller != null ? caller.SerializationOption : YAXSerializationOptions.SerializeNullObjects;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString() {
            return this.m_udtType.ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj) {
            if (obj is UdtWrapper) {
                var other = obj as UdtWrapper;

                return this.m_udtType == other.m_udtType;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode() {
            return this.m_udtType.GetHashCode();
        }

        /// <summary>
        /// Processes the specified attribute.
        /// </summary>
        /// <param name="attr">The attribute to process.</param>
        private void ProcessYAXAttribute(object attr) {
            if (attr is YAXCommentAttribute) {
                string comment = (attr as YAXCommentAttribute).Comment;
                if (!String.IsNullOrEmpty(comment)) {
                    string[] comments = comment.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < comments.Length; i++) {
                        comments[i] = String.Format(" {0} ", comments[i].Trim());
                    }

                    this.Comment = comments;
                }
            }
            else if (attr is YAXSerializableTypeAttribute) {
                var theAttr = attr as YAXSerializableTypeAttribute;
                this.FieldsToSerialize = theAttr.FieldsToSerialize;
                if (theAttr.IsSerializationOptionSet()) {
                    this.SerializationOption = theAttr.Options;
                    this.m_isSerializationOptionSetByAttribute = true;
                }
            }
            else if (attr is YAXSerializeAsAttribute) {
                this.Alias = StringUtils.RefineSingleElement((attr as YAXSerializeAsAttribute).SerializeAs);
            }
            else if (attr is YAXNotCollectionAttribute) {
                if (!ReflectionUtils.IsArray(this.m_udtType)) this.IsAttributedAsNotCollection = true;
            }
            else if (attr is YAXCustomSerializerAttribute) {
                Type serType = (attr as YAXCustomSerializerAttribute).CustomSerializerType;

                Type genTypeArg;
                bool isDesiredInterface = ReflectionUtils.IsDerivedFromGenericInterfaceType(serType, typeof(ICustomSerializer<>), out genTypeArg);

                if (!isDesiredInterface) {
                    throw new YAXException("The provided custom serialization type is not derived from the proper interface");
                }

                if (genTypeArg != this.UnderlyingType) {
                    throw new YAXException("The generic argument of the class and the type of the class do not match");
                }

                this.CustomSerializerType = serType;
            }
            else if (attr is YAXPreserveWhitespaceAttribute) {
                this.PreservesWhitespace = true;
            }
            else if (attr is YAXNamespaceAttribute) {
                var nsAttrib = (attr as YAXNamespaceAttribute);
                this.Namespace = nsAttrib.Namespace;
                this.NamespacePrefix = nsAttrib.Prefix;
            }
            else if (attr is YAXCollectionAttribute) {
                this.m_collectionAttributeInstance = attr as YAXCollectionAttribute;
            }
            else if (attr is YAXDictionaryAttribute) {
                this.m_dictionaryAttributeInstance = attr as YAXDictionaryAttribute;
            }
            else {
                throw new Exception("Attribute not applicable to types!");
            }
        }
    }
}