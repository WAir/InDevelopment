using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.ComponentModel;

namespace SettingsManagment
{

    public class SettingsProfile<SettingsType>
    {
        /// <summary>
        /// Nombre del perfil
        /// </summary>
        public string Name;

        /// <summary>
        /// Key del Profile en el que se basa este (puede ser null en caso de no basarse en ninguno)
        /// </summary>
        public int? BasedOnKey;

        /// <summary>
        /// Grupo de settings
        /// </summary>
        public SettingsType Settings;

        /// <summary>
        /// Diccionario sobre si el setting sobreescribe al del BasedOn
        /// </summary>
        /// <remarks>
        /// Ver ejemplo en LogOptimizerSettings
        /// </remarks>
        public Dictionary<MultipleIndexKey, bool /* IsOverriden ? */ > IsOverriden;

        public void Write(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(BasedOnKey.HasValue);

            if (BasedOnKey.HasValue)
                writer.Write(BasedOnKey.Value);

            SettingsHelper.Write(Settings, writer);

            writer.Write(IsOverriden.Keys.Count);

            foreach (var key in IsOverriden.Keys)
            {
                writer.Write(key.Count);    // cantidad de keys

                foreach (var index in key)
                    writer.Write(index);

                writer.Write(IsOverriden[key]); // Valor
            }
        }

        public static SettingsProfile<K> Read<K>(BinaryReader reader, Dictionary<int, SettingsProfile<K>> settingsProfileIndex) where K : new()
        {
            var profile = new SettingsProfile<K>();


            profile.Name = reader.ReadString();

            var basedOn = reader.ReadBoolean();
            if (basedOn) // BasedOnKey.HasValue
                profile.BasedOnKey = reader.ReadInt32();
            else
                profile.BasedOnKey = null;

            profile.Settings = new K();

            if (profile.BasedOnKey != null)
            {
                profile.Settings = settingsProfileIndex[(int)profile.BasedOnKey].Settings;
                SettingsHelper.Read(profile.Settings, reader, true);
            }
            else
                SettingsHelper.Read(profile.Settings, reader, false);
      
            profile.IsOverriden = new Dictionary<MultipleIndexKey, bool>();
            var overridenKeysCount = reader.ReadInt32();

            for (int keyIndex = 0; keyIndex < overridenKeysCount; keyIndex++)
            {
                var keyNumbersCount = reader.ReadInt32();
                var key = new MultipleIndexKey();
                for (int keyNumberIndex = 0; keyNumberIndex <= keyNumbersCount; keyNumberIndex++)
                    key.Add(reader.ReadInt32());

                var isOverriden = reader.ReadBoolean();
                profile.IsOverriden.Add(key, isOverriden);
            }

            return profile;
        }

    }

    /// <summary>
    /// Representa una llave compuesta de varios indices
    /// Por ejemplo el key : 3.2.6
    /// Segun aparece en el numero en la lista
    /// </summary>
    public class MultipleIndexKey : List<int>
    {

        public MultipleIndexKey()
        {
        }

        public MultipleIndexKey(IEnumerable<int> collection) : base(collection)
        {

        }

        public MultipleIndexKey(int firstLevelKey) : base(1)
        {
            Add(firstLevelKey);
        }

        public MultipleIndexKey(int firstLevelKey, int secondLevelKey) : base(2)
        {
            Add(firstLevelKey);
            Add(secondLevelKey);
        }

        public MultipleIndexKey(int firstLevelKey, int secondLevelKey, int thirdLevelKey) : base(2)
        {
            Add(firstLevelKey);
            Add(secondLevelKey);
            Add(thirdLevelKey);
        }

        public MultipleIndexKey MakeSubKey(int newLevelKey)
        {
            var newKey = new MultipleIndexKey(this);
            newKey.Add(newLevelKey);
            return newKey;
        }

        public override string ToString()
        {
            var keyString = string.Empty;
            foreach (var key in this)
            {
                if (keyString != string.Empty)
                    keyString += ".";
                keyString += key;
            }

            return keyString;
        }
    }
}
