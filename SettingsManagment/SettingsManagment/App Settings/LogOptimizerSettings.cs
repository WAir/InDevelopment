using System;
using SettingsManagment.Attributes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SettingsManagment.Interfaces;

namespace SettingsManagment
{
    /// <summary>
    /// Ejemplo de clase Settings
    /// </summary>
    /// <remarks>
    ///  Abajo aparecen propiedades que combinan los conceptos Supported - Custom - Atomic
    /// 
    /// Supported 
    ///     Se refiere a que el administrador de settings o el que lee/guarda settings puede lidiar con esta clase sin que el usuario haga nada
    ///     Por ejemplo un Rectangle, el adminstrador sabe como guardar y cargar Rectangles (en el lado del aministrador se implemetaran varios codigos para guardar/cargar las clases comunes como Rectangle)
    /// 
    /// Custom
    ///		Una clase que el administrador de settings no conoce y por lo tanto no sabe como guardarla o cargarla
    ///		falta por DISEÑAR esta parte ¿ Que haria el usuario para este caso ? ¿ El define los metodos que guardan y cargan la clase ? ¿ La clase debe implementar alguna interfaz ?  
    ///		No creo que seria buena idea la interfaz, porque las clases tendrian que andar implementando una interfaz solo porque en alguna parte del codigo existe algo que las guarda
    /// 
    /// Atomic
    ///		se refiere que no es una clase de varios settings, si no mas bien todas las propiedades de esta clase represetan 1 solo setting
    /// 
    /// Settings Class
    ///		Es lo opuesto a Atomic. Las propiedades de esta clase, cada una es un setting.
    ///		Es necesario re-aplicar toda la logica de guardar/leer una clase de settings, a esta propiedad.
    /// 
    /// </remarks>
    [DeprecatedSettings(
    )]
    [Version(1)]
    public class LogOptimizerSettings : SettingsBase
    {

        private string _Feature, _Modeling, _Result, _Neutral;

        [Feature]
        [Key(1)]
        public string Feature
        {
            get { return _Feature; }
            set { _Feature = value; NotifyPropertyChanged(); }
        }

        [AlterModeling]
        [Key(2)]
        public string Modeling
        {
            get { return _Modeling; }
            set { _Modeling = value; NotifyPropertyChanged(); }
        }

        [AlterResult]
        [Key(3)]
        public string Result
        {
            get { return _Result; }
            set { _Result = value; NotifyPropertyChanged(); }
        }

        [Key(4)]
        public string Neutral
        {
            get { return _Neutral; }
            set { _Neutral = value; NotifyPropertyChanged(); }
        }

        #region Originales
        /// <summary>
        /// Privimite Value
        /// Los primitivos deberian tener un metodo del BinaryReader/Writer asociado por defecto
        /// por ejemplo int es ReadInt32();
        /// </summary>
        //[Key(1)]
        //public bool PrimitiveValue { get; set; }

        //[Key(2)]
        //public Side EnumValue { get; set; }

        //[Key(3)]
        //public NominalLength CustomAtomicClass { get; set; }

        //[Key(4)]
        //public string SettingString { get; set; }

        //[Key(5)]
        //public Rectangle SupportedAtomicClass { get; set; }

        //[Key(6)]
        //public PartialScanSettings SettingsClass { get; set; }

        //[Key(7)]
        //public Dictionary<int, NominalLength> CustomAtomicClassDictionary { get; set; }

        //[Key(8)]
        //public List<int> PrimiviteList { get; set; }

        //[Key(9)]
        //public List<Rectangle> SupportedAtomicClassList { get; set; }

        //[Key(10)]
        //public List<NominalLength> CustomAtomicClassList { get; set; }

        //[Key(11)]
        //public Dictionary<int, string> PrimiviteAtomicoDictionary { get; set; }

        //[Key(12)]
        //public Dictionary<int, Rectangle> SupportedAtomicClassDictionary { get; set; }

        #endregion

        public LogOptimizerSettings()
        {
            //FeatureA = "Name Configuration A";
            //FeatureB = "Name Configuration B";
            //ModelingA = "Modeling A";
            //ModelingB = "Modeling B";
            //ResultA = "Result A";
            //ResultB = "Result B";


            //PrimitiveValue = true;
            //EnumValue = Side.Top;
            //CustomAtomicClass = null;
            //CustomAtomicClass = new NominalLength(3000.2, 3000);
            //SettingString = "Default setting";

            //SupportedAtomicClass = new Rectangle(1, 1);
            //SettingsClass = new PartialScanSettings(true);

            //PrimiviteList = new List<int>() { 1, 2, 3 };
            //SupportedAtomicClassList = new List<Rectangle>() { new Rectangle(10, 20), new Rectangle(30, 40), new Rectangle(50, 60) };
            //CustomAtomicClassList = new List<NominalLength>() { new NominalLength(2000.1, 2000), new NominalLength(4000.5, 4000) };
            //PrimiviteAtomicoDictionary = new Dictionary<int, string>() { { 5, "Prueba1" }, { 6, "Pedro" }, { 10, "Roberto" } };
            //SupportedAtomicClassDictionary = new Dictionary<int, Rectangle>() { { 12, new Rectangle(5, 5) }, { 16, new Rectangle(22, 23) } };
            //CustomAtomicClassDictionary = new Dictionary<int, NominalLength>() { { 2, new NominalLength(1000.5, 1000) } };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conf">Palabra clave de la configuración</param>
        public LogOptimizerSettings(string conf)
        {
            switch (conf)
            {
                case "Default":
                    Feature = "Feature default";
                    Modeling = "Modeling default";
                    Result = "Result default";
                    Neutral = "Neutral default";
                    break;

                case "FeatureA":
                    Feature = "Feature A";
                    break;

                case "FeatureB":
                    Feature = "Feature B";
                    break;

                case "FeatureC":
                    Feature = "Feature C";
                    break;

                case "ModelingA":
                    Modeling = "Modeling A";
                    break;

                case "ModelingB":
                    Modeling = "Modeling B";
                    break;

                case "ModelingC":
                    Modeling = "Modeling C";
                    break;

                case "ModelingD":
                    Modeling = "Modeling D";
                    break;

                case "Result1":
                    Result = "Result 1";
                    break;

                case "Result2":
                    Result = "Result 2";
                    break;

                case "Neutral1":
                    Neutral = "Neutral 1";
                    break;

                case "Neutral2":
                    Neutral = "Neutral 2";
                    break;

                default:
                    break;
            }
        }


    }
}
