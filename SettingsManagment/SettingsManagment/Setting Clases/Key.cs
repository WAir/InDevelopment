using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsManagment
{
    /// <summary>
    /// Key de un setting (no puede ser 0)
    /// </summary>
    public class KeyAttribute : Attribute
	{
		public byte Key;

		public KeyAttribute(byte key)
		{
			Key = key;
		}
	}
}
