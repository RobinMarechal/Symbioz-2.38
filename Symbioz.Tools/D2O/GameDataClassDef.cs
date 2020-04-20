﻿using SSync.IO;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Symbioz.Tools.D2O {
    public class GameDataClassDefinition {
        #region Attributs

        private readonly GameDataFileAccessor m_GameDataFileAccessor;
        private readonly IDataCenter m_Class;
        private readonly List<GameDataField> m_Fields;

        #endregion

        #region Propriéts

        public List<GameDataField> Fields {
            get { return this.m_Fields; }
        }

        #endregion

        #region Constructeurs

        public GameDataClassDefinition(GameDataFileAccessor gameDataFileAccessor, string className, string namespaceName) {
            this.m_GameDataFileAccessor = gameDataFileAccessor;
            this.m_Class = DataCenterTypeManager.GetInstance<IDataCenter>(className);
            this.m_Fields = new List<GameDataField>();
        }

        #endregion

        #region Méthodes publiques

        public IDataCenter Read(string className, BigEndianReader reader) {
            Type type = this.m_Class.GetType();

            if (type.GetInterface("IDataCenter") == null)
                throw new Exception("Unknow type");


            foreach (GameDataField field in this.m_Fields)
                field.Read(className, reader);

            ConstructorInfo[] constructors = type.GetConstructors();
            ParameterInfo[] constructorParameters = constructors[1].GetParameters();
            List<object> parameters = new List<object>();

            foreach (ParameterInfo parameter in constructorParameters) {
                if (parameter.Name == "gameDataFileAccessor") {
                    parameters.Add(this.m_GameDataFileAccessor);

                    continue;
                }

                foreach (GameDataField field in this.m_Fields) {
                    if (parameter.Name.ToLower() == field.Name.ToLower()) {
                        parameters.Add(field.Value);

                        break;
                    }
                }
            }

            object result = constructors[1].Invoke(parameters.ToArray());

            return (IDataCenter) result;
        }

        public void AddField(string fieldName, BigEndianReader reader) {
            GameDataField gameDataField = new GameDataField(fieldName);

            gameDataField.ReadType(reader);

            this.m_Fields.Add(gameDataField);
        }

        public void SetFields(Dictionary<string, Dictionary<int, GameDataClassDefinition>> classes) {
            foreach (GameDataField gameDataField in this.m_Fields)
                gameDataField.SetClasses(classes);
        }

        #endregion
    }
}