using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.ExceptionDefs
{
    [Serializable]
    public class AitoeMessageServiceException : AitoeBaseException
    {
        public AitoeMessageServiceException()
        {

        }

        public AitoeMessageServiceException(AitoeErrorCodes errorCode)
        {
            _ErrorCode = errorCode;
        }

        public AitoeMessageServiceException(string message) : base(message)
        {

        }

        public AitoeMessageServiceException(string message, AitoeErrorCodes errorCode) : base(message)
        {
            _ErrorCode = errorCode;
        }

        public AitoeMessageServiceException(string message, Exception inner) : base(message, inner)
        {

        }
        public AitoeMessageServiceException(string message, Exception inner, AitoeErrorCodes errorCode) : base(message, inner)
        {
            _ErrorCode = errorCode;
        }
    }
}
