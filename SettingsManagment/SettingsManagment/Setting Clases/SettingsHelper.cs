using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Collections;
using SettingsManagment.Interfaces;

namespace SettingsManagment
{
    public class SettingsHelper
    {
        public static List<SettingProperty> GetProperties(object settingsObject)
        {
            try
            {
                var settingProperties = new List<SettingProperty>();
                var properties = settingsObject.GetType().GetProperties();
                var keyType = typeof(KeyAttribute);

                foreach (var property in properties)
                {
                    var settingProperty = new SettingProperty() { Property = property };
                    var attributes = property.GetCustomAttributes();

                    foreach (var attribute in attributes)
                    {
                        var attributeType = attribute.GetType();

                        if (attributeType == keyType)
                        {
                            settingProperty.Key = ((KeyAttribute)attribute).Key;
                        }
                    }

                    settingProperties.Add(settingProperty);
                }
                return settingProperties;
            }
            catch (Exception e)
            {
                //Logger.Error(typeof(SettingsHelper), e);
                return new List<SettingProperty>();
            }
        }

        public static void Write(object settingsObject, BinaryWriter writer)
        {
            var settingsAttributes = settingsObject.GetType().GetCustomAttributes();
            var versionType = typeof(VersionAttribute);

            foreach (var attribute in settingsAttributes)
            {
                var attributeType = attribute.GetType();
                if (attributeType == versionType)
                    writer.Write(((VersionAttribute)attribute).Version); // version setting
            }

            var settingsProperties = settingsObject.GetType().GetProperties();
            var keyType = typeof(KeyAttribute);

            foreach (var property in settingsProperties)
            {
                var attributes = property.GetCustomAttributes();

                foreach (var attribute in attributes)
                {
                    var attributeType = attribute.GetType();
                    if (attributeType == keyType)
                        writer.Write(((KeyAttribute)attribute).Key);  //Key          
                }

                writer.Write((property.PropertyType.FullName));    // tipo

                bool hasValue = (property.GetValue(settingsObject)) != null;
                writer.Write(hasValue); // has value

                if (hasValue)
                    WriteValueByType(property.PropertyType, writer, property.GetValue(settingsObject)); // valor
            }
            byte lastValue = 0;
            writer.Write(lastValue); // ultimo valor
        }

        /// <summary>
        /// Escribe los valores por su tipo en binario
        /// </summary>
        /// <param name="type">Tipo que se va a escribir</param>
        /// <param name="writer">Writer binario</param>
        /// <param name="settingsObject">Settings con los valores</param>
        /// <param name="property">Propiedad</param>
        public static void WriteValueByType(Type type, BinaryWriter writer, object instance)
        {
            if (type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal))
                WritePrimitive(type, writer, instance);

            else
            {
                if (instance is IList)
                    WriteList(type, writer, instance);

                else if (instance is IDictionary)
                    WriteDictionary(type, writer, instance);

                else if (instance is ISetting)
                    WriteSetting(type, writer, instance);

                else if (type == typeof(Rectangle))
                {
                    writer.Write(((Rectangle)instance).Width);
                    writer.Write(((Rectangle)instance).Height);
                }

                // Para cualquier otra case no definida. Leerá TODOS los fields y propiedades públicos.
                else
                {
                    var fields = type.GetFields();
                    foreach (var field in fields)
                    {
                        var fieldType = field.FieldType;
                        writer.Write(fieldType.FullName);    // type
                        WriteValueByType(fieldType, writer, field.GetValue(instance));
                    }

                    var properties = type.GetProperties();
                    foreach (var property in properties)
                    {
                        var propertyType = property.PropertyType;
                        writer.Write(propertyType.FullName);
                        WriteValueByType(propertyType, writer, property.GetValue(instance));
                    }
                }
            }
        }

        private static void WriteSetting(Type type, BinaryWriter writer, object instance)
        {
            Write(instance, writer);
        }

        private static void WritePrimitive(Type type, BinaryWriter writer, object instance)
        {
            // Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single.
            if (type == typeof(bool))
                writer.Write((bool)instance);

            else if (type.IsEnum || type == typeof(int))
                writer.Write((int)instance);

            else if (type == typeof(byte))
                writer.Write((byte)instance);

            else if (type == typeof(char))
                writer.Write((char)instance);

            else if (type == typeof(double))
                writer.Write((double)instance);

            else if (type == typeof(string))
                writer.Write((string)instance);

            else if (type == typeof(decimal))
                writer.Write((decimal)instance);
        }

        private static void WriteList(Type type, BinaryWriter writer, object instance)
        {
            var instanceList = instance as IList;
            var itemType = instance.GetType().GenericTypeArguments.First();

            writer.Write(itemType.FullName);    // item type
            writer.Write(instanceList.Count); // item count

            foreach (var item in instanceList) // item Values
                WriteValueByType(itemType, writer, item);
        }

        private static void WriteDictionary(Type type, BinaryWriter writer, object instance)
        {
            var instanceDictionary = instance as IDictionary;

            List<object> keys = new List<object>();
            List<object> values = new List<object>();

            var _keys = instanceDictionary.Keys;
            var _values = instanceDictionary.Values;

            var keyValueTypes = new List<object>();

            foreach (var item in _keys)
            {
                keys.Add(item);
                keyValueTypes.Add(item.GetType());
            }

            foreach (var item in _values)
            {
                values.Add(item);
                keyValueTypes.Add(item.GetType());
            }

            var keysType = keyValueTypes.First();
            var valuesType = keyValueTypes.Last();

            writer.Write(((Type)keysType).FullName); //key type
            writer.Write(((Type)valuesType).FullName); // key value

            var count = instanceDictionary.Count;
            writer.Write(count); // count

            for (int i = 0; i < count; i++) // values
            {
                WriteValueByType((Type)keysType, writer, keys[i]);
                WriteValueByType((Type)valuesType, writer, values[i]);
            }
        }

        public static void Read(object settingsObject, BinaryReader reader, bool dumpReading)
        {
            int version = int.MinValue;

            #region Deprecated settings
            var deprecatedSettings = new List<byte>();
            var settingsAttributes = settingsObject.GetType().GetCustomAttributes();
            var deprecatedType = typeof(DeprecatedSettingsAttribute);
            var versionType = typeof(VersionAttribute);

            foreach (var attribute in settingsAttributes)
            {
                var attributeType = attribute.GetType();

                if (attributeType == deprecatedType)
                    deprecatedSettings = ((DeprecatedSettingsAttribute)attribute).DeprecatedSettings;

                else if (attributeType == versionType)
                {
                    if (dumpReading)
                        reader.ReadInt32();
                    else
                        version = reader.ReadInt32();
                }

            }

            var deprecatedKeys = new List<byte>();

            foreach (var item in deprecatedSettings)
                deprecatedKeys.Add(item);

            #endregion

            var properties = settingsObject.GetType().GetProperties();
            var keyType = typeof(KeyAttribute);
            var propertyKeys = new List<byte>(properties.Length);

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes();
                foreach (var attribute in attributes)
                {
                    var attributeType = attribute.GetType();
                    if (attributeType == keyType)
                    {
                        var propertyKey = ((KeyAttribute)attribute).Key;
                        propertyKeys.Add(propertyKey);
                    }
                }
            }

            while (true)
            {
                var peekKey = reader.PeekChar();

                // 0 indicará que no quedan más settings que leer
                if (peekKey == 0)
                {
                    reader.ReadByte();
                    break;
                }

                if (deprecatedKeys.Contains((byte)peekKey))
                {
                    reader.ReadByte(); //key
                    var typeDump = Type.GetType(reader.ReadString()); // type
                    var hasValue = reader.ReadBoolean();

                    if (hasValue)
                        ReadValueByType(typeDump, reader, dumpReading); //value
                }
                else
                {
                    foreach (var property in properties)
                    {
                        var attributes = property.GetCustomAttributes();
                        foreach (var attribute in attributes)
                        {
                            var attributeType = attribute.GetType();
                            if (attributeType == keyType)
                            {
                                var propertyKey = ((KeyAttribute)attribute).Key;
                                if (propertyKey == peekKey)
                                {
                                    reader.ReadByte(); // key
                                    var type = Type.GetType(reader.ReadString()); // type
                                    var hasValue = reader.ReadBoolean();

                                    if (hasValue)
                                    {
                                        if (!dumpReading)
                                            property.SetValue(settingsObject, ReadValueByType(type, reader, dumpReading)); // value
                                        else
                                            ReadValueByType(type, reader, dumpReading);
                                    }

                                    else
                                    {
                                        if (!dumpReading)
                                            property.SetValue(settingsObject, null);
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Lee y retorna datos dependiendo del tipo de estos
        /// </summary>
        /// <param name="type">Tipo del dato a leer</param>
        /// <param name="reader">Binary reader</param>
        /// <returns></returns>
        public static object ReadValueByType(Type type, BinaryReader reader, bool dumpReading)
        {
            if (type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal)) // value
                return ReadPrimitives(type, reader);

            else
            {
                if (type.GetInterface("IList") != null)
                    return ReadList(type, reader, dumpReading);

                else if (type.GetInterface("IDictionary") != null)
                    return ReadDictionary(type, reader, dumpReading);

                else if (type.GetInterface("ISetting") != null)
                    return ReadSetting(type, reader, dumpReading);

                else if (type == typeof(Rectangle))
                {
                    var rectangle = new Rectangle(reader.ReadDouble(), reader.ReadDouble());
                    return rectangle;
                }

                // Para cualquier otra clase. Por defecto leerá todos los fields y properties.
                // Si esto no es lo deseado, crear un else if antes de este else para una custom class.
                else
                {
                    var instance = Activator.CreateInstance(type);

                    var fields = type.GetFields();
                    foreach (var field in fields)
                    {
                        reader.ReadString();
                        var fieldType = field.FieldType;
                        field.SetValue(instance, ReadValueByType(fieldType, reader, dumpReading));
                    }

                    var properties = type.GetProperties();
                    foreach (var property in properties)
                    {
                        reader.ReadString();
                        var propertyType = property.PropertyType;
                        property.SetValue(instance, ReadValueByType(propertyType, reader, dumpReading));
                    }

                    return instance;
                }
            }
        }

        private static object ReadSetting(Type type, BinaryReader reader, bool dumpReading)
        {
            var settings = Activator.CreateInstance(type);
            Read(settings, reader, dumpReading);
            return settings;
        }

        private static object ReadDictionary(Type type, BinaryReader reader, bool dumpReading)
        {
            var keysType = Type.GetType(reader.ReadString()); // key type
            var valuesType = Type.GetType(reader.ReadString()); // key type

            var parameters = new Type[] { keysType, valuesType };
            var dictionary = (IDictionary)Activator.CreateInstance((typeof(Dictionary<,>).MakeGenericType(parameters)));

            var count = reader.ReadInt32(); // count

            for (int i = 0; i < count; i++)
                dictionary.Add(ReadValueByType(keysType, reader, dumpReading), ReadValueByType(valuesType, reader, dumpReading));

            return dictionary;
        }

        private static object ReadList(Type type, BinaryReader reader, bool dumpReading)
        {
            var itemType = Type.GetType(reader.ReadString()); // item type
            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

            var count = reader.ReadInt32(); // count

            for (int i = 0; i < count; i++) // items
                list.Add(ReadValueByType(itemType, reader, dumpReading));

            return list;
        }

        private static object ReadPrimitives(Type type, BinaryReader reader)
        {
            if (type == typeof(bool))
                return reader.ReadBoolean();

            else if (type.IsEnum || type == typeof(int))
                return reader.ReadInt32();

            else if (type == typeof(byte))
                return reader.ReadByte();

            else if (type == typeof(char))
                return reader.ReadChar();

            else if (type == typeof(double))
                return reader.ReadDouble();

            else if (type == typeof(string))
                return reader.ReadString();

            else if (type == typeof(decimal))
                return reader.ReadDecimal();

            return null; // NO debería pasar por aquí
        }
    }
}
