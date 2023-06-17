using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aitoe.Vigilant.Controller.BL.Entites;
using System.IO;
using System.Diagnostics;
using Aitoe.Vigilant.Controller.BL.ExceptionDefs;

namespace Aitoe.Vigilant.Controller.DL.Services
{
    public class PushbulletService : IPushbulletService
    {
        public bool ConfigurePushbullet()
        {
            //SetPushbulletConfigurationMessageVisibility();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = @"Pushbullet.Cli.exe";
            psi.Arguments = "--rp";
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            using (Process pushBulletWPFProcess = Process.Start(psi))
            {
                pushBulletWPFProcess.WaitForExit();
            }
            return true;
        }

        public bool SendPushbulletMessage(IEmail pushbulletMessage)
        {

            //var _sCommandArgs = "-t " + "\"Test Mail-GbdbMailer 1\" " + "-b \"" + "This is from GbDbMailer. Tesing p option WITH 1 attachment\" " + "-e \"" + "Vivekanand.Swamy@gmail.com\"" + " -m 1 -p 0 -i \"" + "D:\\Lotus123.jpg" + "\"";
            //var gbdbmailerProcess = new Process();
            //var processStartInfo = new ProcessStartInfo();
            //processStartInfo.CreateNoWindow = true;
            //processStartInfo.UseShellExecute = false;
            //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //processStartInfo.FileName = _gbdbmailerPath;

            //if (!string.IsNullOrEmpty(MessageAttachmentFilePath))
            //    _sCommandArgs = _sCommandArgs + " -i " + "\"" + MessageAttachmentFilePath + "\"";

            //processStartInfo.Arguments = _sCommandArgs;

            //gbdbmailerProcess.StartInfo = processStartInfo;

            //gbdbmailerProcess.EnableRaisingEvents = true;

            //gbdbmailerProcess.Exited += (s, ea) =>
            //{
            //    int exitCode = gbdbmailerProcess.ExitCode;
            //    //if (exitCode == 0)
            //    //    MessageConfigurationState = 6;
            //    //else
            //    //    MessageConfigurationState = exitCode;
            //};

            //MessageConfigurationState = 5;
            //gbdbmailerProcess.Start();


            List<string> argList = new List<string>();
            string title = pushbulletMessage.ToTitle;
            string body = pushbulletMessage.ToBody;

            if (pushbulletMessage.ToAddressList.Count == 0)
                throw new AitoeMessageServiceException("Address list count is 0", AitoeErrorCodes.PushbulletAddressListIsInvalid);

            string email = pushbulletMessage.ToAddressList[0];
            string filenameAttachment = string.Empty;
            if (pushbulletMessage.Attachments.Count > 0)
            {
                foreach (var attachment in pushbulletMessage.Attachments)
                {
                    if (!string.IsNullOrEmpty(filenameAttachment))
                        filenameAttachment = filenameAttachment + ", ";

                    filenameAttachment = filenameAttachment + attachment;
                }

            }

            //string filenameAttachment = arguments["i"] != null ? arguments["i"] : String.Empty;
            //string filenameUpload = arguments["f"] != null ? arguments["f"] : String.Empty;
            //string rpparam = " -rp " + arguments["rp"] != null ? arguments["rp"] : String.Empty;
            //string args = "";
            argList.Add("-f");
            argList.Add(filenameAttachment);
            argList.Add("-e");
            argList.Add(email);
            argList.Add("-t");
            argList.Add(title);
            argList.Add("-b");
            // The following are not clear
            //1. rpparam
            //2. sharedUrls
            //-f parameter.
            //argList.Add(body + Environment.NewLine + filenameUpload + this.sharedUrls.ToString() + "\" " + rpparam);
            argList.Add(body);
            var argArray = argList.ToArray();
            //Pushbullet.Cli.Program.Main(argArray);
            return true;
        }

        public bool IsAccessTokenExists()
        {
            string dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pushbullet.WPF");
            string accessToken = Path.Combine(dataDirectory, "access_token.bin");
            if (File.Exists(accessToken))
                return true;
            return false;
        }

        public Exception GetError()
        {
            return null;
        }
    }
}
