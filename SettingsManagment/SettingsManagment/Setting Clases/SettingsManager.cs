using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsManagment
{
    /// <summary>
    /// Administrador de parámetros del Optimizador de Trozos (Log Optimizer)
    /// </summary>
    public class SettingsManager : SettingsManagerBase<LogOptimizerSettings>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsManager()
        {
            FilePath = "B:\\AllSettings.bin";
            ActiveSettings = new LogOptimizerSettings("Default");
            TEMPSettings = new List<LogOptimizerSettings>() {
                                            new LogOptimizerSettings("FeatureA"),
                                            new LogOptimizerSettings("ModelingA"),
                                            new LogOptimizerSettings("Result1"),
                                            new LogOptimizerSettings("Neutral1"),
                                            new LogOptimizerSettings("FeatureB"),
                                            new LogOptimizerSettings("ModelingB"),
                                            new LogOptimizerSettings("Result2"),
                                            new LogOptimizerSettings("Neutral2"),
                                            new LogOptimizerSettings("ModelingC"),
                                            new LogOptimizerSettings("ModelingD"),
            };
        }
    }
}
