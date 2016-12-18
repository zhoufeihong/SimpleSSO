using FreeBird.Infrastructure.Core.Authorize;
using FreeBird.Infrastructure.Domain;
using FreeBird.Infrastructure.Mvc;
using FreeBird.Infrastructure.TypeUtilities.TypeAdapter;
using SimpleSSO.Application.System;
using SimpleSSO.DTO.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using static SimpleSSO.Setting;

namespace SimpleSSO.Areas.Admin.Controllers
{
    [FreeBirdMvcAuthorize(Roles = RoleConfig.AdminRole)]
    public class AppController : BaseMvcController
    {
        private AppService _appService;
        private ITypeAdapter _typeAdapter;

        public AppController(AppService appService, ITypeAdapter typeAdapter)
        {
            _appService = appService;
            _typeAdapter = typeAdapter;
        }

        // GET: Admin/App
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Query(AppDTO param, PageParam pageParam)
        {
            List<AppDTO> list = new List<AppDTO>();
            _typeAdapter.Adapt(_appService.Query(param, pageParam).ToList(), list);
            return Json(new { total = pageParam.TotalRecordCount, rows = list });
        }

        public ActionResult Delete(List<Guid> ids)
        {
            _appService.Delete(ids);
            return Json("Sucess");
        }

        public ActionResult Create(AppDTO appParam)
        {
            _appService.Add(appParam);
            return Json("Sucess");
        }

        public ActionResult Edit(AppDTO appParam)
        {
            _appService.Update(appParam);
            return Json("Sucess");
        }

        [FreeBirdMvcAuthorize(Roles = RoleConfig.InUserAdminRole)]
        public ActionResult List()
        {
            List<AppDTO> list = new List<AppDTO>();
            return View(_typeAdapter.Adapt(_appService.Query(null, null).ToList(), list));
        }

        [HttpPost]
        public ActionResult ImportIconImage()
        {
            if (Request.Files != null && Request.Files.Count == 0)
            {
                return JsonFail("没有文件！");
            }
            HttpPostedFileBase file = Request.Files[0];
            var name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fileName = Path.Combine(Request.MapPath("~/Upload/AppIcon"), name);
            try
            {
                file.SaveAs(fileName);
                var path = "/Upload/AppIcon/" + name;
                return Json("Sucess", new { Path = path });
            }
            catch
            {
                return JsonFail("上传异常！");
            }
        }
    }
}