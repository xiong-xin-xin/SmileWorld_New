using BLL;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmileWorld.Controllers
{
    public class HomeController : Controller
    {
        string qq_base_url = "https://graph.qq.com";
        string client_id = "101823433";
        string appkey = "ecb2d9a91bba8becabef6dd576659570";

        private readonly IUserInfoBLL _userInfoBLL;
        public HomeController(IUserInfoBLL userInfoBLL)
        {
            _userInfoBLL = userInfoBLL;
        }

        public async Task<IActionResult> Index()
        {
            await _userInfoBLL.GetUserInfoAsync();
            return View();
        }
        [Route("qq")]
        public async Task<IActionResult> QQ(string code)
        {
            var a = Request.Query;
            //获取access_token
            //string access_token = await $"{qq_base_url}/oauth2.0/token?grant_type=authorization_code&client_id={client_id}&client_secret={appkey}&code={code}&redirect_uri=http://smileworld.tech/qq".GetAsync().ReceiveString();
            string res = "access_token=91B14BE7B2498F00BB9F074DB1853CC1&expires_in=7776000&refresh_token=891C9DA28A0D8C137D923B6455E379BD";
            NameValueCollection nameValue = new NameValueCollection();
            res.Split("&").ToList().ForEach(t => nameValue.Add(t.Split("=").First()?.ToString(), t.Split("=").Last()?.ToString()));
            //获取open_id
            string str = await $"{qq_base_url}/oauth2.0/me?access_token={nameValue["access_token"]}".GetAsync().ReceiveString();
            string json = Regex.Replace(str, @"(.*\()(.*)(\).*)", "$2");
            string open_id = JObject.Parse(json)["openid"].ToString();
            //数据库不存在此openid
            if (true)
            {         //获取用户信息
                var userInfo = await $"{qq_base_url}/user/get_user_info?access_token={nameValue["access_token"]}&oauth_consumer_key={client_id}&openid={open_id}".GetAsync().ReceiveJson<QQUserInfo>();
            }


            return View("Index");
        }
        public class QQUserInfo
        {
            public int ret { get; set; }
            public string msg { get; set; }
            public int is_lost { get; set; }
            public string nickname { get; set; }
            public string gender { get; set; }
            public int gender_type { get; set; }
            public string province { get; set; }
            public string city { get; set; }
            public string year { get; set; }
            public string constellation { get; set; }
            public string figureurl { get; set; }
            public string figureurl_1 { get; set; }
            public string figureurl_2 { get; set; }
            public string figureurl_qq_1 { get; set; }
            public string figureurl_qq_2 { get; set; }
            public string figureurl_qq { get; set; }
            public string figureurl_type { get; set; }
            public string is_yellow_vip { get; set; }
            public string vip { get; set; }
            public string yellow_vip_level { get; set; }
            public string level { get; set; }
            public string is_yellow_year_vip { get; set; }
        }

    }
}