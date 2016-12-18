using FreeBird.Infrastructure.Core.Authorize;
using FreeBird.Infrastructure.Domain;
using FreeBird.Infrastructure.Mvc;
using FreeBird.Infrastructure.TypeUtilities.TypeAdapter;
using SimpleSSO.Application.System;
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
    public class RoleController : BaseMvcController
    {
        private RoleService _roleService;
        private ITypeAdapter _typeAdapter;

        public RoleController(RoleService roleService, ITypeAdapter typeAdapter)
        {
            _roleService = roleService;
            _typeAdapter = typeAdapter;
        }

        // GET: Admin/Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Query(RoleDTO param, PageParam pageParam)
        {
            List<RoleDTO> list = new List<RoleDTO>();
            _typeAdapter.Adapt(_roleService.Query(param, pageParam).ToList(), list);
            return Json(new { total = pageParam.TotalRecordCount, rows = list });
        }

        public ActionResult Delete(List<Guid> ids)
        {
            _roleService.Delete(ids);
            return Json("Sucess");
        }

        public ActionResult Create(RoleDTO roleParam)
        {
            _roleService.Add(roleParam);
            return Json("Sucess");
        }

        public ActionResult Edit(RoleDTO roleParam)
        {
            _roleService.Update(roleParam);
            return Json("Sucess");
        }

    }
}