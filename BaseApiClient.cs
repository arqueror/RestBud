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

using Project.WebApiWrapper.Models;

namespace Project.WebApiWrapper
{
    public abstract class BaseApiClient

    {
        protected string _endpoint;

        private string _userName { get; set; }
        private string _password { get; set; }

        private dynamic _bearerModel =null;
        public string _bearerToken { get; private set; }

        public void SetTokenResponseModel<T>(T bearerModel) where T:class
        {
            _bearerModel = bearerModel;
        }
        public void SetRequestParameters(string _apiUrl, string user="", string pwd="")
        {

            _userName = user;
            _password = pwd;
            _endpoint = _apiUrl;

        }

        public void SetRequestParameters(string _apiUrl, string bearerToken, string user = "", string pwd = "")
        {
            _userName = user;
            _password = pwd;
            _bearerToken =bearerToken;
            _endpoint = _apiUrl;

        }
        public BaseApiClient()
        {
            //Initialize
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
                HttpResponseMessage response = await httpClient.GetAsync(String.Concat(_endpoint, ToQueryString(parms)));
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
                HttpResponseMessage result = await httpClient.PostAsync(_endpoint, requestMessage.Content);
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
                HttpResponseMessage response = await httpClient.PostAsync(_endpoint, requestMessage.Content);
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
                HttpResponseMessage result = await httpClient.PutAsync(String.Concat(_endpoint, "/", id), requestMessage.Content);
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
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await httpClient.PutAsync(_endpoint, requestMessage.Content);
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
                HttpResponseMessage result = await httpClient.DeleteAsync(String.Concat(_endpoint, "/", id));
                return result.Content.ToString();
            }
        }
        protected async Task<HttpClient> NewHttpClient()
        {
            if (_bearerModel == null&&!string.IsNullOrEmpty(_userName)&&!string.IsNullOrEmpty(_password))
                _bearerModel = await GetBearerToken<TokenResponseModel>(_endpoint, _userName, _password).ConfigureAwait(continueOnCapturedContext: false);
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
        public async Task<T> GetBearerToken<T>(string siteUrl, string Username, string Password,string tokenAction="token") where T:class
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(siteUrl);
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
