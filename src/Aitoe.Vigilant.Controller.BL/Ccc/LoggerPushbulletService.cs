using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aitoe.Vigilant.Controller.BL.Entites;
using log4net;

namespace Aitoe.Vigilant.Controller.BL.Ccc
{
    public class LoggerPushbulletService : IPushbulletService
    {
        private readonly IPushbulletService _PushbulletService;
        private readonly ILog _Log;
        public LoggerPushbulletService(IPushbulletService pushbulletService, ILog log)
        {
            if (pushbulletService == null)
                throw new ArgumentNullException("Pushbullet Service is null");
            _PushbulletService = pushbulletService;

            if (log == null)
                throw new ArgumentNullException("Log is null");
            _Log = log;
            _Log.Info("LoggerPushbulletService");
        }
        public bool ConfigurePushbullet()
        {
            _Log.Info("ConfigurePushbullet");
            return _PushbulletService.ConfigurePushbullet();
        }

        public Exception GetError()
        {
            _Log.Info("GetError");
            return _PushbulletService.GetError();
        }

        public bool IsAccessTokenExists()
        {
            _Log.Info("IsAccessTokenExists");
            return _PushbulletService.IsAccessTokenExists();
        }

        public bool SendPushbulletMessage(IEmail pushbulletMessage)
        {
            _Log.Info("SendPushbulletMessage");
            return _PushbulletService.SendPushbulletMessage(pushbulletMessage);
        }
    }
}
