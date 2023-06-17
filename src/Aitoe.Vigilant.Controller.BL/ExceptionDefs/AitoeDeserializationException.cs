using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.ExceptionDefs
{
    public class AitoeDeserializationException : AitoeBaseException
    {
        public AitoeDeserializationException(AitoeErrorCodes errorCode)
        {
            _ErrorCode = errorCode;
        }
    }
}
