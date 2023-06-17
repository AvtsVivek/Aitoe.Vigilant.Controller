using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Ccc
{
    public class LoggerAuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationService _AuthenticationService;
        private readonly ILog _Log;

        public LoggerAuthenticationService(IAuthenticationService authService, ILog log)
        {
            if (authService == null)
                throw new ArgumentNullException("Auth Service is null");
            _AuthenticationService = authService;

            if (log == null)
                throw new ArgumentNullException("Log is null");
            _Log = log;
        }

        public IAuthDetails GetAuthDetails()
        {
            _Log.Info("GetAuthDetails");
            return _AuthenticationService.GetAuthDetails();
        }

        public bool? PersistAuthDetails(IAuthDetails authDetails)
        {
            _Log.Info("PersistAuthDetails");
            return _AuthenticationService.PersistAuthDetails(authDetails);
        }

        //public async Task<int?> GetStatusCodeAsync(string userName, string orderId, string password)
        //{
        //    _Log.Info("GetStatusCodeAsync");
        //    return await _AuthenticationService.GetStatusCodeAsync(userName, orderId, password);
        //}

        public async Task<int?> GetStatusCodeAsync(IAuthDetails authDetails)
        {
            _Log.Info("GetStatusCodeAsync");
            return await _AuthenticationService.GetStatusCodeAsync(authDetails);
        }

        public bool? IsLoginDetailSet()
        {
            _Log.Info("IsLoginDetailSet");
            return _AuthenticationService.IsLoginDetailSet();
        }

        public async Task<bool?> Login(IAuthDetails authDetails)
        {
            _Log.Info("Login");
            return await _AuthenticationService.Login(authDetails);
        }

        public Task<int?> Logout()
        {
            _Log.Info("Logout");
            return _AuthenticationService.Logout();
        }

        public async Task<bool?> ValidateUserForOrder(IAuthDetails authDetails)
        {
            _Log.Info("ValidateUserForOrder");
            return await _AuthenticationService.ValidateUserForOrder(authDetails);
        }

        public int? GetCamerasLicensed()
        {
            _Log.Info("GetCamerasLicensed");
            return _AuthenticationService.GetCamerasLicensed();
        }

        public async Task<int?> ExtractNoOfCameras(IAuthDetails authDetails)
        {
            _Log.Info("ValidateUserForOrder");
            return await _AuthenticationService.ExtractNoOfCameras(authDetails);
        }

        public async Task<bool?> GetActivatedFlag(IAuthDetails authDetails)
        {
            _Log.Info("GetActivatedFlag");
            return await _AuthenticationService.GetActivatedFlag(authDetails);
        }

        public async Task<bool?> UnsetActivatedFlag(IAuthDetails authDetails)
        {
            _Log.Info("UnsetActivatedFlag");
            return await _AuthenticationService.UnsetActivatedFlag(authDetails);
        }

        public async Task<bool?> SetActivatedFlag(IAuthDetails authDetails)
        {
            _Log.Info("SetActivatedFlag");
            return await _AuthenticationService.SetActivatedFlag(authDetails);
        }

    }
}
