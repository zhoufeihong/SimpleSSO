using FreeBird.Infrastructure.Core.Authorize;
using FreeBird.Infrastructure.Domain;
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
    [FreeBirdMvcAuthorize(Roles = RoleConfig.AdminRole)]
    public class UserController : BaseMvcController
    {
        private UserService _userService;
        private ITypeAdapter _typeAdapter;

        public UserController(UserService userService, ITypeAdapter typeAdapter)
        {
            _userService = userService;
            _typeAdapter = typeAdapter;
        }

        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Query(UserDTO param,PageParam pageParam)
        {
            List<UserDTO> list = new List<UserDTO>();
            _typeAdapter.Adapt(_userService.Query(param, pageParam).ToList(), list);
            return Json(new { total = pageParam.TotalRecordCount, rows = list });
        }

        public ActionResult Delete(List<Guid> ids)
        {
            _userService.Delete(ids);
            return Json("Sucess");
        }

        public ActionResult Create(UserDTO userParam)
        {
            _userService.Add(userParam);
            return Json("Sucess");
        }

        public ActionResult Edit(UserDTO userParam)
        {
            _userService.Update(userParam);
            return Json("Sucess");
        }

    }
}