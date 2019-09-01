using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace TelegramMirror.Controllers
{
    public class RedirectController : ApiController
    {
        // GET: api/Redirect
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        string GetTargetProperty(string Property, int TargetId)
        {
            string targetFile = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~\\App_Data\\Targets.txt"));
            var matches = Regex.Matches(targetFile, @"Target\[(\d+)\].(\w+)=(.*)");
            foreach (Match m in matches)
            {
                if (Convert.ToInt32(m.Groups[1].Value) == TargetId)
                    if (m.Groups[2].Value.ToLower() == Property.ToLower())
                        return m.Groups[3].Value.Replace("\r", "").Replace("\n", "");
            }

            return null;
        }

        void Log(string Text)
        {
            try
            {
                System.IO.File.WriteAllText(
                    HttpContext.Current.Server.MapPath("~\\App_Data\\Log.txt"),
                    Text);
            }
            catch { }
        }

        // GET: api/Redirect/5
        public HttpResponseMessage Get(int t, string u)
        {
            try
            {
                HttpClient cl = new HttpClient();
                cl.DefaultRequestHeaders.Clear();
                foreach (var h in Request.Headers)
                {
                    if (h.Key.ToLower().Contains("powered"))
                        continue;
                    cl.DefaultRequestHeaders.Add(h.Key, h.Value);
                }
                cl.DefaultRequestHeaders.Host = GetTargetProperty("Host", t);//"stage.bamantamin.com";
                var targetUrl = GetTargetProperty("URL", t);
                var result = cl.GetAsync(targetUrl + u).Result;

                HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK);
                httpResponse.Content = new StringContent(result.Content.ReadAsStringAsync().Result);
                httpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                return httpResponse;
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                throw ex;
            }
        }

        // POST: api/Redirect
        public HttpResponseMessage Post(int t, string u)
        {
            try
            {
                HttpClient cl = new HttpClient();
                cl.DefaultRequestHeaders.Clear();
                foreach (var h in Request.Headers)
                {
                    if (h.Key.ToLower().Contains("powered"))
                        continue;
                    cl.DefaultRequestHeaders.Add(h.Key, h.Value);
                }
                cl.DefaultRequestHeaders.Host = GetTargetProperty("Host", t);//"stage.bamantamin.com";
                var targetUrl = GetTargetProperty("URL", t);
                var result = cl.PostAsync(targetUrl + u, Request.Content).Result;

                HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.OK);
                httpResponse.Content = new StringContent(result.Content.ReadAsStringAsync().Result);
                httpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                return httpResponse;
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                throw ex;
            }
        }

        // PUT: api/Redirect/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Redirect/5
        public void Delete(int id)
        {
        }
    }
}
