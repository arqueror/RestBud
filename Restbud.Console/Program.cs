using System;

namespace Restbud.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            NetStandard.Restbud rb = new NetStandard.Restbud();
            rb.SetRequestParameters("http://localhost:51122/api/v1");

            var payload = new UserCredentialsDTO
            {
                Email = "email@dsadas.com",
                Password = "FakePassword"
            };

            System.Net.Http.HttpContent requestContent = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");
            System.Threading.Tasks.Task.Run(async () =>
            {
                var response = await rb.RequestBearerToken<UserDTO>("http://localhost:51122/api/v1/Token/generate",requestContent);
            });
            System.Console.ReadKey();
        }
        
    }

    public class UserDTO
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public long UserId { get; set; }

        public string Token { get; set; }
    }
    public class UserCredentialsDTO
    {
        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }
    }
}
