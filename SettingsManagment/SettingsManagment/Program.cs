using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsManagment
{
    class Program
    {
        static SettingsManager SettingsManager;

        static void Main(string[] args)
        {
            SettingsManager = new SettingsManager();

            SettingsManager.LoadOrCreate();


            //SettingsManager.LoadConfigurationSet("FeatureA", "ModelingZ", "Result1", "Neutral1");

            SettingsManager.SaveToFile();

            DisplayProfiles(0, false);

            //SettingsManager.SettingsProfileIndex[2].Settings.Modeling = "Cambiado en A";
            //DisplayProfiles(0, false);

            //SettingsManager.SettingsProfileIndex[6].Settings.Modeling = "Cambiado en B";
            //DisplayProfiles(0, false);

            SettingsManager.CreateConfigurationSet("Realtime", null, "FeatureA", "ModelingA", "Result1", "Neutral1");



            DisplayProfiles(0, false);

            Console.WriteLine("\nFIN");
        }

        static void DisplayProfiles(int scenarioIndex, bool overridenOnly)
        {

            Console.WriteLine("Cantidad de perfiles: {0}", SettingsManager.SettingsProfileIndex.Keys.Count);
            Console.WriteLine();

            #region PERFIL ACTIVO
            Console.WriteLine("PERFIL ACTIVO \n\nKey: {0}, Nombre: {1} ", SettingsManager.ActiveProfileIndex, SettingsManager.SettingsProfileIndex[SettingsManager.ActiveProfileIndex].Name);

            Console.WriteLine("PROPIEDADES");
            var activeSettings = SettingsManager.ActiveSettings.GetType().GetProperties();
            foreach (var property in activeSettings)
            {
                if (property.PropertyType.IsPrimitive || property.PropertyType.IsEnum || property.PropertyType == typeof(string))
                    Console.WriteLine("{0} = {1}", property.Name, property.GetValue(SettingsManager.ActiveSettings));
                else
                    Console.WriteLine("{0} = ??", property.Name);
            }

            Console.WriteLine("PROPIEDADES SOBREESCRITAS ({0})", SettingsManager.SettingsProfileIndex[SettingsManager.ActiveProfileIndex].IsOverriden.Keys.Count);
            var overridenProperties = SettingsManager.SettingsProfileIndex[SettingsManager.ActiveProfileIndex].IsOverriden;

            foreach (var overriden in overridenProperties)
                Console.WriteLine("Key: {0} Overriden?: {1}", overriden.ToString(), overriden.Value);

            #endregion

            #region PERFILES

            Console.WriteLine("\nPERFILES\n");
            foreach (var profile in SettingsManager.SettingsProfileIndex)
            {
                Console.WriteLine("Key: {0}, Nombre: {1} ", profile.Key, profile.Value.Name);

                var basedOnName = profile.Value.BasedOnKey != null ? SettingsManager.SettingsProfileIndex[(int)profile.Value.BasedOnKey].Name : "null";
                var basedOnKey = profile.Value.BasedOnKey != null ? ((int)profile.Value.BasedOnKey).ToString() : "#";
                Console.WriteLine("Based On {0} : {1}", basedOnKey, basedOnName);

                Console.WriteLine("PROPIEDADES");

                var profileSettings = profile.Value.Settings.GetType().GetProperties();

                foreach (var property in profileSettings)
                {
                    var value = property.GetValue(profile.Value.Settings);

                    if (property.PropertyType.IsPrimitive || property.PropertyType.IsEnum || property.PropertyType == typeof(string))
                        Console.WriteLine("{0} = {1}", property.Name, value);
                    else
                        Console.WriteLine("{0} = ??", property.Name);
                }

                Console.WriteLine("PROPIEDADES SOBREESCRITAS ({0})", SettingsManager.SettingsProfileIndex[SettingsManager.ActiveProfileIndex].IsOverriden.Keys.Count);
                var overridenPropertiesPerProfile = SettingsManager.SettingsProfileIndex[SettingsManager.ActiveProfileIndex].IsOverriden;

                foreach (var overriden in overridenPropertiesPerProfile)
                    Console.WriteLine("Key: {0} Overriden?: {1}", overriden.ToString(), overriden.Value);

                Console.WriteLine();
            }

            #endregion
            Console.ReadKey();
        }

        static void DisplayByTags()
        {

        }
    }

    public class InternalScenarioSettings
    {
    }

    public class Scenario
    {
        public string Name;
        public string BasedOn;
    }
}
