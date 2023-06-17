using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Infra.Extensions
{
    public class StringConvertExtensions
    {
        private readonly ILog _Log;
        public StringConvertExtensions(ILog log)
        {
            if (log == null)
                throw new ArgumentNullException("Log is null");
            _Log = log;
        }

        public T Deserialize<T>(string s)
        {
            try
            {
                if (string.IsNullOrEmpty(s))
                    return default(T);

                byte[] licenseDataBytes = Convert.FromBase64String(s);

                if (licenseDataBytes == null || licenseDataBytes.Length == 0)
                    return default(T);

                var ms = new MemoryStream(licenseDataBytes);

                if (ms == null)
                    return default(T);

                using (ms)
                {
                    var bf = new BinaryFormatter();
                    var ld = bf.Deserialize(ms);
                    return (T)ld;
                }
            }
            catch (Exception exc)
            {
                _Log.Error(exc.Message);
                return default(T);
            }
        }
    }
}
