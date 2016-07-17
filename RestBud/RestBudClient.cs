using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using RestBud.Models;
using static RestBud.Models.RestBudVariables;
//RICARDO VASQUEZ SIERRA 2016
//YOU CAN USE THIS LIBRARY FOR COMMERCIAL USE, JUST GIVE ME SOME CREDIT
namespace RestBud
{
     public class RestBudClient
    {
        private string _endpoint;

         public void SetRestBudConfig(string baseUrl , string apiUserName = "", string apiUsrPwd = "",int apiVersion=-1)
         {
            BaseApiUrl = baseUrl;
            UserName = apiUserName != string.Empty ? apiUserName : string.Empty;
            Password = apiUsrPwd != string.Empty ? apiUsrPwd : string.Empty;
            ApiVersion = apiVersion != -1 ? apiVersion : -1;
           

         }
        public void SetRequestParameters(string controller, string action,BearerModel token)
        {
            TokenModel = token;
            _endpoint = String.Concat(BaseApiUrl, ApiVersion != -1 ? "v" + ApiVersion+'/' : string.Empty, controller, "/", action);

        }
        public void SetRequestParameters(string controller, string action)
        {
            TokenModel = null;
            _endpoint = String.Concat(BaseApiUrl, ApiVersion!=-1?"v"+ApiVersion+'/':string.Empty , controller, "/", action);

        }
        public RestBudClient()
        {
            //Initialize
        }
        public T Get<T>(string id)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                HttpResponseMessage response;
                response = httpClient.GetAsync(String.Concat(_endpoint, "/", id)).Result;
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }
        public async Task<T> GetAsync<T>(string id)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                HttpResponseMessage response;
                response = httpClient.GetAsync(String.Concat(_endpoint, "/", id)).Result;

                var data = await response.Content.ReadAsStringAsync();
                //return data as T;
                return JsonConvert.DeserializeObject<T>(data);
            }
        }
        public async Task<T> GetAsync<T>(int top = 0, int skip = 0)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                string endpoint = _endpoint + "?";
                List<string> parameters = new List<string>();
                if (top > 0) parameters.Add(string.Concat("$top=", top));
                if (skip > 0) parameters.Add(string.Concat("$skip=", skip));
                endpoint += string.Join("&", parameters);
                HttpResponseMessage response = await httpClient.GetAsync(endpoint);
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(data);
            }
        }
        public T Get<T>(int top = 0, int skip = 0)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                string endpoint = _endpoint + "?";
                List<string> parameters = new List<string>();
                if (top > 0) parameters.Add(string.Concat("$top=", top));
                if (skip > 0) parameters.Add(string.Concat("$skip=", skip));
                endpoint += string.Join("&", parameters);
                HttpResponseMessage response = httpClient.GetAsync(endpoint).Result;
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }
        public T Get<T>(NameValueCollection parms)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                HttpResponseMessage response = httpClient.GetAsync(String.Concat(_endpoint, ToQueryString(parms))).Result;
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }
        public async Task<T> GetAsync<T>(NameValueCollection parms)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                HttpResponseMessage response =await httpClient.GetAsync(String.Concat(_endpoint, ToQueryString(parms)));
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(data);
            }
        }
        //Just returns if was success or not
        public bool Post<T>(T data)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                String obj = JsonConvert.SerializeObject(data);
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage result = httpClient.PostAsync(_endpoint, requestMessage.Content).Result;
                return result.IsSuccessStatusCode;
            }
        }
        public async Task<bool> PostAsync<T>(T data)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                String obj = JsonConvert.SerializeObject(data);
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage result =await httpClient.PostAsync(_endpoint, requestMessage.Content);
                return result.IsSuccessStatusCode;
            }
        }
        //Parses response into custom object type
        public T Post<T>(object data) where T : class
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                String obj = JsonConvert.SerializeObject(data);
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PostAsync(_endpoint, requestMessage.Content).Result;
                var parsedResponse = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                return parsedResponse;
            }
        }
        public async Task<T> PostAsync<T>(object data) where T : class
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                String obj = JsonConvert.SerializeObject(data);
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response =await httpClient.PostAsync(_endpoint, requestMessage.Content);
                var returnedObject = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<T>(returnedObject);
                return parsedResponse;
            }
        }
        public string Put<T>(string id, T data)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Put;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage result = httpClient.PutAsync(String.Concat(_endpoint, "/", id), requestMessage.Content).Result;
                return result.Content.ReadAsStringAsync().Result;
            }
        }
        public async Task<string> PutAsync<T>(string id, T data)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Put;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage result =await httpClient.PutAsync(String.Concat(_endpoint, "/", id), requestMessage.Content);
                var returnedObject = await result.Content.ReadAsStringAsync();
                return returnedObject;
            }
        }

        public string Put<T>(T data)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Put;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8,
                    "application/json");
                HttpResponseMessage result = httpClient.PutAsync(_endpoint, requestMessage.Content).Result;
                return result.Content.ReadAsStringAsync().Result;
            }
        }
        public async Task<string> PutAsync<T>(T data)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Put;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8,"application/json");
                HttpResponseMessage result =await httpClient.PutAsync(_endpoint, requestMessage.Content);
                var returnedObject = await result.Content.ReadAsStringAsync();
                return returnedObject;
            }
        }
        public string Delete(string id)
        {
            using (var httpClient = NewHttpClient().Result)
            {
                HttpResponseMessage result = httpClient.DeleteAsync(String.Concat(_endpoint, "/", id)).Result;
                return result.Content.ToString();
            }
        }
        public async Task<string> DeleteAsync(string id)
        {
            using (var httpClient = NewHttpClient().Result)
            {
                HttpResponseMessage result =await httpClient.DeleteAsync(String.Concat(_endpoint, "/", id));
                return result.Content.ToString();
            }
        }
        protected async Task<HttpClient> NewHttpClient()
        {
            if (TokenModel == null) return new HttpClient();//Client without authentication
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenModel.AccessToken);
            return client;
        }

        protected string ToQueryString(NameValueCollection nvc)
        {
           
            var queryString = "?" + string.Join("&", Array.ConvertAll(nvc.AllKeys, key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(nvc[key]))));
            return queryString;
        }
       public async Task<T> GetBearerToken<T>(string apiUrl,string tokenActionProvider, string Username, string Password) where T:class
       {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();

            HttpContent requestContent = new StringContent("grant_type=password&username=" + Username + "&password=" + Password, Encoding.UTF8, "application/x-www-form-urlencoded");
            var responseMessage = await client.PostAsync(tokenActionProvider, requestContent).ConfigureAwait(continueOnCapturedContext: false);
            if (responseMessage.IsSuccessStatusCode)
            {
                string jsonMessage;
                using (Stream responseStream = await responseMessage.Content.ReadAsStreamAsync())
                {
                    jsonMessage = new StreamReader(responseStream).ReadToEnd();
                }
                var tokenResponse = JsonConvert.DeserializeObject<T>(jsonMessage);
                return tokenResponse;
                
            }
           var returnedObject = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(returnedObject);
        }
    }
}
