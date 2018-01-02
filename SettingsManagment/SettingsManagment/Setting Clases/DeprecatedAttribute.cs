using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsManagment
{/*
	public class DeprecatedSettingsAttribute : Attribute
	{
		public byte[] Keys;

		public DeprecatedSettingsAttribute(byte key, Type type)
		{
			//Keys = keys;
		}
	}


	*/

    //public class DeprecatedSettingDescriptor
    //{
    //    /// <summary>
    //    /// Clave indice numerico del setting
    //    /// </summary>
    //    public byte Key;

    //    /// <summary>
    //    /// Tipo del setting
    //    /// </summary>
    //    public Type Type;

    //    /// <summary>
    //    /// Versión del conjunto de settings donde este setting dejó de existir
    //    /// </summary>
    //    public int ObsolteVersion;

    //}

    public class DeprecatedSettingsAttribute : Attribute
    {
        //public List<DeprecatedSettingDescriptor> DeprecatedSettings;
        public List<byte> DeprecatedSettings;
        
        //public DeprecatedSettingsAttribute(
        //    byte key0 = 0, Type type0 = null, int obsolteVersion0 = -1,
        //    byte key1 = 0, Type type1 = null, int obsolteVersion1 = -1,
        //    byte key2 = 0, Type type2 = null, int obsolteVersion2 = -1,
        //    byte key3 = 0, Type type3 = null, int obsolteVersion3 = -1,
        //    byte key4 = 0, Type type4 = null, int obsolteVersion4 = -1,
        //    byte key5 = 0, Type type5 = null, int obsolteVersion5 = -1,
        //    byte key6 = 0, Type type6 = null, int obsolteVersion6 = -1,
        //    byte key7 = 0, Type type7 = null, int obsolteVersion7 = -1,
        //    byte key8 = 0, Type type8 = null, int obsolteVersion8 = -1,
        //    byte key9 = 0, Type type9 = null, int obsolteVersion9 = -1
        //)
        public DeprecatedSettingsAttribute(params byte[] args)
        {
            //DeprecatedSettings = new List<DeprecatedSettingDescriptor>();
            //var keys = new List<byte> { key0, key1, key2, key3, key4, key5, key6, key7, key8, key9 };
            ////var types = new List<Type> { type0, type1, type2, type3, type4, type5, type6, type7, type8, type9 };
            ////var versions = new List<int> { obsolteVersion0, obsolteVersion1, obsolteVersion2, obsolteVersion3, obsolteVersion4, obsolteVersion5, obsolteVersion6, obsolteVersion7, obsolteVersion8, obsolteVersion9 };

            //for (int i = 0; i < keys.Count; i++)
            //{
            //    var key = keys[i];
            //    //var type = types[i];
            //    var version = versions[i];

            //    //var IsUsed = type != null && type != null;
            //    // verificar que los deprecated esten correctamente usados , si definen el key deben definir los otros dos campos
            //    // caso especial es el 0.

            //    //if (IsUsed)
            //    if (version != -1)
            //        DeprecatedSettings.Add(new DeprecatedSettingDescriptor() { Key = key });/* Type = type, ObsolteVersion = version });

            DeprecatedSettings = new List<byte>();

            foreach (var arg in args)
                DeprecatedSettings.Add(arg);
        }
    }
}
