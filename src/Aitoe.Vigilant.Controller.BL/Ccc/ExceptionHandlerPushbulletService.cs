using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aitoe.Vigilant.Controller.BL.Entites;
using log4net;
using Aitoe.Vigilant.Controller.BL.Infra;
using System.Reflection;

namespace Aitoe.Vigilant.Controller.BL.Ccc
{
    public class ExceptionHandlerPushbulletService : IPushbulletService
    {
        private readonly IPushbulletService _PushbulletService;
        private Exception _PushbulletException = null;
        private readonly ILog _Log;
        public ExceptionHandlerPushbulletService(IPushbulletService pushbulletService, ILog log)
        {
            if (pushbulletService == null)
                throw new ArgumentNullException("Pushbullet Service is null");
            _PushbulletService = pushbulletService;

            if (log == null)
                throw new ArgumentNullException("Log is null");
            _Log = log;
        }
        public bool ConfigurePushbullet()
        {
            try
            {
                return _PushbulletService.ConfigurePushbullet();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _PushbulletException = ex.GetSummaryAitoeBaseException();
                return false;
            }
        }

        public Exception GetError()
        {
            return _PushbulletException;
        }

        public bool IsAccessTokenExists()
        {
            try
            {
                return _PushbulletService.IsAccessTokenExists();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _PushbulletException = ex.GetSummaryAitoeBaseException();
                return false;
            }
        }

        public bool SendPushbulletMessage(IEmail pushbulletMessage)
        {
            try
            {
                return _PushbulletService.SendPushbulletMessage(pushbulletMessage);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _PushbulletException = ex.GetSummaryAitoeBaseException();
                return false;
            }
        }
    }
}
