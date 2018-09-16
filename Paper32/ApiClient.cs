using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Paper32
{
    class ApiClient
    {
        string url;
        string mac;
        string user;
        string auth;
        public ApiClient(string _url, string _mac, string _user, string _auth)
        {
            url = _url;
            mac = _mac;
            user = _user;
            auth = _auth;
        }
        static public void DoTrace(string _url, string _mac, string _user, string _version)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_url + "/users/" + _mac + "/trace/" + _user + "/" + _version + "/do");
            req.Timeout = 500;
            req.Method = "TRACE";
            try
            {
                req.GetResponse();
            }
            catch { }
        }
        public string GetInstruction()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url + "/users/" + mac + "/last");
            req.Timeout = 500;
            req.Method = "GET";
            req.Headers.Add("Authorization", auth);
            try
            {
                WebResponse res = req.GetResponse();
                return new StreamReader(res.GetResponseStream()).ReadToEnd();
            }
            catch { }
            return "";
        }
        public void DeleteInstruction()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url + "/users/" + mac + "/done");
            req.Timeout = 500;
            req.Method = "DELETE";
            req.Headers.Add("Authorization", auth);
            try
            {
                /*
                 *   https://youtu.be/T3ck6_ytGao
                 */
                req.GetResponse();
            }
            catch { }
        }
    }
    /*
    *     4tucbFg <--  ogidoc le sap tse'n icec     !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    *
    *     NEXTNEXTNEXT PETS T+HERE 
    *     
            {{{{{{{{{that }}}}}}}}} cord
    */
}
