using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FreeBird.Infrastructure.Mvc
{
    public class ASJsonResult : JsonResult
    {
        public JsonSerializerSettings Settings { get; private set; }

        public ASJsonResult()
        {
            Settings = new JsonSerializerSettings
            {
                //解决循环引用的问题，也是json.net官方给出的解决配置选项
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString= "yyyy-MM-dd HH:mm:ss"
            };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpRequestBase request = context.HttpContext.Request;
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet
                && string.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JSON GET is not allowed");
            }

            string contentType = this.ContentType;
            if (string.IsNullOrEmpty(contentType))
            {
                bool isAjaxRequest = string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest");
                contentType = isAjaxRequest ? "application/json" : "text/html";
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = contentType;
            response.ContentEncoding = this.ContentEncoding ?? Encoding.UTF8;

            if (this.Data != null)
            {
                JsonSerializer serializer = JsonSerializer.Create(this.Settings);
                using (StringWriter sw = new StringWriter())
                {
                    serializer.Serialize(sw, this.Data);
                    response.Write(sw.ToString());
                }
            }
        }

    }
}
