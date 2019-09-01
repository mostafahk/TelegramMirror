using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace TelegramMirror.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index(string Url)
        {
            HttpClient client = new HttpClient();
            var r = client.GetAsync(Url).Result;

            return new FileStreamResult(r.Content.ReadAsStreamAsync().Result, r.Content.Headers.ContentType.ToString());
        }
    }
}