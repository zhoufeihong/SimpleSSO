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
    public class RoleService
    {
        private readonly IRepository<Role> _roleRepo;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentContext _currentContext;

        public RoleService(IRepository<Role> roleRepo, IUnitOfWork unitOfWork,
            ICurrentContext currentContext)
        {
            _roleRepo = roleRepo;
            _unitOfWork = unitOfWork;
            _currentContext = currentContext;
        }

        public void Add(RoleDTO roleParam)
        {
            Guard.ArgumentNotNull(roleParam, "roleParam");
            if (_roleRepo.Exists(u => u.Name == roleParam.Name))
            {
                throw new BusinessException("该应用已存在，请另外选择一个。");
            }
            Role role = new Role
            {
                Name = roleParam.Name
            };
            role.Init(_currentContext.User?.Name);
            _roleRepo.Add(role);
            _unitOfWork.Commit();
        }

        public void Update(RoleDTO roleParam)
        {
            Guard.ArgumentNotNull(roleParam, "appParam");
            Role oldRole = _roleRepo.Get(u => u.ID == roleParam.ID);
            if (oldRole == null)
            {
                throw new BusinessException("不存在该角色。");
            }
            oldRole.Name = roleParam.Name;
            oldRole.InitUpdate(_currentContext.User?.Name);
            _roleRepo.Update(oldRole);
            _unitOfWork.Commit();

        }

        public void Delete(List<Guid> ids)
        {
            _roleRepo.DeleteOnDemand(ids);
        }

        public IEnumerable<Role> Query(RoleDTO role, IPageParam pageParam)
        {
            var expression = ExpressionBuilder.True<Role>();
            if (role != null)
            {
                expression = expression.AndIf(t => t.Name == role.Name, !string.IsNullOrEmpty(role.Name));
            }
            if (pageParam == null)
            {
                return _roleRepo.Query(expression);
            }
            return _roleRepo.Query(expression, t => t.CreatedOn, true, pageParam);
        }

    }
}
