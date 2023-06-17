using Aitoe.Vigilant.Controller.BL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.DL.Infra
{
    [Serializable]
    public class LicenseData
    {
        public DateTime SavedDateTime;
        public IAuthDetails AuthDetails { get; set; }
        public int? CamerasLicensed { get; set; }
        public Dictionary<string, string> HardwareData { get; private set; }
        public void SetHardwareData()
        {
            HardwareData = GetHardwareData();
        }
        public static Dictionary<string, string> GetHardwareData()
        {
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            List<ManagementObject> mol = moc.Cast<ManagementObject>().ToList();

            var mo = mol.FirstOrDefault();
            var props = mo.Properties;
            var moPropDictionary = new Dictionary<string, string>();
            foreach (var prop in props)
            {

                if (prop.Name == "LoadPercentage")
                    continue;

                if (prop.Name == "CurrentClockSpeed")
                    continue;

                if (prop.Value != null && prop.Value.ToString() != "")
                    moPropDictionary.Add(prop.Name, prop.Value.ToString());
                else
                    moPropDictionary.Add(prop.Name, string.Empty);
            }
            return moPropDictionary;
        }
    }
}