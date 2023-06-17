using Aitoe.Vigilant.Controller.BL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.DL.Entities
{
    public class EmailMessage : IEmail
    {
        private List<string> _attachments;
        public List<string> Attachments
        {
            get
            {
                return _attachments;
            }

            set
            {
                _attachments = value;
            }
        }

        private string _toBody = string.Empty;
        public string ToBody
        {
            get
            {
                return _toBody;
            }

            set
            {
                _toBody = value;
            }
        }

        private List<string> _toAddressList = new List<string>();
        public List<string> ToAddressList
        {
            get
            {
                return _toAddressList;
            }

            set
            {
                _toAddressList = value;
            }
        }

        private string _toTitle = string.Empty;
        public string ToTitle
        {
            get
            {
                return _toTitle;
            }

            set
            {
                _toTitle = value;
            }
        }
    }
}
