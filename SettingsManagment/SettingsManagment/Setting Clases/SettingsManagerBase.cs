using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace SettingsManagment
{
    [Version(1)]
    public class SettingsManagerBase<SettingsType> where SettingsType : new()
    {

        public List<SettingsType> TEMPSettings;

        /// <summary>
        /// Indice de escenarios por un key numerico
        /// El Key siempre crece al agregar un nuevo escenario
        /// Si un escenario se remueve, el key nunca se usa nuevamente
        /// </summary>
        public Dictionary<int /* Key */ , SettingsProfile<SettingsType>> SettingsProfileIndex;

        public SettingsType ActiveSettings; // Grupo de Settings Activo

        //public SettingsProfile<SettingsType> ActiveProfile; // Perfil de Settings Activo

        public int ActiveProfileIndex;

        public string FilePath;

        public void LoadFromFile()
        {

            if (File.Exists(FilePath))
            {
                using (var reader = new BinaryReader(File.Open(FilePath, FileMode.Open)))
                {
                    Read(reader);
                    //SettingsBase.Read(reader);
                }
            }
        }

        public void SaveToFile()
        {
            using (var writer = new BinaryWriter(File.Open(FilePath, FileMode.Create)))
            {
                Write(writer);
            }
        }

        private void Write(BinaryWriter writer)
        {
            var versionAttribute = GetType().GetCustomAttributes(typeof(VersionAttribute), true)[0] as VersionAttribute;

            writer.Write(versionAttribute.Version); // Version del manager
            writer.Write(ActiveProfileIndex);

            writer.Write(SettingsProfileIndex.Keys.Count); // Cantidad de profiles

            foreach (var key in SettingsProfileIndex.Keys)
            {
                writer.Write(key); // Key (dicc) del perfil
                var profile = SettingsProfileIndex[key];
                profile.Write(writer);
            }
        }

        private void Read(BinaryReader reader)
        {
            var managerVersion = reader.ReadInt32(); // Version del manager (aqui probablemente requiere versionado para el manager)
            ActiveProfileIndex = reader.ReadInt32();

            var profilesCount = reader.ReadInt32();

            SettingsProfileIndex = new Dictionary<int, SettingsProfile<SettingsType>>();

            for (int i = 0; i < profilesCount; i++)
            {
                var profileKey = reader.ReadInt32();
                var profile = SettingsProfile<SettingsType>.Read(reader, SettingsProfileIndex);

                SettingsProfileIndex.Add(profileKey, profile);

            }

            ActiveSettings = SettingsProfileIndex[ActiveProfileIndex].Settings;
        }


        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var properties = sender.GetType().GetProperties();
            var instance = Activator.CreateInstance(e.GetType());

            foreach (var property in properties)
            {
                if (property.Name == e.PropertyName)
                { }
            }

        }


        /// <summary>
        /// Carga la configuración si es que encuentra un archivo, sino crea una nueva
        /// </summary>
        public void LoadOrCreate()
        {
            if (File.Exists(FilePath))
            {
                using (var reader = new BinaryReader(File.Open(FilePath, FileMode.Open)))
                {
                    Read(reader);
                    //SettingsBase.Read(reader);
                }
            }
            else
            {
                var DefaultConfiguration = new SettingsProfile<SettingsType>()
                {
                    Name = "Default",
                    BasedOnKey = null, // no se basa en nadie
                    Settings = ActiveSettings,
                    IsOverriden = new Dictionary<MultipleIndexKey, bool>()
                };

                ActiveProfileIndex = 0;
                ActiveSettings = DefaultConfiguration.Settings; // El activo

                SettingsProfileIndex = new Dictionary<int, SettingsProfile<SettingsType>>() { { 0, DefaultConfiguration } };

                #region OTROS PERFILES
                SettingsType nullSettings = new SettingsType();
                CreateConfiguration(0, "FeatureA", null, 1,false, nullSettings);
                CreateConfiguration(1, "ModelingA", null, 2, false, nullSettings);
                CreateConfiguration(2, "Result1", null, 3, false, nullSettings);
                CreateConfiguration(3, "Neutral1", null, 4, false, nullSettings);

                CreateConfiguration(4, "FeatureB", null, 5, false, nullSettings);
                CreateConfiguration(5, "ModelingB", 2, 6, false, nullSettings);
                CreateConfiguration(6, "Result2", 3, 7, false, nullSettings);
                CreateConfiguration(7, "Neutral2", 4, 8, false, nullSettings);

                CreateConfiguration(8, "ModelingC", null, 9, false, nullSettings);
                #endregion
            }
        }

        /// <summary>
        /// Carga 4 configuraciones que ya estén guardadas
        /// </summary>
        /// <param name="feature">Configuración de características</param>
        /// <param name="modeling">Configuración de modelado</param>
        /// <param name="result">Configuración de resultado</param>
        /// <param name="neutral">Otras settings</param>
        public void CreateConfigurationSet(string name, int? basedOnKey, string feature, string modeling, string result, string neutral)
        {
            var featureConfiguration = new SettingsProfile<SettingsType>();
            var modelingConfiguration = new SettingsProfile<SettingsType>();
            var resultConfiguration = new SettingsProfile<SettingsType>();
            var neutralConfiguration = new SettingsProfile<SettingsType>();

            // Asigna las 4 configuraciones
            foreach (var profileEntry in SettingsProfileIndex)
            {
                var profile = profileEntry.Value;

                if (profile.Name == feature)
                    featureConfiguration = profile;

                else if (profile.Name == modeling)
                    modelingConfiguration = profile;

                else if (profile.Name == result)
                    resultConfiguration = profile;

                else if (profile.Name == neutral)
                    neutralConfiguration = profile;
            }

            var settings = new SettingsProfile<SettingsType>();
            settings.Settings = new SettingsType();

            var settingsProperties = settings.Settings.GetType().GetProperties();
            //var featureProperties = featureConfiguration.Settings.GetType().GetProperties();
            //var modelingProperties = modelingConfiguration.Settings.GetType().GetProperties();
            //var resultProperties = resultConfiguration.Settings.GetType().GetProperties();
            //var neutralProperties = neutralConfiguration.Settings.GetType().GetProperties();

            var i = 0;
            foreach (var property in settingsProperties)
            {
                var featureValue = settingsProperties[i].GetValue(featureConfiguration.Settings);
                if (featureValue != null)
                    property.SetValue(settings.Settings, featureValue);

                var modelingValue = settingsProperties[i].GetValue(modelingConfiguration.Settings);
                if (modelingValue != null)
                    property.SetValue(settings.Settings, modelingValue);

                var resultValue = settingsProperties[i].GetValue(resultConfiguration.Settings);
                if (resultValue != null)
                    property.SetValue(settings.Settings, resultValue);

                var neutralValue = settingsProperties[i].GetValue(neutralConfiguration.Settings);
                if (neutralValue != null)
                    property.SetValue(settings.Settings, neutralValue);

                i++;
            }

            CreateConfiguration(-1, name, null, 10, true, settings.Settings);
        }

        /// <summary>
        /// Crea configuraciones
        /// </summary>
        /// <param name="TEMP">Index para settings correspondientes (desaparecerá)</param>
        /// <param name="name">Nombre de la configuración</param>
        /// <param name="basedOnKey">Key en la que se basa la configuración. Puede ser null (no se basa en ninguno)</param>
        /// <param name="key">Key que corresponde a la configuración. -1 para agregar al final.</param>
        public void CreateConfiguration(int TEMP, string name, int? basedOnKey, int key, bool existingSettings, SettingsType _settings)
        {
            var settings = basedOnKey != null ? SettingsProfileIndex[(int)basedOnKey].Settings : (existingSettings ? _settings : TEMPSettings[TEMP]);

            var configuration = new SettingsProfile<SettingsType>()
            {
                Name = name,
                BasedOnKey = basedOnKey != null ? basedOnKey : null,
                Settings = settings,
                IsOverriden = new Dictionary<MultipleIndexKey, bool>()
            };

            SettingsProfileIndex.Add(key != -1 ? key : SettingsProfileIndex.Count, configuration);
        }
    }
}
