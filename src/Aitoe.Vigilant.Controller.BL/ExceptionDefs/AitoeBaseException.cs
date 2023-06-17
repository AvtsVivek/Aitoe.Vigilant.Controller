using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.ExceptionDefs
{
    [Serializable]
    public class AitoeBaseException : Exception
    {
        protected AitoeErrorCodes _ErrorCode;
        public AitoeBaseException()
        {

        }

        public AitoeBaseException(AitoeErrorCodes errorCode)
        {
            _ErrorCode = errorCode;
        }

        public AitoeBaseException(string message) : base(message)
        {

        }

        public AitoeBaseException(string message, AitoeErrorCodes errorCode) : base(message)
        {
            _ErrorCode = errorCode;
        }

        public AitoeBaseException(string message, Exception inner) : base(message, inner)
        {

        }
        public AitoeBaseException(string message, Exception inner, AitoeErrorCodes errorCode) : base(message, inner)
        {
            _ErrorCode = errorCode;
        }

        public override string ToString()
        {
            string sMessage = base.ToString() + Environment.NewLine;
            sMessage = sMessage + _ErrorCode.ToString() + Environment.NewLine;
            return sMessage;
        }
        public AitoeErrorCodes GetErrorCode()
        {
            return _ErrorCode;
        }
    }
}
