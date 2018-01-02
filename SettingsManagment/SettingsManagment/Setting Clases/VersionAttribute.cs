using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsManagment
{
	public class VersionAttribute : Attribute
	{
		public int Version;

		public VersionAttribute(int version)
		{
			Version = version;
		}
	}
}
