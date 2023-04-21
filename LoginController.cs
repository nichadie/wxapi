using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Cors;

namespace wx.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]/[action]")] // 这种路由方式是以get post作为url
                                                              //[Route("api/action")]// 这种路由是指定开头，具体URL根据方法上的路由
                                                              //[ApiController]
                                                              //[Route("[controller]")]
    public class LoginController
    {
        [HttpPost]
        //[Route("get")]//指定路由
        public object WXLogin(Logindetail login)
        {
            string openid = "";
            if (login.code!=null&&login.code!="")
            {
                string appid = "wxid";
                string appSecret = "secret";


                string url = "https://api.weixin.qq.com/sns/jscode2session";//请求的url
                string sUrlpara = "?appid=" + appid + "&secret=" + appSecret +
                             "&js_code=" + login.code + "&grant_type=authorization_code";//请求的参数
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                request = (HttpWebRequest)WebRequest.Create(url + sUrlpara);
                request.Method = "GET";    //设置为post请求
                request.ReadWriteTimeout = 5000;
                request.KeepAlive = false;
                request.ContentType = "text/html;charset=UTF-8";
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string readernd = reader.ReadToEnd();
                JObject jsonObj = JsonConvert.DeserializeObject(readernd) as JObject;
                string json = jsonObj.ToString();   
                Console.WriteLine(json);
                foreach (var p in jsonObj.Properties().ToArray())
                {
                    Console.WriteLine("key={0}, value={1}", p.Name, jsonObj[p.Name].Value<string>());
                    if (p.Name == "openid")
                    {
                        openid = jsonObj[p.Name].Value<string>();


                    }
                }


                    //}


                }

            return openid;
        }

        public class Logindetail
        {
            /// <summary>
            /// 凭证
            /// </summary>
            public string code { get; set; }
        }
            
    }
}
