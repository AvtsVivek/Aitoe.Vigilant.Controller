using Aitoe.Vigilant.Controller.BL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.ServiceInterfaces
{
    public interface IPushbulletService
    {
        bool IsAccessTokenExists();
        bool ConfigurePushbullet();
        bool SendPushbulletMessage(IEmail pushbulletMessage);
        Exception GetError();
    }
}
