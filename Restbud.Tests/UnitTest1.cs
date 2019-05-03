using System;
using System.Net.Http;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Restbud.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //NetStandard.Restbud rb=new NetStandard.Restbud();
            //rb.SetRequestParameters("http://localhost:51122/api/v1/Token/generate");
            //HttpContent requestContent = new StringContent("grant_type=password&username=someuser&password=somepwd", Encoding.UTF8, "application/x-www-form-urlencoded");
            //System.Threading.Tasks.Task.Run(async () =>
            //{
            //    var response = await rb.RequestBearerToken<UserDTO>(requestContent);
            //});
           
        }
    }


    public class UserDTO
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public long UserId { get; set; }

        public string Token { get; set; }
    }
}
