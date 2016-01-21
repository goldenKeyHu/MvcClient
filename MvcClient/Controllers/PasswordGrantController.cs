using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcClient.Controllers
{
    public class PasswordGrantController : Controller
    {
        // GET: PasswordGrantTest
        public ActionResult Index()
        {
            return View();
        }

        private HttpClient _httpClient;
        public HttpClient HtpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                    //_httpClient.BaseAddress = new Uri("http://openapi.cnblogs.com");
                }
                return _httpClient;
            }
        }

        public bool GetAccessToken()
        {
            var clientId = "e8eaf916-291d-44d1-aa65-dc501ad5cb3f";
            var clientSecret = "123456";
            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "password");
            parameters.Add("username", "goldenkey");
            parameters.Add("password", "123");

            HtpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret)));

            string responseValue = HtpClient.PostAsync("http://localhost:10722/token", new FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync().Result;
            string acessToken = JObject.Parse(responseValue)["access_token"].Value<string>();
            string refreshToken = JObject.Parse(responseValue)["refresh_token"].Value<string>();    
            
            //注：生产环境下，WebClient保存RefreshToken是没有意义的，在此是为了做一个refreshToken 的测试    
            HttpContext.Cache.Insert("accessToken", acessToken);
            HttpContext.Cache.Insert("refreshToken", refreshToken);
            
            return true;
        }

        public string RefreshAccessToken()
        {
            var clientId = "e8eaf916-291d-44d1-aa65-dc501ad5cb3f";
            var clientSecret = "123456";
            var refreshTokenOld = HttpContext.Cache.Get("refreshToken").ToString();

            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "refresh_token");
            parameters.Add("refresh_token", refreshTokenOld);

            HtpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret)));

            string responseValue = HtpClient.PostAsync("http://localhost:10722/token", new FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync().Result;
            string acessToken = JObject.Parse(responseValue)["access_token"].Value<string>();
            string refreshTokenNew = JObject.Parse(responseValue)["refresh_token"].Value<string>();

            //注：生产环境下，WebClient保存RefreshToken是没有意义的，在此是为了做一个refreshToken 的测试    
            HttpContext.Cache.Insert("accessToken", acessToken);
            HttpContext.Cache.Insert("refreshToken", refreshTokenNew);

            return responseValue;
        }

        public string GetResource()
        {
            string token = HttpContext.Cache.Get("accessToken").ToString();
            HtpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseValue = HtpClient.GetAsync("http://localhost:10722/UserAbout/GetCurrent").Result.Content.ReadAsStringAsync().Result;
            return responseValue;
        }
    }
}