using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.CLP
{
    public class AitoeVigilantAlertOptions
    {
        [Option('t', "Title", Required =true, HelpText ="Subject of the message")]
        public string Title { get; set; }

        [Option('b', "Body", HelpText ="Body of the message")]
        public string Body { get; set; }

        [Option('p', "Pushbullet", HelpText ="Send message using Pushbullet")]
        public bool UsePushbullet { get; set; }
        [Option('e', "Email", HelpText ="Send message using email")]
        public bool UseEmail { get; set; }

        [Option("em", HelpText ="Email Address(es), comma seperated if multiple")]
        public string EmailAddress { get; set; }

        [Option("ep", HelpText = "Pushbullet Address(es), comma seperated if multiple")]
        public string PushbulletAddress { get; set; }

        [Option('i', "Attachments", HelpText = "Email Attachment(s), comma seperated if multiple")]
        public string EmailAttachments { get; set; }
        [Option('f', "DropboxFile", HelpText = "File to be uploaded to dropbox")]
        public string DropboxFile { get; set;}

        [ParserState] //Pluralsight Player_25. This helps in getting the parser errors.
        public IParserState ParserState { get; set; }
        [HelpOption]
        public string GetUsage()
        {
            // Based on 24the video file.
            // The following is one way to use AutoBuild. This reads the assembly info and then gives out the helptext accordingly.
            //var t = HelpText.AutoBuild(this);

            var h = new HelpText
            {
                Copyright = new CopyrightInfo("Aitoe video analytics pvt. ltd.", 2016),
                Heading = "Aitoe Commandline.",
                AddDashesToOption = true,
                AdditionalNewLineAfterOption = true
            };
            h.AddOptions(this);
            return h;
        }

        public string GetAlertDestinations()
        {
            string sDestinations = string.Empty;
            sDestinations = sDestinations + (UseEmail ? " Email" : "");
            sDestinations = sDestinations + (UsePushbullet ? " Pushbullet" : "");
            return sDestinations;
        }
    }
}
