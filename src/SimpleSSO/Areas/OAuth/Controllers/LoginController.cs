using FreeBird.Infrastructure.Core.Authorize;
using FreeBird.Infrastructure.Http;
using FreeBird.Infrastructure.Mvc;
using FreeBird.Infrastructure.TypeUtilities.TypeAdapter;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SimpleSSO.Application.System;
using SimpleSSO.Code;
using SimpleSSO.DTO.System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SimpleSSO.Setting;

namespace SimpleSSO.Areas.OAuth.Controllers
{
    [FreeBirdApiAuthorize(Roles = RoleConfig.InAppUserBaseRole)]
    public class LoginController : BaseMvcController
    {
        private UserService _userService;
        private ITypeAdapter _typeAdapter;

        public LoginController(UserService userService, ITypeAdapter typeAdapter)
        {
            _userService = userService;
            _typeAdapter = typeAdapter;
        }

        // GET: OAuth/Login
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return Redirect("~/Admin/Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(UserDTO userParam)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var isRemenber = Request.Form["isRemenber"].Contains("true");
            var roleName = Request.Form["ddl_Role"];
            var result = _userService.Login(userParam.Name, userParam.Password);
            if (result.Success)
            {
                var user = result.Data;
                var cookiesIdentity = ClaimsIdentityCreate.GenerateUserIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                var role = user.Roles.Select(s => s.Name).FirstOrDefault(t => t == roleName);
                if (string.IsNullOrEmpty(role))
                {
                    ModelState.AddModelError("", "角色选择不正确.");
                    return View();
                }
                cookiesIdentity.AddRole(role);
                Request.GetOwinContext().Request.Context.Authentication.SignIn(new
                    AuthenticationProperties
                {
                    IsPersistent = isRemenber
                }, cookiesIdentity);
                var queryStr = Request.QueryString["Query"];
                if (!string.IsNullOrEmpty(queryStr))
                {
                    FormStringControl queryControl = new FormStringControl(queryStr);
                    if (queryControl.ContainParamName("ReturnUrl"))
                    {
                        return Redirect(queryControl.GetParamValue("ReturnUrl"));
                    }
                }
                return Redirect("~/Admin/Home");
            }
            ModelState.AddModelError("", result.Message);
            return View();
        }

        [Route("Logout")]
        public ActionResult Logout()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Redirect(Url.Content("Index"));
        }

        [AllowAnonymous]
        public ActionResult UserRoles(string userName, string likeUser)
        {
            var user = _userService.GetUserByName(userName);
            if (user != null)
            {
                var results = user.Roles.Select(s => new { id = s.Name, text = s.Name })
                    .Where(t => t.text.Contains(likeUser) || string.IsNullOrWhiteSpace(likeUser))
                    .ToList();
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}