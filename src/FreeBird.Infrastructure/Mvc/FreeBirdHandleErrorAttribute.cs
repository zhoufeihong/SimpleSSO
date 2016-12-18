using FreeBird.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FreeBird.Infrastructure.Mvc
{
    public class FreeBirdHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;
            bool isAjaxRequest = string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest");
            bool isUIFormRequest = string.Equals(request.QueryString["ajax"], "true");

            if (isAjaxRequest || isUIFormRequest)
            {
                HandleAjaxException(filterContext, isUIFormRequest);
            }
            else
            {
                HandleException(filterContext);
            }
        }

        private void HandleAjaxException(ExceptionContext filterContext, bool isEasyUIFormRequest)
        {
            string message;
            Exception exception = filterContext.Exception;

            if (exception is BusinessException)
            {
                message = exception.Message;
            }
            else if (exception is AuthorizationException)
            {
                message = "抱歉，您没有权限执行此操作。";
            }
            else
            {
                exception = filterContext.Exception.GetBaseException();
                message = exception.Message;
            }

            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jsonResult.Data = new { Success = false, Message = message };
            if (isEasyUIFormRequest) 
            {
                jsonResult.ContentType = "text/html";
            }

            filterContext.Result = jsonResult;
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 200; //设置HTTP状态码为200，以避免IIS使用内部错误页代替系统自定义错误页
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        private void HandleException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            if (!(filterContext.Exception is BusinessException || filterContext.Exception is AuthorizationException))
            {
                Exception exception = filterContext.Exception.GetBaseException();
            }

            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            filterContext.Result = new ViewResult
            {
                ViewName = View,
                MasterName = Master,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                TempData = filterContext.Controller.TempData
            };

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 200; //设置HTTP状态码为200，以避免IIS使用内部错误页代替系统自定义错误页
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}
