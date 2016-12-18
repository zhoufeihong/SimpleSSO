using SimpleSSO.Application.System;
using System.Web.Http;
using SimpleSSO.DTO.System;
using FreeBird.Infrastructure.TypeUtilities.TypeAdapter;
using FreeBird.Infrastructure.Core.Authorize;
using static SimpleSSO.Setting;
using System.Linq;

namespace SimpleSSO.Controllers
{
    [FreeBirdApiAuthorize(Roles = RoleConfig.InAppUserBaseRole + "," + RoleConfig.AppRole)]
    [RoutePrefix("TicketUser")]
    public class TicketUserController : ApiController
    {
        private UserService _userService;
        private AppService _appService;
        private ITypeAdapter _typeAdapter;

        public TicketUserController(UserService userService, AppService appService, ITypeAdapter typeAdapter)
        {
            _userService = userService;
            _typeAdapter = typeAdapter;
            _appService = appService;
        }

        [Route("TicketMessage")]
        [HttpGet]
        public IHttpActionResult TicketMessage()
        {
            if (User.IsInRole(RoleConfig.AppRole))
            {
                var app = _appService.Query(new AppDTO { Name = User.Identity.Name }, null).FirstOrDefault();
                return Json(
                    new
                    {
                        AppID = app.ID,
                        AppName = app.Name,
                        RedirectUri = app.ReturnUrl
                    });
            }
            var userData = _userService.GetUserByName(User.Identity.Name);
            var result = _typeAdapter.Adapt<UserDTO>(userData);
            return Json(new
            {
                UserID = result.UserID,
                UserName = result.Name,
                Mobile = result.Mobile,
                Email = result.Email
            });
        }

    }
}
