using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        private string requestBaseAddress = ConfigurationManager.AppSettings["RequestBaseAddress"];
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

        [HttpPost]
        public ActionResult RequestHelper(FormCollection formData)
        {
            var parameters = new Dictionary<string, string>();
            for (int i = 0; i < formData.Keys.Count; i++)
            {
                parameters.Add(formData.Keys[i], formData[formData.Keys[i]]);
            }

            string token = HttpContext.Cache.Get("accessToken").ToString();
            HtpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseValue = HtpClient.PostAsync(requestBaseAddress + formData["ReuestUrl"], new FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync().Result;

            return Json(responseValue, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RequestHelper()
        {
            string reuestUrl = HttpContext.Request["ReuestUrl"];
            string token = HttpContext.Cache.Get("accessToken").ToString();
            HtpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseValue = HtpClient.GetAsync(requestBaseAddress + reuestUrl).Result.Content.ReadAsStringAsync().Result;

            return Json(responseValue, JsonRequestBehavior.AllowGet);
        }
    }
}