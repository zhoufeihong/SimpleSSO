using FreeBird.Infrastructure.Domain;
using FreeBird.Infrastructure.Domain.Repositories;
using FreeBird.Infrastructure.Domain.Uow;
using FreeBird.Infrastructure.Exceptions;
using FreeBird.Infrastructure.Linq;
using FreeBird.Infrastructure.Utilities;
using SimpleSSO.Application.Core;
using SimpleSSO.Domain.System;
using SimpleSSO.DTO.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSSO.Application.System
{
    public class AppService
    {
        private readonly IRepository<App> _appRepo;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentContext _currentContext;

        public AppService(IRepository<App> appRepo, IUnitOfWork unitOfWork,
            ICurrentContext currentContext)
        {
            _appRepo = appRepo;
            _unitOfWork = unitOfWork;
            _currentContext = currentContext;
        }

        public void Add(AppDTO appParam)
        {
            Guard.ArgumentNotNull(appParam, "appParam");
            if (_appRepo.Exists(u => u.Name == appParam.Name))
            {
                throw new BusinessException("该应用已存在，请另外选择一个。");
            }
            App app = new App
            {
                Name = appParam.Name,
                ClientSecret = appParam.ClientSecret,
                ReturnUrl = appParam.ReturnUrl,
                IsCredible = appParam.IsCredible,
                IconUrl = appParam.IconUrl
            };
            app.Init(_currentContext.User?.Name);
            _appRepo.Add(app);
            _unitOfWork.Commit();
        }

        public void Update(AppDTO appParam)
        {
            Guard.ArgumentNotNull(appParam, "appParam");
            App oldApp = _appRepo.Get(u => u.ID == appParam.ID);
            if (oldApp == null)
            {
                throw new BusinessException("不存在该应用。");
            }
            oldApp.Name = appParam.Name;
            oldApp.ClientSecret = appParam.ClientSecret;
            oldApp.ReturnUrl = appParam.ReturnUrl;
            oldApp.IsCredible = appParam.IsCredible;
            oldApp.IconUrl = appParam.IconUrl;
            oldApp.InitUpdate(_currentContext.User?.Name);
            _appRepo.Update(oldApp);
            _unitOfWork.Commit();

        }

        public void Delete(List<Guid> ids)
        {
            _appRepo.DeleteOnDemand(ids);
        }

        public IEnumerable<App> Query(AppDTO app, IPageParam pageParam)
        {
            var expression = ExpressionBuilder.True<App>();
            if (app != null)
            {
                expression = expression.AndIf(t => t.Name == app.Name, !string.IsNullOrEmpty(app.Name));
                expression = expression.AndIf(t => t.ClientID == app.ClientID, app.ClientID != 0);
            }
            if (pageParam == null)
            {
                return _appRepo.Query(expression);
            }
            return _appRepo.Query(expression, t => t.CreatedOn, true, pageParam);
        }
    }
}
