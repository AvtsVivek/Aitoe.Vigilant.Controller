using Aitoe.Vigilant.Controller.SL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.SL
{
    public class CameraService
    {
        public readonly ICameraRepository m_CamRepository;

        public CameraService(ICameraRepository camRepository)
        {
            if (camRepository == null)
                throw new ArgumentNullException("Camera repository injected into Camera Service is null.");

            m_CamRepository = camRepository;
        }

        public IQueryable<string> GetAllIpCameras()
        {
            return m_CamRepository.GetAllIpCameras();
        }

        public int GetCameraCount()
        {
            return m_CamRepository.GetAllIpCameras().Count();
        }

        
        //public async void PopulateCameraDictionaryPeriodically()
        //{
        //    while (true)
        //    {
        //        Console.WriteLine("Checking status at {0}", DateTime.Now);
        //        Console.WriteLine("The count of dictionary is {0}", NetworkCamDictionary.Count());
        //        // Put code to check status here...
        //        await PopulateCameraDictionary();
        //        // At end of loop, delay 2 seconds without blocking a thread
        //        await Task.Delay(2000); // await allows thread to return
        //                                // After 2 seconds, some thread will continue after await to loop around
        //    }
        //}

        //public void PopulateCameraDictionaryPeriodically1()
        //{
            
        //}

        //public void PopulateCameraDictionary1()
        //{

        //}

        //public async Task PopulateCameraDictionary()
        //{
        //    var cameraResponseMessageList = await NetworkUtilities.GetSoapResponsesFromCamerasAsync();
            
        //    //var cameraResponseMessageList = taskPingCamerasAndGetResponses.Result;
        //    var sList = String.Empty;
        //    List<string> camIPAddressList = new List<string>();
        //    CaptureCamListTemp(cameraResponseMessageList, sList, camIPAddressList);
        //    WriteCamListToFile();
        //    cameraResponseMessageList.ForEach(AddCamInfoToDictionary);
        //}

        //private static void CaptureCamListTemp(List<string> cameraResponseMessageList, string sList, List<string> camIPAddressList)
        //{
        //    var i = 1;
        //    cameraResponseMessageList.ForEach(r => camIPAddressList.Add(NetworkUtilities.GetCameraIpXmlFromResponseMessage(r)));
        //    camIPAddressList.Sort();
        //    camIPAddressList.ForEach(a => sList = sList + i++ + ". " + a + Environment.NewLine);
        //}

        //private void WriteCamListToFile()
        //{
        //    var sList = String.Empty;
        //    var i = 1;
        //    foreach (var cam in NetworkCamDictionary)
        //    {
        //        sList = sList + i++ + " " + cam.Key + " " + NetworkUtilities.GetCameraIpXmlFromResponseMessage(cam.Value) + Environment.NewLine;
        //    }
        //}

        //private void AddCamInfoToDictionary(string sCameraResponseMessage)
        //{
        //    var sCameraIpXml = NetworkUtilities.GetCameraIpXmlFromResponseMessage(sCameraResponseMessage);
        //    var sCameraIp = ExtractIpAddFromXAddrs(sCameraIpXml);
        //    if(!NetworkCamDictionary.Keys.Contains(sCameraIp))
        //        NetworkCamDictionary.Add(sCameraIp, sCameraResponseMessage);            
        //}
                
        //private static string ExtractIpAddFromXAddrs(string s)
        //{
        //    String IPV4_Pattern = "(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";

        //    Match IPV4AddMatch = Regex.Match(s, IPV4_Pattern);

        //    if (IPV4AddMatch.Success)
        //        return IPV4AddMatch.Value;
        //    else
        //        return String.Empty;
        //}
    }
}
