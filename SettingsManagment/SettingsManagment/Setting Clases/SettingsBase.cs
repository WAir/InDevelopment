using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using SettingsManagment.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SettingsManagment
{

    public class SettingsBase : ISetting, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Guarda la versión actual
        /// </summary>
        public bool Read(BinaryReader reader)
        {
            var version = reader.ReadInt32();
            try
            {
                switch (version)
                {
                    case 1:
                        return ReadV1(reader);
                    case 2:
                        return ReadV2(reader);
                    case 3:
                        return ReadV2(reader);
                    case 4:
                        return ReadV2(reader);
                    case 5:
                        return ReadV2(reader);
                    case 6:
                        return ReadV2(reader);
                    case 7:
                        return ReadV2(reader);
                    case 8:
                        return ReadV2(reader);
                    case 9:
                        return ReadV2(reader);
                    case 10:
                        return ReadV2(reader);
                    default:
                        //Logger.IsNotValid(this, nameof(version), version);
                        break;
                }
            }
            catch (NotImplementedException)
            {
                //Logger.IsNotValid(this, nameof(version), version, "Metodo para leer esta versión no implementado");
            }
            catch (Exception e)
            {
                //Logger.Error(this, e);

            }
            return false;
        }

        /// <summary>
        /// Lee la versión 1
        /// </summary>
        /// <param name="reader">Lector binario</param>
        /// <returns>TRUE si esta es la version actual, FALSE en caso contrario</returns>
        public virtual bool ReadV1(BinaryReader reader) { throw new NotImplementedException(); }

        /// <summary>
        /// Lee la versión 2
        /// </summary>
        /// <param name="reader">Lector binario</param>
        /// <returns>TRUE si esta es la version actual, FALSE en caso contrario</returns>
        public virtual bool ReadV2(BinaryReader reader) { throw new NotImplementedException(); }

        /// <summary>
        /// Lee la versión 3
        /// </summary>
        /// <param name="reader">Lector binario</param>
        /// <returns>TRUE si esta es la version actual, FALSE en caso contrario</returns>
        public virtual bool ReadV3(BinaryReader reader) { throw new NotImplementedException(); }

        /// <summary>
        /// Lee la versión 4
        /// </summary>
        /// <param name="reader">Lector binario</param>
        /// <returns>TRUE si esta es la version actual, FALSE en caso contrario</returns>
        public virtual bool ReadV4(BinaryReader reader) { throw new NotImplementedException(); }

        /// <summary>
        /// Lee la versión 5
        /// </summary>
        /// <param name="reader">Lector binario</param>
        /// <returns>TRUE si esta es la version actual, FALSE en caso contrario</returns>
        public virtual bool ReadV5(BinaryReader reader) { throw new NotImplementedException(); }

        /// <summary>
        /// Lee la versión 6
        /// </summary>
        /// <param name="reader">Lector binario</param>
        /// <returns>TRUE si esta es la version actual, FALSE en caso contrario</returns>
        public virtual bool ReadV6(BinaryReader reader) { throw new NotImplementedException(); }

        /// <summary>
        /// Lee la versión 7
        /// </summary>
        /// <param name="reader">Lector binario</param>
        /// <returns>TRUE si esta es la version actual, FALSE en caso contrario</returns>
        public virtual bool ReadV7(BinaryReader reader) { throw new NotImplementedException(); }

        /// <summary>
        /// Lee la versión 8
        /// </summary>
        /// <param name="reader">Lector binario</param>
        /// <returns>TRUE si esta es la version actual, FALSE en caso contrario</returns>
        public virtual bool ReadV8(BinaryReader reader) { throw new NotImplementedException(); }

        /// <summary>
        /// Lee la versión 9
        /// </summary>
        /// <param name="reader">Lector binario</param>
        /// <returns>TRUE si esta es la version actual, FALSE en caso contrario</returns>
        public virtual bool ReadV9(BinaryReader reader) { throw new NotImplementedException(); }

        /// <summary>
        /// Lee la versión 10
        /// </summary>
        /// <param name="reader">Lector binario</param>
        /// <returns>TRUE si esta es la version actual, FALSE en caso contrario</returns>
        public virtual bool ReadV10(BinaryReader reader) { throw new NotImplementedException(); }
    }
}
