using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SettingsManagment
{
    [Version(1)]
    [DeprecatedSettings(
    )]

    public class PartialScanSettings : SettingsBase
    {
        [Key(1)]
        public bool Enabled { get; set; }

        [Key(2)]
        public double LengthTreshold { get; set; }

        public PartialScanSettings()
        {
        }

        public PartialScanSettings(bool ban)
        {
            //Enabled = true;
            LengthTreshold = 666;
        }
    }
}
