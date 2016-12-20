using Newtonsoft.Json.Linq;
using SimpleSSOTest.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SimpleSSOTest.Controllers
{
    [RoutePrefix("api/code")]
    public class CodeController : ApiController
    {
        private HttpClient _httpClient;

        private const string _serverUrl = "http://localhost:8550";

        private const string _serverTokenUrl = _serverUrl + "/Token";

        private const string _serverTicketMessageUrl = _serverUrl + "/TicketUser/TicketMessage";

        public CodeController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet]
        [Route("App1")]
        public async Task<string> App1(string code = "")
        {
            return await AppData(code, "App1", "1", "123");
        }

        [HttpGet]
        [Route("AppPassword")]
        public async Task<string> AppPassword()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "password");
            parameters.Add("username", "zfh");
            parameters.Add("password", "123");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes("1" + ":" + "123")));
            var response = await _httpClient.PostAsync(_serverTokenUrl, new FormUrlEncodedContent(parameters));
            var result = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(result);
            var refreshToken = obj["refresh_token"].Value<string>();
            var accessToken = obj["access_token"].Value<string>();
            return $"<font color='black'><b>应用App1获取到用户zfh的</b></font></br>refresh_token:{refreshToken}</br>access_token:{accessToken}";
        }

        [HttpGet]
        [Route("AppclientCredentials")]
        public async Task<string> AppclientCredentials()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "client_credentials");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes("1" + ":" + "123")));
            var response = await _httpClient.PostAsync(_serverTokenUrl, new FormUrlEncodedContent(parameters));
            var result = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(result);
            var refreshToken = obj["refresh_token"].Value<string>();
            var accessToken = obj["access_token"].Value<string>();
            return $"<font color='black'><b>应用App1获取到</b></font></br>refresh_token:{refreshToken}</br>access_token:{accessToken}";
        }

  
        #region

        private async Task<string> AppData(string code,
            string appName, string clientID, string clientSecret)
        {
            StringBuilder strMessage = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(code))
            {
                string accessToken = "";
                string codeResult = await AuthorizationCode(appName, clientID, clientSecret, code);
                var obj = JObject.Parse(codeResult);
                var refreshToken = obj["refresh_token"].Value<string>();
                accessToken = obj["access_token"].Value<string>();
                strMessage.Append($"<font color='black'><b>应用{appName}使用</b></font></br>code:{code}获取到</br>refresh_token:{refreshToken}</br>access_token:{accessToken}");
                if (!string.IsNullOrEmpty(accessToken))
                {
                    strMessage.Append($"</br><font color='black'><b>使用AccessToken获取到信息:</b></font>{ await GetTicketMessageData(accessToken) }");
                    obj = JObject.Parse(await RefreshToken(clientID, clientSecret, refreshToken));
                    refreshToken = obj["refresh_token"].Value<string>();
                    accessToken = obj["access_token"].Value<string>();
                    strMessage.Append($"</br><font color='black'><b>应用{appName}刷新秘钥获取到</b></font></br>refresh_token:{refreshToken}</br>access_token:{accessToken}");
                    strMessage.Append($"</br><font color='black'><b>使用刷新后AccessToken获取到信息:</b></font>{ await GetTicketMessageData(accessToken) }");
                }
            }
            else
            {
                strMessage.AppendLine("获取code失败.");
            }
            return await Task.FromResult(strMessage.ToString());
        }

        /// <summary>
        /// authorization_code示例
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<string> AuthorizationCode(string appName, string clientID, string clientSecret, string code)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "authorization_code");
            parameters.Add("code", code);
            parameters.Add("redirect_uri", Url.Content("~/") + "api/Code/" + appName);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(clientID + ":" + clientSecret)));
            var response = await _httpClient.PostAsync(_serverTokenUrl, new FormUrlEncodedContent(parameters));
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 通过秘钥获取用户信息示例
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<string> GetTicketMessageData(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetAsync(_serverTicketMessageUrl).Result.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 刷新秘钥示例
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="clientSecret"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private async Task<string> RefreshToken(string clientID, string clientSecret, string refreshToken)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "refresh_token");
            parameters.Add("refresh_token", refreshToken);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(clientID + ":" + clientSecret)));
            var response = await _httpClient.PostAsync(_serverTokenUrl, new FormUrlEncodedContent(parameters));
            return await response.Content.ReadAsStringAsync();
        }

        #endregion

    }
}
