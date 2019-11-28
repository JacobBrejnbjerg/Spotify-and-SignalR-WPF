using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SpotifyAPI.Lib
{
    public class HttpCaller
    {
        private readonly HttpClient _httpClient;

        public HttpCaller()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        public HttpCaller(string baseUrl)
        {
            // Creates HttpClient with a base URL
            _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(5),
                BaseAddress = new Uri(baseUrl)
            };
        }

        public async Task<T> Get<T>(string url)
        {
            string jsonResult = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(jsonResult);
        }

        public async Task<T> Get<T>(string url, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string jsonResult = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(jsonResult);
        }


        public async Task<HttpStatusCode> Post(string url)
        {
            HttpResponseMessage msg = await _httpClient.PostAsync(url, null);
            return msg.StatusCode;
        }

        public async Task<T> Post<T>(string url)
        {
            HttpResponseMessage msg = await _httpClient.PostAsync(url, null);
            string jsonResult = await msg.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonResult);
        }

        public async Task<HttpStatusCode> Post(string url, string token, HttpContent content = null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage msg = await _httpClient.PostAsync(url, content);
            return msg.StatusCode;
        }


        public async Task<HttpStatusCode> Put(string url)
        {
            HttpResponseMessage msg = await _httpClient.PutAsync(url, null);
            return msg.StatusCode;
        }

        public async Task<T> Put<T>(string url)
        {
            HttpResponseMessage msg = await _httpClient.PutAsync(url, null);
            string jsonResult = await msg.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonResult);
        }

        public async Task<HttpStatusCode> Put(string url, string token, HttpContent content = null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage msg = await _httpClient.PutAsync(url, content);
            return msg.StatusCode;
        }

        
        public async Task<HttpStatusCode> Delete(string url)
        {
            HttpResponseMessage msg = await _httpClient.DeleteAsync(url);
            return msg.StatusCode;
        }

        public async Task<HttpStatusCode> Delete(string url, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage msg = await _httpClient.DeleteAsync(url);
            return msg.StatusCode;
        }
    }
}
