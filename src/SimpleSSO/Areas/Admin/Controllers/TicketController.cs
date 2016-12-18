using FreeBird.Infrastructure.Core.Authorize;
using FreeBird.Infrastructure.Mvc;
using FreeBird.Infrastructure.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SimpleSSO.Setting;

namespace SimpleSSO.Areas.Admin.Controllers
{
    [FreeBirdMvcAuthorize(Roles = RoleConfig.AdminRole)]
    public class TicketController : BaseMvcController
    {
        private ITicketManage _ticketManage;

        public TicketController(ITicketManage ticketManage)
        {
            _ticketManage = ticketManage;
        }

        // GET: Admin/Ticket
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Query
        public ActionResult Query()
        {
            var tikets = _ticketManage.GetAll().ToList();
            return Json(new { total = tikets.Count, rows = tikets });
        }

        [HttpPost]
        public ActionResult Delete(List<string> tokens)
        {
            if (tokens != null)
            {
                foreach (var token in tokens)
                {
                    var dGrantData = new GrantData
                    {
                        Token = token
                    };
                    _ticketManage.RemoveTicketValue(ref dGrantData);
                }
            }
            return Json("success");
        }

    }
}