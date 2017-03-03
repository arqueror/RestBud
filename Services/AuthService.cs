using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project.WebApiWrapper.Contracts;
using Project.WebApiWrapper.Models;

namespace Project.WebApiWrapper.Services
{

    public class AuthService:BaseApiClient, IAuthService
    {
       
        public bool Register(CreateUserBindingModel user)
        {
            //No need user and pwd, because it's a new user we want to register
            SetRequestParameters("Account", "Register");
            var isSuccess = Post<CreateUserBindingModel>(user);
            return isSuccess;

        }
        
        //If the user already exists in DB, it will get AuthTOken automatically 
        //and embed it to the request
        //The CurrentToken is stored in 
        //private TokenResponse 
        public UserReturnModel Login(string userName, string password)
        {
            NameValueCollection parms = new NameValueCollection() {
                { "user", userName },
                { "password", password }
            };

            SetRequestParameters("Account", "Login", userName, password);
            var userModel = Get<UserReturnModel>(parms);

            return userModel;

        }
        #region ExternalAUth
        public UserReturnModel GetExternalUserInformation(string userName,string bearerToken)
        {
            NameValueCollection parms = new NameValueCollection() {
                { "user", userName }
            };

            SetRequestParameters("Account", "GetExternalUserInformation",bearerToken);
            var userModel = Get<UserReturnModel>(parms);

            return userModel;

        }
        
        public TokenResponseModel RegisterExternal(RegisterExternalBindingModel autInfo)
        {
            //No need user and pwd, because it's a new user we want to register
            SetRequestParameters("Account", "RegisterExternal");
            var tokenInfo= Post<TokenResponseModel>(autInfo);
            return tokenInfo;

        }
        public TokenResponseModel ObtainExternalToken(RegisterExternalBindingModel authInfo)
        {
            NameValueCollection parms = new NameValueCollection() {
                { "provider", authInfo.Provider },
                { "externalAccessToken", authInfo.ExternalAccessToken }
            };

            SetRequestParameters("Account", "ObtainLocalAccessToken");
            var tokenInfo = Get<TokenResponseModel>(parms);
            return tokenInfo;

        }
        #endregion
    }
}
