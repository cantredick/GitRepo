using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebWX
{
    public partial class CorpWX : System.Web.UI.Page
    {

        public static string AccessToken;
        public static string Code;
        public static string Userid;

        public static string flag = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            //获取AccessToken
            AccessToken = IsExistAccess_Token();


            string absUrl = Request.Url.AbsoluteUri;
            if (absUrl.Contains("code"))//判断是否重定向获取到code值
            {
                Code = Request["code"];
            }

            Userid = GetUser_info().UserId.ToString();

            //flag = Code2Xml(Code);
            

            
        }



        /// <summary>
        /// 通过corpid和appsecret获取Access_token
        /// </summary>
        /// <returns></returns>
        private static Access_token GetAccess_token()
        {
            string corpid = "wxa5626fae9ab7ef5b";
            string secret = "92ORkOk0y1WsoUokHp-hLRicfuxMtj9EUf4fK1KH4Yo";
            string strUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=" + corpid + "&corpsecret=" + secret;
            Access_token mode = new Access_token();
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();//在这里对Access_token 赋值  
                Access_token token = new Access_token();
                token = JsonHelper.ParseFromJson<Access_token>(content);
                mode.access_token = token.access_token;
                mode.expires_in = token.expires_in;
            }
            return mode;
        }


        /// <summary>
        /// 获取Access_token值
        /// </summary>
        /// <returns></returns>
        public static string IsExistAccess_Token()
        {
            string Token = string.Empty;
            DateTime YouXRQ;
            // 读取XML文件中的数据，并显示出来 ，注意文件路径  
            string filepath = System.Web.HttpContext.Current.Server.MapPath("access_token.xml");
            StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            str.Close();
            str.Dispose();
            Token = xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText;
            YouXRQ = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText);
            if (DateTime.Now > YouXRQ)//当Access_token失效时，才重新获取
            {
                DateTime _youxrq = DateTime.Now;
                Access_token mode = GetAccess_token();
                xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText = mode.access_token;
                _youxrq = _youxrq.AddSeconds(int.Parse(mode.expires_in));
                xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText = _youxrq.ToString();
                xml.Save(filepath);
                Token = mode.access_token;
            }
            return Token;
        }


        /// <summary>
        /// 通过Access_token和code获取userinfo
        /// </summary>
        /// <returns></returns>
        private static User_info GetUser_info()
        {
            string strUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token=" + AccessToken + "&code=" + Code;
            //string strUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token=" + AccessToken + "&code=9xn8L6r98AxEjpx7o3n794kU6rH4F5UiYZqnuqKnAuc";
            User_info mode = new User_info();
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();//在这里对 User_info 赋值  
                User_info user = new User_info();
                user = JsonHelper.ParseFromJson<User_info>(content);
                mode.UserId = user.UserId;
                mode.DeviceId = user.DeviceId;
            }
            return mode;
        }


        //将code临时存放至xml文件，调试需要
        public static string  Code2Xml(string _code)
        {

            try
            {
                string filepath = System.Web.HttpContext.Current.Server.MapPath("code.xml");
                StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
                XmlDocument xml = new XmlDocument();
                xml.Load(str);
                str.Close();
                str.Dispose();
                xml.SelectSingleNode("xml").SelectSingleNode("code").InnerText = _code;
                xml.Save(filepath);
                return "1";
            }
            catch (Exception e)
            {

                throw e;
            }


    


        }

        /// <summary>
        /// 工具类
        /// </summary>
        public class User_info
        {
            public User_info()
            {
                // 
                //TODO:用于验证Access_token是否过期实体
                // 
            }
            string _user_id;
            string _device_id;

            /// <summary> 
            /// 获取到的userid  
            /// </summary> 
            public string UserId
            {
                get { return _user_id; }
                set { _user_id = value; }
            }

            /// <summary> 
            /// 设备id
            /// </summary> 
            public string DeviceId
            {
                get { return _device_id; }
                set { _device_id = value; }
            }
        }

        /// <summary>
        /// 工具类
        /// </summary>
        public class Access_token
        {
            public Access_token()
            {
                // 
                //TODO:用于验证Access_token是否过期实体
                // 
            }
            string _access_token;
            string _expires_in;

            /// <summary> 
            /// 获取到的凭证  
            /// </summary> 
            public string access_token //类属性名称需要与json数据中字符串保持一致！！！
            {
                get { return _access_token; }
                set { _access_token = value; }
            }

            /// <summary> 
            /// 凭证有效时间，单位：秒 
            /// </summary> 
            public string expires_in
            {
                get { return _expires_in; }
                set { _expires_in = value; }
            }
        }

    }
}