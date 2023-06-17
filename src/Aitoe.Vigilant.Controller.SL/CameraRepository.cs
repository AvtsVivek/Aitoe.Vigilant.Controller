using Aitoe.Vigilant.Controller.SL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.SL
{
    public class CameraRepository : ICameraRepository
    {
        private SortedDictionary<string, string> NetworkCamDictionary;
        public CameraRepository()
        {
            NetworkCamDictionary = new SortedDictionary<string, string>();
            PopulateCameraDictionaryPeriodically();
        }
        public IQueryable<string> GetAllIpCameras()
        {
            return NetworkCamDictionary.Keys.ToList().AsQueryable();
        }

        private async void PopulateCameraDictionaryPeriodically()
        {
            while (true)
            {
                Console.WriteLine("Checking status at {0}", DateTime.Now);
                Console.WriteLine("The count of dictionary is {0}", NetworkCamDictionary.Count());
                await PopulateCameraDictionary();
                await Task.Delay(2000);
            }
        }

        private async Task PopulateCameraDictionary()
        {
            var cameraResponseMessageList = await NetworkUtilities.GetSoapResponsesFromCamerasAsync();

            //var cameraResponseMessageList = taskPingCamerasAndGetResponses.Result;
            //var sList = String.Empty;
            //List<string> camIPAddressList = new List<string>();
            //CaptureCamListTemp(cameraResponseMessageList, sList, camIPAddressList);
            //WriteCamListToFile();
            cameraResponseMessageList.ForEach(AddCamInfoToDictionary);
        }

        private void AddCamInfoToDictionary(string sCameraResponseMessage)
        {
            var sCameraIpXml = NetworkUtilities.GetCameraIpXmlFromResponseMessage(sCameraResponseMessage);
            var sCameraIp = ExtractIpAddFromXAddrs(sCameraIpXml);
            if (!NetworkCamDictionary.Keys.Contains(sCameraIp))
                NetworkCamDictionary.Add(sCameraIp, sCameraResponseMessage);
        }

        private static string ExtractIpAddFromXAddrs(string s)
        {
            String IPV4_Pattern = "(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";

            Match IPV4AddMatch = Regex.Match(s, IPV4_Pattern);

            if (IPV4AddMatch.Success)
                return IPV4AddMatch.Value;
            else
                return String.Empty;
        }
    }
}
