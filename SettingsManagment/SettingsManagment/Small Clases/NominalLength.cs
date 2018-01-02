using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsManagment
{
	public class NominalLength
	{
		public double Length;
		public double Nominal;

		public NominalLength(double length, double nominal)
		{
			Length = length;
			Nominal = nominal;
		}

        public NominalLength()
        {

        }
	}
}
