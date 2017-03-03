using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.WebApiWrapper.Models;

namespace Project.WebApiWrapper.Contracts
{
        public interface IAuthService
        {
            bool Register(CreateUserBindingModel user);
            UserReturnModel Login(string id, string password);
            TokenResponseModel RegisterExternal(RegisterExternalBindingModel user);
            UserReturnModel GetExternalUserInformation(string userName, string bearerToken);
            TokenResponseModel ObtainExternalToken(RegisterExternalBindingModel authInfo);

        }

}
