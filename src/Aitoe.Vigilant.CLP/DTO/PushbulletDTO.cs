using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.CLP.DTO
{
    public class PushbulletDTO : MessageDTO
    {
        public List<string> ToAddressList { get; set; }
        public string ToTitle { get; set; }
        public string ToBody { get; set; }
        public List<string> Attachments { get; set; }

        public PushbulletDTO()
        {

        }
    }
}
