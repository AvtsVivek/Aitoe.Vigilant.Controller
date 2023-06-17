using Aitoe.Vigilant.Controller.BL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.ServiceInterfaces
{
    public interface IAuthenticationService
    {
        //https://msdn.microsoft.com/en-us/library/system.web.applicationservices.authenticationservice(v=vs.110).aspx
        bool? IsLoginDetailSet();
        Task<bool?> Login(IAuthDetails authDetails);
        Task<int?> Logout();
        Task<bool?> ValidateUserForOrder(IAuthDetails authDetails);
        Task<int?> GetStatusCodeAsync(IAuthDetails authDetails);
        bool? PersistAuthDetails(IAuthDetails authDetails);
        IAuthDetails GetAuthDetails();
        int? GetCamerasLicensed();
        Task<int?> ExtractNoOfCameras(IAuthDetails authDetails);
        Task<bool?> GetActivatedFlag(IAuthDetails authDetails);
        Task<bool?> UnsetActivatedFlag(IAuthDetails authDetails);
        Task<bool?> SetActivatedFlag(IAuthDetails authDetails);
    }
}