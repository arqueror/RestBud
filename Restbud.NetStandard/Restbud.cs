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
using Xamarin.Forms;

namespace Restbud.NetStandard
{
    /// <summary>
    /// Provides methods for GET,POST,PUT,DELETE operations
    /// </summary>
    public class Restbud

    {
        protected string _baseUrl;
        private string _requestMediaType { get; set; }
        //private string _userName { get; set; }
        //private string _password { get; set; }

        //private dynamic _bearerModel =null;
        public string BearerToken { get; private set; }

        public bool StoreCredentialsLocally { get; set; }

        /// <summary>
        /// Sets default service endpoint for current request.
        /// </summary>
        /// <param name="_apiUrl">Base REST service URL</param>
        /// <param name="bearerToken">bearer token</param>
        /// <param name="user">username: Use if you dont have bearerToken yet</param>
        /// <param name="pwd">password:  Use if you dont have bearerToken yet</param>
        public void SetRequestParameters(string _apiUrl, string bearerToken = "",string requestsMediaType= "application/json")
        {

            //_userName = user;
            BearerToken = bearerToken;
            //_password = pwd;
            _baseUrl = _apiUrl;
            _requestMediaType = requestsMediaType;

        }
        public Restbud()
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
            using (HttpClient httpClient = NewHttpClient())
            {
                HttpResponseMessage response;
                response = httpClient.GetAsync(String.Concat(_baseUrl, "/", id)).Result;
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
        public async Task<Response<T, HttpResponseMessage>> GetAsync<T>(string id) where T : class
        {
            using (HttpClient httpClient = NewHttpClient())
            {
                var rs = new Response<T, HttpResponseMessage>();
                HttpResponseMessage response;
                response = await httpClient.GetAsync(String.Concat(_baseUrl, "/", id));

                rs.HttpResponseMessage = response;
                if (response.IsSuccessStatusCode)
                {
                    string jsonMessage;
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        jsonMessage = new StreamReader(responseStream).ReadToEnd();
                    }

                    T tokenResponse = (T)JsonConvert.DeserializeObject(jsonMessage, typeof(T));
                    rs.Content = tokenResponse;

                }
                return rs;
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
            using (HttpClient httpClient = NewHttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(String.Concat(_baseUrl, ToQueryString(parms))).Result;
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }
        /// <summary>
        ///  Performs an async GET operation passing a list of parameters
        /// </summary>
        /// <typeparam name="T">response type</typeparam>
        /// <param name="parms">list of params</param>
        /// <returns>object</returns>
        public async Task<Response<T, HttpResponseMessage>> GetAsync<T>(NameValueCollection parms) where T : class
        {
            using (HttpClient httpClient = NewHttpClient())
            {
                var rs = new Response<T, HttpResponseMessage>();
                HttpResponseMessage response = await httpClient.GetAsync(String.Concat(_baseUrl, ToQueryString(parms)));

                rs.HttpResponseMessage = response;
                if (response.IsSuccessStatusCode)
                {
                    string jsonMessage;
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        jsonMessage = new StreamReader(responseStream).ReadToEnd();
                    }

                    T tokenResponse = (T)JsonConvert.DeserializeObject(jsonMessage, typeof(T));
                    rs.Content = tokenResponse;

                }
                return rs;
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
            using (HttpClient httpClient = NewHttpClient())
            {
                String obj = JsonConvert.SerializeObject(data);
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage result = httpClient.PostAsync(_baseUrl, requestMessage.Content).Result;
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
            using (HttpClient httpClient = NewHttpClient())
            {
                String obj = JsonConvert.SerializeObject(data);
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await httpClient.PostAsync(_baseUrl, requestMessage.Content);
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
            using (HttpClient httpClient = NewHttpClient())
            {
                String obj = JsonConvert.SerializeObject(data);
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PostAsync(_baseUrl, requestMessage.Content).Result;
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
        public async Task<Response<T, HttpResponseMessage>> PostAsync<T>(object data) where T : class
        {
            using (HttpClient httpClient = NewHttpClient())
            {
                var rs = new Response<T, HttpResponseMessage>();
                String obj = JsonConvert.SerializeObject(data);
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(_baseUrl, requestMessage.Content);
                rs.HttpResponseMessage = response;
                if (response.IsSuccessStatusCode)
                {
                    string jsonMessage;
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        jsonMessage = new StreamReader(responseStream).ReadToEnd();
                    }

                    T tokenResponse = (T)JsonConvert.DeserializeObject(jsonMessage, typeof(T));
                    rs.Content = tokenResponse;

                }
                return rs;
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
            using (HttpClient httpClient = NewHttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Put;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PutAsync(String.Concat(_baseUrl, "/", id), requestMessage.Content).Result;
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
        public async Task<Response<T, HttpResponseMessage>> PutAsync<T>(string id, T data) where T : class
        {
            using (HttpClient httpClient = NewHttpClient())
            {
                var rs = new Response<T, HttpResponseMessage>();
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Put;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(String.Concat(_baseUrl, "/", id), requestMessage.Content);
                rs.HttpResponseMessage = response;
                if (response.IsSuccessStatusCode)
                {
                    string jsonMessage;
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        jsonMessage = new StreamReader(responseStream).ReadToEnd();
                    }

                    T tokenResponse = (T)JsonConvert.DeserializeObject(jsonMessage, typeof(T));
                    rs.Content = tokenResponse;

                }
                return rs;
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
            using (HttpClient httpClient = NewHttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Put;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8,
                    "application/json");
                HttpResponseMessage result = httpClient.PutAsync(_baseUrl, requestMessage.Content).Result;
                var returnedObject = result.Content.ReadAsStringAsync().Result;
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
        public async Task<Response<T, HttpResponseMessage>> PutAsync<T>(T data) where T : class
        {
            using (HttpClient httpClient = NewHttpClient())
            {
                var rs = new Response<T, HttpResponseMessage>();
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Put;
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(_baseUrl, requestMessage.Content);
                rs.HttpResponseMessage = response;
                if (response.IsSuccessStatusCode)
                {
                    string jsonMessage;
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        jsonMessage = new StreamReader(responseStream).ReadToEnd();
                    }

                    T tokenResponse = (T)JsonConvert.DeserializeObject(jsonMessage, typeof(T));
                    rs.Content = tokenResponse;

                }
                return rs;
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
            using (var httpClient = NewHttpClient())
            {
                HttpResponseMessage result = httpClient.DeleteAsync(String.Concat(_baseUrl, "/", id)).Result;
                var returnedObject = result.Content.ReadAsStringAsync().Result;
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
        public async Task<Response<T, HttpResponseMessage>> DeleteAsync<T>(string id) where T : class
        {
            using (HttpClient httpClient = NewHttpClient())
            {
                var rs = new Response<T, HttpResponseMessage>();
                HttpResponseMessage response = await httpClient.DeleteAsync(String.Concat(_baseUrl, "/", id));
                rs.HttpResponseMessage = response;
                if (response.IsSuccessStatusCode)
                {
                    string jsonMessage;
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        jsonMessage = new StreamReader(responseStream).ReadToEnd();
                    }

                    T tokenResponse = (T)JsonConvert.DeserializeObject(jsonMessage, typeof(T));
                    rs.Content = tokenResponse;

                }
                return rs;
            }
        }
        protected HttpClient NewHttpClient()
        {
            //if (_bearerModel == null&&!string.IsNullOrEmpty(_userName)&&!string.IsNullOrEmpty(_password))
            //    _bearerModel = await RequestBearerToken<dynamic>(_endpoint, _userName, _password).ConfigureAwait(continueOnCapturedContext: false);
            //if (_bearerModel == null) return new HttpClient();//no credentials provided, use a default client for Anonymous auth
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(BearerToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
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
        /// <param name="requestContent">Content for this request</param>
        /// <returns></returns>
        public async Task<Response<T,HttpResponseMessage>> RequestBearerToken<T>(string endpoint,HttpContent requestContent) where T : class
        {
            var rs = new Response<T, HttpResponseMessage>();
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(endpointUrl);
            client.DefaultRequestHeaders.Accept.Clear();

            //HttpContent requestContent = new StringContent("grant_type=password&username=" + Username + "&password=" + Password, Encoding.UTF8, "application/x-www-form-urlencoded");
            var responseMessage = await client.PostAsync(endpoint, requestContent).ConfigureAwait(continueOnCapturedContext: false);
            rs.HttpResponseMessage = responseMessage;
            if (responseMessage.IsSuccessStatusCode)
            {
                string jsonMessage;
                using (Stream responseStream = await responseMessage.Content.ReadAsStreamAsync())
                {
                    jsonMessage = new StreamReader(responseStream).ReadToEnd();
                }

                T tokenResponse = (T)JsonConvert.DeserializeObject(jsonMessage, typeof(T));
                rs.Content = tokenResponse;
               
            }
            return rs;
        }
        //protected void RetrieveCredentials()
        //{
        //    if (Application.Current.Properties.ContainsKey("restbud_username"))
        //    {
        //        var id = Application.Current.Properties["id"] as int;
        //    }
        //    if (Application.Current.Properties.ContainsKey("restbud_password"))
        //    {
        //        var id = Application.Current.Properties["id"] as int;
        //    }
        //    if (Application.Current.Properties.ContainsKey("restbud_bearer"))
        //    {
        //        var id = Application.Current.Properties["id"] as int;
        //    }
        //}
        //protected void StoreCredentials()
        //{
        //    if (!StoreCredentialsLocally) return;
        //    if (!string.IsNullOrEmpty(_userName))
        //    {
        //        Application.Current.Properties["restbud_username"] = _userName;
        //    }
        //    if (!string.IsNullOrEmpty(_password))
        //    {
        //        Application.Current.Properties["restbud_password"] = _password;
        //    }
        //    if (!string.IsNullOrEmpty(BearerToken))
        //    {
        //        Application.Current.Properties["restbud_bearer"] = BearerToken;
        //    }
        //    Application.Current.SavePropertiesAsync();
        //}

    }
}
