using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;

namespace FreeBird.Infrastructure.Mvc
{
    public class BaseMvcController : Controller
    {
        public string UserName
        {
            get
            {
                return User.Identity.Name;
            }
        }

        protected virtual JsonResult Json(string message, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            return Json(message, null, behavior);
        }

        protected new JsonResult Json(object data, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            return Json(string.Empty, data, behavior);
        }

        protected virtual JsonResult Json(string message, object data, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            return new ASJsonResult
            {
                Data = new { Success = true, Message = message, Data = data },
                JsonRequestBehavior = behavior
            };
        }

        protected virtual JsonResult JsonFail(string message, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            return new ASJsonResult
            {
                Data = new { Success = true, Message = message },
                JsonRequestBehavior = behavior
            };
        }

    }
}
