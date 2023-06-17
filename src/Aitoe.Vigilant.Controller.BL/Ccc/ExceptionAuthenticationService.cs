using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.Infra;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Ccc
{
    public class ExceptionHandlerAuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationService _AuthenticationService;
        private Exception AuthenticationException = null;
        private readonly ILog _Log;

        public ExceptionHandlerAuthenticationService(IAuthenticationService authService, ILog log)
        {
            if (authService == null)
                throw new ArgumentNullException("Auth Service is null");
            _AuthenticationService = authService;

            if (log == null)
                throw new ArgumentNullException("Log is null");
            _Log = log;
        }


        //public async Task<int?> GetStatusCodeAsync(string userName, string orderId, string password)
        //{
        //    try
        //    {
        //        return await _AuthenticationService.GetStatusCodeAsync(userName, orderId, password);
        //    }
        //    catch (Exception ex)
        //    {
        //        _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
        //        AuthenticationException = ex.GetSummaryAitoeBaseException();
        //        return null;
        //    }
        //}

        public async Task<int?> GetStatusCodeAsync(IAuthDetails authDetails)
        {
            try
            {
                return await _AuthenticationService.GetStatusCodeAsync(authDetails);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                AuthenticationException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public bool? IsLoginDetailSet()
        {
            try
            {
                return _AuthenticationService.IsLoginDetailSet();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                AuthenticationException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public async Task<bool?> Login(IAuthDetails authDetails)
        {
            try
            {
                return await _AuthenticationService.Login(authDetails);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                AuthenticationException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        //public async Task<bool?> Login(string userName, string password, string customCredential, bool isPersistent)
        //{
        //    try
        //    {
        //        return await _AuthenticationService.Login(userName, password, customCredential, isPersistent);
        //    }
        //    catch (Exception ex)
        //    {
        //        _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
        //        AuthenticationException = ex.GetSummaryAitoeBaseException();
        //        return null;
        //    }
        //}

        public Task<int?> Logout()
        {
            try
            {
                return _AuthenticationService.Logout();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                AuthenticationException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public async Task<bool?> ValidateUserForOrder(IAuthDetails authDetails)
        {
            try
            {
                return await _AuthenticationService.ValidateUserForOrder(authDetails);
                //return _AuthenticationService.PersistAuthDetails(authDetails);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                AuthenticationException = ex.GetSummaryAitoeBaseException();
                return null;
            }

        }

        public bool? PersistAuthDetails(IAuthDetails authDetails)
        {
            try
            {
                return _AuthenticationService.PersistAuthDetails(authDetails);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                AuthenticationException = ex.GetSummaryAitoeBaseException();
                return null;
            }

        }

        public IAuthDetails GetAuthDetails()
        {
            try
            {
                return _AuthenticationService.GetAuthDetails();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                AuthenticationException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public int? GetCamerasLicensed()
        {
            try
            {
                return _AuthenticationService.GetCamerasLicensed();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                AuthenticationException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public async Task<int?> ExtractNoOfCameras(IAuthDetails authDetails)
        {
            try
            {
                return await _AuthenticationService.ExtractNoOfCameras(authDetails);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                AuthenticationException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public async Task<bool?> GetActivatedFlag(IAuthDetails authDetails)
        {
            try
            {
                return await _AuthenticationService.GetActivatedFlag(authDetails);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                AuthenticationException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public async Task<bool?> UnsetActivatedFlag(IAuthDetails authDetails)
        {
            try
            {
                return await _AuthenticationService.UnsetActivatedFlag(authDetails);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                AuthenticationException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public async Task<bool?> SetActivatedFlag(IAuthDetails authDetails)
        {
            try
            {
                return await _AuthenticationService.SetActivatedFlag(authDetails);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                AuthenticationException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

    }
}
