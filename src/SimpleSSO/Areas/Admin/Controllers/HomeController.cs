using FreeBird.Infrastructure.Core.Authorize;
using FreeBird.Infrastructure.Mvc;
using FreeBird.Infrastructure.TypeUtilities.TypeAdapter;
using SimpleSSO.Application.System;
using SimpleSSO.Domain.System;
using SimpleSSO.DTO.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SimpleSSO.Setting;

namespace SimpleSSO.Areas.Admin.Controllers
{
    [FreeBirdMvcAuthorize(Roles = RoleConfig.InUserAdminRole)]
    public class HomeController : BaseMvcController
    {
        private UserService _userService;
        private ITypeAdapter _typeAdapter;

        public HomeController(UserService userService, ITypeAdapter typeAdapter)
        {
            _userService = userService;
            _typeAdapter = typeAdapter;
        }

        // GET: Admin/Account
        public ActionResult Index()
        {
            var userDTO = _typeAdapter.Adapt<UserDTO>(_userService.GetUserByName(UserName));
            if (userDTO.Name.Equals("admin"))
            {
                userDTO.IsAdmin = true;
            }
            return View(userDTO);
        }

    }
}