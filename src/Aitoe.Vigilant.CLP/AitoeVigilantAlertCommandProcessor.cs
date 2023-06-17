using Aitoe.Vigilant.CLP.DTO;
using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.ExceptionDefs;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Aitoe.Vigilant.CLP
{
    internal class AitoeVigilantAlertCommandProcessor : ProcessorTemplateBase<AitoeVigilantAlertOptions>
    {
        //private int _linesProcessed; // This will most likely be one line for our case.

        private readonly IEmailService _EmailService;
        private readonly IMapper _InternalMapper;
        private readonly IPushbulletService _PushbulletService;
        private readonly IDropboxService _DropboxService;
        public AitoeVigilantAlertCommandProcessor(IEmailService emailService, IPushbulletService pushbulletService, IMapper mapper, IDropboxService dropboxService)
        {
            if (emailService == null)
                throw new ArgumentNullException("email Service is null");
            _EmailService = emailService;

            if (mapper == null)
                throw new ArgumentNullException("Mapper is null");
            _InternalMapper = mapper;

            if (pushbulletService == null)
                throw new ArgumentNullException("pushbullet service is null");
            _PushbulletService = pushbulletService;

            if (dropboxService == null)
                throw new ArgumentNullException("dropbox service is null");
            _DropboxService = dropboxService;
        }

        //protected MessageDTO {get; set;}

        public MessageDTO _EmailDTO { get; set; }
        public MessageDTO _PushbulletDTO { get; set; }

        protected override void PreProcess()
        {
            string sDropBoxLink = string.Empty;
            if (!string.IsNullOrWhiteSpace(Options.DropboxFile))
            {
                sDropBoxLink = UploadFileToDropbox(Options.DropboxFile);
            }

            //Output.WriteLine("Converting to: " + Options.TargetCase);
            Output.WriteLine("Attempting sending message to: " + Options.GetAlertDestinations());
            if(Options.UseEmail)
            {
                var edto = new EmailDTO();
                edto.ToTitle = Options.Title;
                edto.ToBody = Options.Body + Environment.NewLine + sDropBoxLink;
                edto.ToAddressList = Options.EmailAddress.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                edto.Attachments = Options.EmailAttachments != null ? Options.EmailAttachments.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList() : new List<string>();
                _EmailDTO = edto;
            }

            if (Options.UsePushbullet)
            {
                var pbdto = new PushbulletDTO();
                pbdto.ToTitle = Options.Title;
                pbdto.ToBody = Options.Body + Environment.NewLine + sDropBoxLink;
                pbdto.ToAddressList = Options.PushbulletAddress.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                pbdto.Attachments = Options.EmailAttachments != null ? Options.EmailAttachments.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList() : new List<string>();
                _PushbulletDTO = pbdto;
            }
        }

        protected override void ProcessLines()
        {
            if (Options.UseEmail && Options.UsePushbullet)
            {
                TrySendingPushbulletAndEmail();
            }
            else
            {
                if (Options.UseEmail)
                {
                    TrySendingEmail();
                }
                if (Options.UsePushbullet)
                {
                    TrySendingPushbullet();
                }
            }
        }

        private string UploadFileToDropbox(string dropboxFile)
        {
            if(!File.Exists(dropboxFile))
                Output.WriteLine(dropboxFile + " does not exist. So ignoring and procceding.");

            //var metaData = _DropboxService.UploadFileToDropBox1(dropboxFile, _InnerFolder);
            var metaData = _DropboxService.UploadFileToDropBox(dropboxFile, _InnerFolder);

            //FileMetadata metaData;
            //try
            //{
            //    metaData = Task.Run(() => _DropboxService.UploadFileToDropBox(dropboxFile, _InnerFolder)).WaitAndUnwrapException();
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}



            //var metaData = AsyncContext.Run(() => _DropboxService.UploadFileToDropBox(dropboxFile, _InnerFolder));

            //try
            //{
            //    metaData.Wait();
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}



            var currentApp = _DropboxService.CurrentApplication;

            var fileName = Path.GetFileName(dropboxFile);

            var finalPathOnDropbox = "/" + currentApp + "/" + _InnerFolder.Trim(new Char[] { ' ', '/' }) + "/" + fileName;

            var finalPathOnDropboxFromMetadata = metaData.Result.PathDisplay;

            if (finalPathOnDropbox == finalPathOnDropboxFromMetadata)
            {
                // things are ok. So 
                Output.WriteLine("File {0} uploaded to folder {1} successfully.", dropboxFile, _InnerFolder);
                Output.WriteLine("Full path is {0}", finalPathOnDropbox);
                return "https://www.dropbox.com/home/Apps/Aitoe" + finalPathOnDropbox;
            }
            else
                return null;
        }

        private string _InnerFolder = DateTime.Now.Year.ToString() + "-" + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(DateTime.Now.Month).ToUpper() + "-" + DateTime.Now.Day.ToString();
        protected override void PostProcess()
        {
            //Output.WriteLine("{0} lines processed.", _linesProcessed);
            // Simply send any error message or success message.
        }

        private void TrySendingPushbulletAndEmail()
        {
            var pbMessage = _InternalMapper.Map<IEmail>((PushbulletDTO)_PushbulletDTO);
            var respPb = _PushbulletService.SendPushbulletMessage(pbMessage);

            var email = _InternalMapper.Map<IEmail>((EmailDTO)_EmailDTO);
            var respEmail = _EmailService.TrySendEmail(email);

            if (respEmail && respPb)
                Environment.Exit((int)ExitCodes.RegularExit);
            else if (respEmail && !respPb)
            {
                var e = _PushbulletService.GetError();
                if (e != null && e is AitoeBaseException)
                {
                    var abe = e as AitoeBaseException;
                    AitoeErrorCodes aec = abe.GetErrorCode();
                    Environment.Exit((int)aec);
                }
                else
                    Environment.Exit((int)ExitCodes.OtherFailure);

            }
            else if (!respEmail && respPb)
            {
                var e = _EmailService.GetError();
                if (e != null && e is AitoeBaseException)
                {
                    var abe = e as AitoeBaseException;
                    AitoeErrorCodes aec = abe.GetErrorCode();
                    Environment.Exit((int)aec);
                }
                else
                    Environment.Exit((int)ExitCodes.OtherFailure);
            }
            else
            {
                var e1 = _EmailService.GetError();
                var e2 = _PushbulletService.GetError();
                AitoeErrorCodes aec1, aec2;
                if (e1 != null && e1 is AitoeBaseException)
                {
                    var abe1 = e1 as AitoeBaseException;
                    aec1 = abe1.GetErrorCode();
                    //Environment.Exit((int)aec1);
                }

                if (e2 != null && e2 is AitoeBaseException)
                {
                    var abe2 = e2 as AitoeBaseException;
                    aec2 = abe2.GetErrorCode();
                    //Environment.Exit((int)aec2);
                }

                Environment.Exit((int)AitoeErrorCodes.MultipleFailures);
            }
        }

        private void TrySendingPushbullet()
        {
            var pbMessage = _InternalMapper.Map<IEmail>((PushbulletDTO)_PushbulletDTO);
            var resp = _PushbulletService.SendPushbulletMessage(pbMessage);

            if (resp == true)
            {
                Environment.Exit((int)ExitCodes.RegularExit);
            }
            else
            {
                var e = _PushbulletService.GetError();
                if (e != null && e is AitoeBaseException)
                {
                    var abe = e as AitoeBaseException;
                    AitoeErrorCodes aec = abe.GetErrorCode();
                    Environment.Exit((int)aec);
                }
                else
                    Environment.Exit((int)ExitCodes.OtherFailure);
            }
        }

        private void TrySendingEmail()
        {
            var email = _InternalMapper.Map<IEmail>((EmailDTO)_EmailDTO);
            var resp = _EmailService.TrySendEmail(email);

            if (resp == true)
            {
                //Console.WriteLine("Email is sent");
                Environment.Exit((int)ExitCodes.RegularExit);
            }
            else
            {
                var e = _EmailService.GetError();
                if (e != null && e is AitoeBaseException)
                {
                    var abe = e as AitoeBaseException;
                    AitoeErrorCodes aec = abe.GetErrorCode();
                    Environment.Exit((int)aec);
                }
                else
                    Environment.Exit((int)ExitCodes.OtherFailure);
            }
        }
    }
}
