using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;

namespace Screening
{
    public static class Captch
    {
        public static bool ValidateCaptcha(string response)
        {
            //secret that was generated in key value pair  
            string secret = WebConfigurationManager.AppSettings["RecaptchPrivateKey"];

            using (var client = new WebClient())
            {
                var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
                var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);
                return Convert.ToBoolean(captchaResponse.Success);
            }
        }
    }
    public class CaptchaResponse
    {
        [JsonProperty("Success")]
        public bool Success { get; set; }
        [JsonProperty("error-codes")]
        public List<string> ErrorMessage { get; set; }
    }
}