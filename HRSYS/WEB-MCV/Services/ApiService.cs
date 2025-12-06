using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace WEB_MCV.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private readonly JwtService _jwt;

        public ApiService(HttpClient client, JwtService jwt)
        {
            _client = client;
            _jwt = jwt;
        }

        private void AttachToken(string? overrideToken = null)
        {
            var token = overrideToken ?? _jwt.Get();

            if (string.IsNullOrWhiteSpace(token))
            {
                _client.DefaultRequestHeaders.Authorization = null;
                return;
            }

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<(bool isSuccess, string json)> GetAsync(string url, string? tokenOverride = null)
        {
            AttachToken(tokenOverride);
            var res = await _client.GetAsync(url);
            var json = await res.Content.ReadAsStringAsync();
            return (res.IsSuccessStatusCode, json);
        }

        public async Task<string> PostAsync(string url, object data, string? tokenOverride = null)
        {
            AttachToken(tokenOverride);

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json"
            );

            var res = await _client.PostAsync(url, jsonContent);
            var json = await res.Content.ReadAsStringAsync();

            return json; 
        }


        public async Task<(bool isSuccess, string json)> PutAsync(string url, object data, string? tokenOverride = null)
        {
            AttachToken(tokenOverride);

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json"
            );

            var res = await _client.PutAsync(url, jsonContent);
            var json = await res.Content.ReadAsStringAsync();
            return (res.IsSuccessStatusCode, json);
        }

        public async Task<(bool isSuccess, string json)> DeleteAsync(string url, string? tokenOverride = null)
        {
            AttachToken(tokenOverride);

            var res = await _client.DeleteAsync(url);
            var json = await res.Content.ReadAsStringAsync();
            return (res.IsSuccessStatusCode, json);
        }
    }
}
