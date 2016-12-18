using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleSSOTest.Controllers
{
    public class TokenClientController : Controller
    {
        private const string _serverUrl = "http://localhost:8550";

        private const string _serverTicketMessageUrl = _serverUrl + "/TicketUser/TicketMessage";
        // GET: TokenClient/ShowUser
        public ActionResult ShowUser()
        {
            ViewBag.ServerTicketMessageUrl = _serverTicketMessageUrl;
            return View();
        }
    }
}