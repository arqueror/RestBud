using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;


namespace Restbud
{
    /// <summary>
    /// Provides methods for GET,POST,PUT,DELETE operations
    /// </summary>
    public class BaseApiClient

    {
        protected string _endpoint;

        private string _userName { get; set; }
        private string _password { get; set; }

        private dynamic _bearerModel =null;
        public string _bearerToken { get; private set; }
        /// <summary>
        /// Specifies bearer token type. This will be used later by GetBearerToken method when called internally to save a temp variable with token information for later calls
        /// </summary>
        /// <param name="type">BearerToken type</param>
        public void SetTokenResponseType(Type type)
        {
            _bearerModel = Convert.ChangeType(_bearerModel, type);
        }
        /// <summary>
        /// Sets default service endpoint for current request.
        /// </summary>
        /// <param name="_apiUrl"></param>
        /// <param name="bearerToken">bearer token</param>
        /// <param name="user">username: Use if you dont have bearerToken yet</param>
        /// <param name="pwd">password:  Use if you dont have bearerToken yet</param>
        public void SetRequestParameters(string _apiUrl, string bearerToken="", string user="", string pwd="")
        {

            _userName = user;
            _bearerToken = bearerToken;
            _password = pwd;
            _endpoint = _apiUrl;

        }
        public BaseApiClient()
        {
            //Initialize
        }
        /// <summary>
        /// Performs a GET operation passing specified resource id
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="id">resource id</param>
        /// <returns>object</returns>
        public T Get<T>(string id)
        {
            using (HttpClient httpClient =NewHttpClient().Result)
            {
                HttpResponseMessage response;
                response = httpClient.GetAsync(String.Concat(_endpoint, "/", id)).Result;
                var data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(data);
            }
        }
        /// <summary>
        /// Performs an async GET operation passing specified resource id
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="id">resource id</param>
        /// <returns>object</returns>
        public async Task<T> GetAsync<T>(string id)
        {
            using (HttpClient httpClient = await NewHttpClient())
            {
                HttpResponseMessage response;
                response =await httpClient.GetAsync(String.Concat(_endpoint, "/", id));

                var data = await response.Content.ReadAsStringAsync();
                //return data as T;
                return JsonConvert.DeserializeObject<T>(data);
            }
        }
        /// <summary>
        ///  Performs a GET operation passing a list of parameters
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="parms">list of params</param>
        /// <returns>object</returns>
        public T Get<T>(NameValueCollection parms)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                HttpResponseMessage response = httpClient.GetAsync(String.Concat(_endpoint, ToQueryString(parms))).Result;
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }
        /// <summary>
        ///  Performs an async GET operation passing a list of parameters
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="parms">list of params</param>
        /// <returns>object</returns>
        public async Task<T> GetAsync<T>(NameValueCollection parms)
        {
            using (HttpClient httpClient =await NewHttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(String.Concat(_endpoint, ToQueryString(parms)));
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(data);
            }
        }
        /// <summary>
        ///  Performs a POST operation passing an object
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="data">object to send in request</param>
        /// <returns>true if request succeeded</returns>
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
        /// <summary>
        ///  Performs an async POST operation passing an object
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="data">object to send in request</param>
        /// <returns>true if request succeeded</returns>
        public async Task<bool> PostAsync<T>(T data)
        {
            using (HttpClient httpClient = await NewHttpClient())
            {
                String obj = JsonConvert.SerializeObject(data);
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await httpClient.PostAsync(_endpoint, requestMessage.Content);
                return result.IsSuccessStatusCode;
            }
        }
        /// <summary>
        ///  Performs a POST operation passing an object
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="data">object to send in request</param>
        /// <returns>object</returns>
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
        /// <summary>
        ///  Performs an async POST operation passing an object
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="data">object to send in request</param>
        /// <returns>object</returns>
        public async Task<T> PostAsync<T>(object data) where T : class
        {
            using (HttpClient httpClient = await NewHttpClient())
            {
                String obj = JsonConvert.SerializeObject(data);
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(_endpoint, requestMessage.Content);
                var returnedObject = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<T>(returnedObject);
                return parsedResponse;
            }
        }
        /// <summary>
        /// Performs a PUT operation passing in resource id and object
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="id">resource id</param>
        /// <param name="data">object containing updated resource data</param>
        /// <returns>object</returns>
        public T Put<T>(string id, T data)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Put;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PutAsync(String.Concat(_endpoint, "/", id), requestMessage.Content).Result;
                var returnedObject = response.Content.ReadAsStringAsync().Result;
                var parsedResponse = JsonConvert.DeserializeObject<T>(returnedObject);
                return parsedResponse;
            }
        }
        /// <summary>
        /// Performs an async PUT operation passing in resource id and object
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="id">resource id</param>
        /// <param name="data">object containing updated resource data</param>
        /// <returns>object</returns>
        public async Task<T> PutAsync<T>(string id, T data)
        {
            using (HttpClient httpClient = await NewHttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Put;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await httpClient.PutAsync(String.Concat(_endpoint, "/", id), requestMessage.Content);
                var returnedObject = await result.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<T>(returnedObject);
                return parsedResponse;
            }
        }
        /// <summary>
        /// Performs a PUT operation passing in resource data
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="data">object containing updated resource data</param>
        /// <returns>object</returns>
        public T Put<T>(T data)
        {
            using (HttpClient httpClient = NewHttpClient().Result)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Put;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8,
                    "application/json");
                HttpResponseMessage result = httpClient.PutAsync(_endpoint, requestMessage.Content).Result;
                var returnedObject =  result.Content.ReadAsStringAsync().Result;
                var parsedResponse = JsonConvert.DeserializeObject<T>(returnedObject);
                return parsedResponse;
            }
        }
        /// <summary>
        /// Performs an async PUT operation passing in resource data
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="data">object containing updated resource data</param>
        /// <returns>object</returns>
        public async Task<T> PutAsync<T>(T data)
        {
            using (HttpClient httpClient =await NewHttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Put;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await httpClient.PutAsync(_endpoint, requestMessage.Content);
                var returnedObject =await result.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<T>(returnedObject);
                return parsedResponse;
            }
        }
        /// <summary>
        /// Performs a DELETE operation passing resource id
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="id">resource id</param>
        /// <returns>object</returns>
        public T Delete<T>(string id)
        {
            using (var httpClient = NewHttpClient().Result)
            {
                HttpResponseMessage result = httpClient.DeleteAsync(String.Concat(_endpoint, "/", id)).Result;
                var returnedObject =  result.Content.ReadAsStringAsync().Result;
                var parsedResponse = JsonConvert.DeserializeObject<T>(returnedObject);
                return parsedResponse;
            }
        }
        /// <summary>
        /// Performs an async DELETE operation passing resource id
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="id">resource id</param>
        /// <returns>object</returns>
        public async Task<T> DeleteAsync<T>(string id)
        {
            using (HttpClient httpClient = await NewHttpClient())
            {
                HttpResponseMessage result = await httpClient.DeleteAsync(String.Concat(_endpoint, "/", id));
                var returnedObject = await result.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<T>(returnedObject);
                return parsedResponse;
            }
        }
        protected async Task<HttpClient> NewHttpClient()
        {
            if (_bearerModel == null&&!string.IsNullOrEmpty(_userName)&&!string.IsNullOrEmpty(_password))
                _bearerModel = await GetBearerToken<dynamic>(_endpoint, _userName, _password).ConfigureAwait(continueOnCapturedContext: false);
            if (_bearerModel == null) return new HttpClient();//no credentials provided, use a default client for Anonymous auth
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_endpoint);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            return client;
        }

        protected string ToQueryString(NameValueCollection nvc)
        {
            var queryString = "?" + string.Join("&", Array.ConvertAll(nvc.AllKeys, key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(nvc[key]))));
            return queryString;
        }
        /// <summary>
        /// Gets bearer token for specified service using username and password
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="endpointUrl">endpoint url</param>
        /// <param name="Username">username</param>
        /// <param name="Password">password</param>
        /// <param name="tokenAction">Optional: default route for bearer token is /token</param>
        /// <returns></returns>
        public async Task<T> GetBearerToken<T>(string endpointUrl, string Username, string Password,string tokenAction="token") where T:class
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(endpointUrl);
            client.DefaultRequestHeaders.Accept.Clear();

            HttpContent requestContent = new StringContent("grant_type=password&username=" + Username + "&password=" + Password, Encoding.UTF8, "application/x-www-form-urlencoded");
            var responseMessage = await client.PostAsync(tokenAction, requestContent).ConfigureAwait(continueOnCapturedContext: false);
            if (responseMessage.IsSuccessStatusCode)
            {
                string jsonMessage;
                using (Stream responseStream = await responseMessage.Content.ReadAsStreamAsync())
                {
                    jsonMessage = new StreamReader(responseStream).ReadToEnd();
                }

                T tokenResponse = (T)JsonConvert.DeserializeObject(jsonMessage, typeof(T));
                return tokenResponse;
            }
            else
            {
                return null;
            }
        }


    }

}
