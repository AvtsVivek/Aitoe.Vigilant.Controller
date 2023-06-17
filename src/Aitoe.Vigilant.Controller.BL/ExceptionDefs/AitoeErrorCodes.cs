using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.ExceptionDefs
{
    public enum AitoeErrorCodes
    {
        OtherFailure = 40001,
        MultipleFailures = 40002,

        // Email error codes. Start with 50000
        SMTPHostNotSet = 50001,
        SendersEmailAddressIsInvalid = 50002,
        SMTPServerIsInvalid = 50003,
        EmailAddressListIsInvalid = 50004,

        // Pushbullet error codes. Start with 60000
        PushbulletAddressListIsInvalid = 60001,


        // Dropbox error codes. Start with 30000


        // WebRequestCodes startwith 70000
        WebRequestException = 70001,
        InAppropriateLoginResponse = 70002,
        InAppropriateCodeRecieved = 70003,
        InAppropriateLicenseDataForCustomerId = 70004,
        InAppropriateNumberOfCamsRecieved = 70005,
        LicenseDataDeserializationUnsuccessful = 80001
    }
}
