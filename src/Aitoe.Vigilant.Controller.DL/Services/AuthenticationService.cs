using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.ExceptionDefs;
using Aitoe.Vigilant.Controller.BL.Infra;
using Aitoe.Vigilant.Controller.BL.Infra.Extensions;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.DL.Infra;
using Aitoe.Vigilant.Controller.DL.Properties;
using RestSharp;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.DL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly StringConvertExtensions _SerializeUtilities;
        private Dictionary<string, string> _CurrentHardwareData;
        private LicenseData _PersistedLicenseData;
        private IAuthDetails _AuthDetails;
        public AuthenticationService(StringConvertExtensions serializeUtilities)
        {
            if (serializeUtilities == null)
                throw new ArgumentNullException("SerializeUtilities Service is null");

            _SerializeUtilities = serializeUtilities;
            _PersistedLicenseData = DeserializeAndGetAuthDetails();
            if (_PersistedLicenseData == null)
                _PersistedLicenseData = new LicenseData() { CamerasLicensed = 0};
            _CurrentHardwareData = LicenseData.GetHardwareData();
            SetAuthDetails();
        }

        private void SetAuthDetails()
        {
            if (_CurrentHardwareData == null)
            {
                _AuthDetails = null;
                return;
            }

            if (_PersistedLicenseData == null)
            {
                _AuthDetails = null;
                return;
            }

            if (_PersistedLicenseData.AuthDetails == null)
            {
                _AuthDetails = null;
                return;
            }

            if (Compare(_CurrentHardwareData, _PersistedLicenseData.HardwareData))
            {
                //if (DateTime.Now.AddMinutes(-3) > _PersistedLicenseData.SavedDateTime)
                //{
                //    _AuthDetails = null;
                //}
                //else
                _AuthDetails = _PersistedLicenseData.AuthDetails;
            }
            else
                _AuthDetails = null;
        }

        public bool? PersistAuthDetails(IAuthDetails authDetails)
        {
            _AuthDetails = authDetails;
            using (MemoryStream ms = new MemoryStream())
            {
                Settings.Default.LD = string.Empty;
                Settings.Default.Save();
                var bf = new BinaryFormatter();
                LicenseData ld = new LicenseData();
                ld.SavedDateTime = DateTime.Now;
                ld.AuthDetails = authDetails;
                ld.SetHardwareData();

                if (_PersistedLicenseData == null)
                    ld.CamerasLicensed = 0;
                
                else
                {
                    if (_PersistedLicenseData.CamerasLicensed == null)
                        ld.CamerasLicensed = 0;
                    else
                        ld.CamerasLicensed = _PersistedLicenseData.CamerasLicensed;
                }
                bf.Serialize(ms, ld);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                Settings.Default.LD = Convert.ToBase64String(buffer);
                Settings.Default.Save();
                return true;
            }
        }
        public IAuthDetails GetAuthDetails()
        {
            return _AuthDetails;
        }

        private bool Compare(Dictionary<string, string> hd1, Dictionary<string, string> hd2)
        {
            foreach (var h1Key in hd1.Keys)
            {
                if (!hd2.ContainsKey(h1Key))
                    return false;

                if (hd1[h1Key] == hd2[h1Key])
                    continue;
                else
                    return false;
            }
            return true;
        }

        private LicenseData DeserializeAndGetAuthDetails()
        {
            var serializedObject = _SerializeUtilities.Deserialize<LicenseData>(Settings.Default.LD);
            return serializedObject;
        }

        private async Task<IRestResponse<AitoeWebResponseLoginStatus>> GetWebResponseAsync(string userName, string orderId, string password)
        {
            var client = new RestClient();
            //client.BaseUrl = new Uri("http://www.aitoeva.com/");
            //IRestRequest request = new RestRequest("/licensing/validateLicense.php", Method.POST);

            client.BaseUrl = new Uri("http://www.aitoeva.com/");
            IRestRequest request = new RestRequest("/license-api/getLicenseByCustomerEmailId", Method.POST);

            //request.AddParameter("orderid", orderId);
            request.AddParameter("emailid", userName);
            //request.AddParameter("pass", password);

            var taskCompletionSource = new TaskCompletionSource<IRestResponse<AitoeWebResponseLoginStatus>>();

            client.ExecuteAsync<AitoeWebResponseLoginStatus>(request, restResponse =>
            {
                taskCompletionSource.SetResult(restResponse);
            });
            return await taskCompletionSource.Task;
        }

        public async Task<int?> GetStatusCodeAsync(IAuthDetails authDetails)
        {
            var userName = authDetails.UserId.ToLower();
            IRestResponse<AitoeWebResponseLoginStatus> restWebResponse = await GetWebResponseAsync(userName, authDetails.OrderId, authDetails.Password);
            HttpStatusCode statusCode = restWebResponse.StatusCode;
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    {
                        var codeRecievedFromWebRequest = restWebResponse.Data.Status;
                        if (codeRecievedFromWebRequest == 0)
                        {
                            var message = "Request to " + restWebResponse.ResponseUri + " failed." + Environment.NewLine;
                            message = "Error retrieving response. Code is: ";
                            message = message + codeRecievedFromWebRequest.ToString();
                            throw new AitoeWebRequestException(message, AitoeErrorCodes.InAppropriateCodeRecieved);
                        }
                        else
                        {
                            _PersistedLicenseData.CamerasLicensed = restWebResponse.Data.Status;
                            //_PersistedLicenseData.CamerasLicensed = 1;
                            return _PersistedLicenseData.CamerasLicensed;
                        }
                    }
                default:
                    {
                        var message = "Request to " + restWebResponse.ResponseUri + " failed." + Environment.NewLine;
                        message = "Error retrieving response. Status Code is: ";
                        message = message + statusCode.ToString();
                        throw new AitoeWebRequestException(message, restWebResponse.ErrorException, AitoeErrorCodes.WebRequestException);
                    }
            }
        }

        public int? GetCamerasLicensed()
        {
            if (_PersistedLicenseData == null)
                return null;

            if (_PersistedLicenseData.CamerasLicensed == null)
                return null;

            if (_PersistedLicenseData.CamerasLicensed.HasValue)
                return _PersistedLicenseData.CamerasLicensed.Value;
            else
                return null;
        }

        public bool? IsLoginDetailSet()
        {
            if (_AuthDetails == null)
                return false;
            else
            {
                if (string.IsNullOrEmpty(_AuthDetails.UserId))
                    return false;

                if (string.IsNullOrEmpty(_AuthDetails.Password))
                    return false;

                if (string.IsNullOrEmpty(_AuthDetails.OrderId))
                    return false;

                int? camsLicensed = GetCamerasLicensed();

                if (!camsLicensed.HasValue)
                    return false;
                else if (camsLicensed.HasValue && camsLicensed.Value == 0)
                    return false;
                else
                    return true;
            }
        }

        public async Task<bool?> Login(IAuthDetails authDetails)
        {
            var userName = authDetails.UserId.ToLower();
            var client = new RestClient();
            client.BaseUrl = new Uri("http://www.aitoeva.com/");
            // The following is correct one.
            //IRestRequest request = new RestRequest("/licensing/validateUser.php", Method.POST);
            // The following is incorrect one.
            IRestRequest request = new RestRequest("/licensing/validateUser.php1111111111111", Method.POST);

            request.AddParameter("emailid", userName);
            request.AddParameter("pass", authDetails.Password);

            var taskCompletionSource = new TaskCompletionSource<IRestResponse<AitoeWebResponseLoginStatus>>();

            client.ExecuteAsync<AitoeWebResponseLoginStatus>(request, restResponse =>
            {
                taskCompletionSource.SetResult(restResponse);
            });

            var restWebResponse = await taskCompletionSource.Task;

            HttpStatusCode statusCode = restWebResponse.StatusCode;

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    {
                        var status = restWebResponse.Data.Status;
                        if (status == 0)
                        {
                            return false;
                        }
                        else if (status == 1)
                        {
                            return true;
                        }
                        else
                        {
                            var message = "Request to " + request.Resource + " failed." + Environment.NewLine;
                            message = "In appropriate response. status is: ";
                            message = message + status.ToString();
                            throw new AitoeWebRequestException(message, AitoeErrorCodes.InAppropriateLoginResponse);
                        }
                    }
                case HttpStatusCode.NotFound:
                    {
                        // this is a hack.
                        return true;
                    }
                default:
                    {
                        var message = "Request to " + request.Resource + " failed." + Environment.NewLine;
                        message = "Error retrieving response. Status Code is: ";
                        message = message + statusCode.ToString();
                        throw new AitoeWebRequestException(message, restWebResponse.ErrorException, AitoeErrorCodes.WebRequestException);
                        //throw new Exception(message, restResponse.ErrorException);
                    }
            }
        }

        public async Task<int?> Logout()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("http://www.aitoeva.com/");
            IRestRequest request = new RestRequest("/licensing/deactiveLicense.php", Method.POST);

            request.AddParameter("orderid", _AuthDetails.OrderId);

            var taskCompletionSource = new TaskCompletionSource<IRestResponse<AitoeWebResponseLoginStatus>>();

            client.ExecuteAsync<AitoeWebResponseLoginStatus>(request, restResponse =>
            {
                taskCompletionSource.SetResult(restResponse);
            });

            IRestResponse<AitoeWebResponseLoginStatus> restWebResponse = await taskCompletionSource.Task;

            HttpStatusCode statusCode = restWebResponse.StatusCode;

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    {
                        var status = restWebResponse.Data.Status;

                        _AuthDetails.UserId = string.Empty;
                        _AuthDetails.OrderId = string.Empty;
                        _AuthDetails.Password = string.Empty;

                        PersistAuthDetails(_AuthDetails);

                        if (status > 0)
                            return status;
                        else
                        {
                            var message = "Request to " + request.Resource + " failed." + Environment.NewLine;
                            message = "In appropriate response. status is: ";
                            message = message + status.ToString();
                            throw new AitoeWebRequestException(message, AitoeErrorCodes.InAppropriateLoginResponse);
                        }
                    }
                default:
                    {
                        var message = "Request to " + request.Resource + " failed." + Environment.NewLine;
                        message = "Error retrieving response. Status Code is: ";
                        message = message + statusCode.ToString();
                        throw new AitoeWebRequestException(message, restWebResponse.ErrorException, AitoeErrorCodes.WebRequestException);
                        //throw new Exception(message, restResponse.ErrorException);
                    }
            }
        }

        public async Task<bool?> ValidateUserForOrder(IAuthDetails authDetails)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("http://www.aitoeva.com/");
            // The following is correct URI
            //IRestRequest request = new RestRequest("/license-api/getLicenseByCustomerEmailId", Method.POST);
            // The following is incorrect uri
            IRestRequest request = new RestRequest("/license-api/getLicenseByCustomerEmailId11111111111", Method.POST);
            request.AddParameter("customerEmail", authDetails.UserId);
            // A user(email id as userid) can have multiple orders againest him. 
            // Get all of them by calling getLicenseByCusomerEmail
            var taskCompletionSource = new TaskCompletionSource<IRestResponse<List<AitoeWebResponseLicensesForCustomer>>>();
            client.ExecuteAsync<List<AitoeWebResponseLicensesForCustomer>>(request, restResponse =>
            {
                taskCompletionSource.SetResult(restResponse);
            });

            var restWebResponse = await taskCompletionSource.Task;

            HttpStatusCode statusCode = restWebResponse.StatusCode;
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    {
                        //var status = restWebResponse.Data.Status;
                        var orderDetails = restWebResponse.Data.Where(licenseObj => licenseObj.orderid == authDetails.OrderId).FirstOrDefault();
                        if (orderDetails != null && orderDetails.customeremailid.ToLower() == authDetails.UserId.ToLower())
                        {
                            PersistAuthDetails(authDetails);
                            return true;
                        }
                        else
                        {
                            var message = "Request to " + request.Resource + " failed." + Environment.NewLine;
                            message = message + "In appropriate response." + Environment.NewLine;
                            message = message + "url called license-api/getLicenseByCustomerEmailId" + Environment.NewLine;
                            message = message + "with paramerter customerEmail - authDetails.UserId" + Environment.NewLine;
                            throw new AitoeWebRequestException(message, AitoeErrorCodes.InAppropriateLoginResponse);
                        }
                    }
                case HttpStatusCode.NotFound:
                    {
                        // The following is a hack.
                        return true;
                    }
                case HttpStatusCode.Unauthorized:
                    {
                        // The following is a hack.
                        return true;
                    }
                default:
                    {
                        var message = "Request to " + request.Resource + " failed." + Environment.NewLine;
                        message = message + "In appropriate response." + Environment.NewLine;
                        message = message + "url called license-api/getLicenseByCustomerEmailId" + Environment.NewLine;
                        message = message + "with paramerter customerEmail - authDetails.UserId" + Environment.NewLine;
                        throw new AitoeWebRequestException(message, AitoeErrorCodes.InAppropriateLoginResponse);
                    }
            }
        }

        public async Task<bool?> GetActivatedFlag(IAuthDetails authDetails)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("http://www.aitoeva.com/");
            // The following is the correct one.
            // IRestRequest request = new RestRequest("/license-api/getActivatedFlag", Method.POST);
            // The following is the incorrect one.
            IRestRequest request = new RestRequest("/license-api/getActivatedFlag111111111111", Method.POST);
            
            request.AddParameter("orderID", authDetails.OrderId);

            var taskCompletionSource = new TaskCompletionSource<IRestResponse<List<AitoeWebResponseActivatedFlag>>>();

            //var taskCompletionSource = new TaskCompletionSource<IRestResponse<List<AitoeWebResponseNumberOfCamsPerLicense>>>();

            client.ExecuteAsync<List<AitoeWebResponseActivatedFlag>>(request, restResponse =>
            {
                taskCompletionSource.SetResult(restResponse);
            });

            var restWebResponse = await taskCompletionSource.Task;

            HttpStatusCode statusCode = restWebResponse.StatusCode;

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    {
                        var activatedFlag = restWebResponse.Data[0].status;
                        if (activatedFlag == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                case HttpStatusCode.Unauthorized:
                    {
                        // The following is a hack.
                        return false;
                    }
                default:
                    {
                        var message = "Request to " + request.Resource + " failed." + Environment.NewLine;
                        message = message + "In appropriate response." + Environment.NewLine;
                        message = message + "url called license-api/getActivatedFlag" + Environment.NewLine;
                        message = message + "with paramerter customerEmail - authDetails.orderId" + Environment.NewLine;
                        throw new AitoeWebRequestException(message, AitoeErrorCodes.InAppropriateNumberOfCamsRecieved);
                    }
            }
            
        }

        public async Task<bool?> UnsetActivatedFlag(IAuthDetails authDetails)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("http://www.aitoeva.com/");
            IRestRequest request = new RestRequest("/license-api/unsetActivatedFlag", Method.POST);

            request.AddParameter("orderID", authDetails.OrderId);

            var taskCompletionSource = new TaskCompletionSource<IRestResponse<List<AitoeWebResponseSetUnActivatedFlag>>>();

            client.ExecuteAsync<List<AitoeWebResponseSetUnActivatedFlag>>(request, restResponse =>
            {
                taskCompletionSource.SetResult(restResponse);
            });

            var restWebResponse = await taskCompletionSource.Task;

            HttpStatusCode statusCode = restWebResponse.StatusCode;

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    {
                        var activatedFlag = restWebResponse.Data[0].status;
                        if (activatedFlag == 0)
                            return false;
                        else
                        {
                            _AuthDetails.UserId = string.Empty;
                            _AuthDetails.OrderId = string.Empty;
                            _AuthDetails.Password = string.Empty;
                            PersistAuthDetails(_AuthDetails);
                            return true;
                        }
                    }
                default:
                    {
                        var message = "Request to " + request.Resource + " failed." + Environment.NewLine;
                        message = message + "In appropriate response." + Environment.NewLine;
                        message = message + "url called license-api/ussetActivatedFlag" + Environment.NewLine;
                        message = message + "with paramerter authDetails.orderId" + Environment.NewLine;
                        throw new AitoeWebRequestException(message, AitoeErrorCodes.InAppropriateNumberOfCamsRecieved);
                    }
            }
        }

        public async Task<bool?> SetActivatedFlag(IAuthDetails authDetails)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("http://www.aitoeva.com/");
            // The following is correct.
            //IRestRequest request = new RestRequest("/license-api/setActivatedFlag", Method.POST);
            // The following is incorrect.
            IRestRequest request = new RestRequest("/license-api/setActivatedFlag11111", Method.POST);

            request.AddParameter("orderID", authDetails.OrderId);

            var taskCompletionSource = new TaskCompletionSource<IRestResponse<List<AitoeWebResponseSetActivatedFlag>>>();

            client.ExecuteAsync<List<AitoeWebResponseSetActivatedFlag>>(request, restResponse =>
            {
                taskCompletionSource.SetResult(restResponse);
            });

            var restWebResponse = await taskCompletionSource.Task;

            HttpStatusCode statusCode = restWebResponse.StatusCode;

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    {
                        var activatedFlag = restWebResponse.Data[0].status;
                        if (activatedFlag == 0)
                            return false;
                        else
                            return true;
                    }
                case HttpStatusCode.Unauthorized:
                    {
                        // The following is a hack.
                        return true;
                    }
                default:
                    {
                        var message = "Request to " + request.Resource + " failed." + Environment.NewLine;
                        message = message + "In appropriate response." + Environment.NewLine;
                        message = message + "url called license-api/SetActivatedFlag" + Environment.NewLine;
                        message = message + "with paramerter authDetails.orderId" + Environment.NewLine;
                        throw new AitoeWebRequestException(message, AitoeErrorCodes.InAppropriateNumberOfCamsRecieved);
                    }
            }
        }

        public async Task<int?> ExtractNoOfCameras(IAuthDetails authDetails)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("http://www.aitoeva.com/");
            IRestRequest request = new RestRequest("/license-api/getnumberofCamerasByLicense", Method.POST);

            request.AddParameter("orderID", authDetails.OrderId);

            var taskCompletionSource = new TaskCompletionSource<IRestResponse<List<AitoeWebResponseNumberOfCamsPerLicense>>>();

            client.ExecuteAsync<List<AitoeWebResponseNumberOfCamsPerLicense>>(request, restResponse =>
            {
                taskCompletionSource.SetResult(restResponse);
            });

            var restWebResponse = await taskCompletionSource.Task;

            HttpStatusCode statusCode = restWebResponse.StatusCode;

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    {
                        var numberOfCams = restWebResponse.Data[0].numberofcameras;
                        if (numberOfCams > 0)
                        {
                            if (_PersistedLicenseData == null)
                                return numberOfCams;

                            if (_PersistedLicenseData.CamerasLicensed == null)
                                return numberOfCams;

                            _PersistedLicenseData.CamerasLicensed = numberOfCams;
                            return numberOfCams;
                        }
                        else
                        {
                            var message = "Request to " + request.Resource + " failed." + Environment.NewLine;
                            message = message + "In appropriate response." + Environment.NewLine;
                            message = message + "url called license-api/getnumberofByLicense" + Environment.NewLine;
                            message = message + "with paramerter orderId - authDetails.OrderId" + Environment.NewLine;
                            throw new AitoeWebRequestException(message, AitoeErrorCodes.InAppropriateNumberOfCamsRecieved);
                        }
                    }
                case HttpStatusCode.Unauthorized:
                    {
                        if (_PersistedLicenseData != null)
                        {
                            _PersistedLicenseData.CamerasLicensed = 20;
                        }
                        // The following is a hack.
                        return 20;
                    }
                default:
                    {
                        var message = "Request to " + request.Resource + " failed." + Environment.NewLine;
                        message = message + "In appropriate response." + Environment.NewLine;
                        message = message + "url called license-api/getLicenseByCustomerEmailId" + Environment.NewLine;
                        message = message + "with paramerter customerEmail - authDetails.UserId" + Environment.NewLine;
                        throw new AitoeWebRequestException(message, AitoeErrorCodes.InAppropriateNumberOfCamsRecieved);
                    }
            }
        }
    }
}
