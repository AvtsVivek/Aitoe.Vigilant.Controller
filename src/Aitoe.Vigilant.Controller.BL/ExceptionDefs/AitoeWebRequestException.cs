using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.ExceptionDefs
{
    public class AitoeWebRequestException : AitoeBaseException
    {
        public AitoeWebRequestException()
        {

        }

        public AitoeWebRequestException(AitoeErrorCodes errorCode)
        {
            _ErrorCode = errorCode;
        }

        public AitoeWebRequestException(string message) : base(message)
        {

        }

        public AitoeWebRequestException(string message, AitoeErrorCodes errorCode) : base(message)
        {
            _ErrorCode = errorCode;
        }

        public AitoeWebRequestException(string message, Exception inner) : base(message, inner)
        {

        }
        public AitoeWebRequestException(string message, Exception inner, AitoeErrorCodes errorCode) : base(message, inner)
        {
            _ErrorCode = errorCode;
        }


    }
}
