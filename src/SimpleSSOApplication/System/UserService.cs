using FreeBird.Infrastructure;
using FreeBird.Infrastructure.Domain;
using FreeBird.Infrastructure.Domain.Repositories;
using FreeBird.Infrastructure.Domain.Uow;
using FreeBird.Infrastructure.Exceptions;
using FreeBird.Infrastructure.Linq;
using FreeBird.Infrastructure.Utilities;
using SimpleSSO.Application.Core;
using SimpleSSO.Domain;
using SimpleSSO.Domain.System;
using SimpleSSO.DTO.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSSO.Application.System
{
    public class UserService
    {
        private readonly IRepository<User> _userRepo;

        private readonly IRepository<Role> _roleRepo;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentContext _currentContext;

        public UserService(IRepository<User> userRepo, IRepository<Role> roleRepo, IUnitOfWork unitOfWork,
            ICurrentContext currentContext)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _unitOfWork = unitOfWork;
            _currentContext = currentContext;
        }

        public void Add(UserDTO userParam)
        {
            Guard.ArgumentNotNull(userParam, "userParam");
            if (_userRepo.Exists(u => u.Name == userParam.Name))
            {
                throw new BusinessException("该用户名已存在，请另外选择一个。");
            }
            User user = new User()
            {
                Name = userParam.Name,
                Password = userParam.Password,
                RealName = userParam.RealName,
                Mobile = userParam.Mobile,
                Email = userParam.Email,
                IsLocked = userParam.IsLocked
            };
            user.Init(_currentContext.User?.Name);
            user.EncryptPassword();
            _userRepo.Add(user);
            _unitOfWork.Commit();
        }

        public void Update(UserDTO userParam)
        {
            Guard.ArgumentNotNull(userParam, "userParam");
            User oldUser = _userRepo.Get(u => u.ID == userParam.ID);
            if (oldUser == null)
            {
                throw new BusinessException("不存在该用户。");
            }
            oldUser.RealName = userParam.RealName;
            oldUser.Mobile = userParam.Mobile;
            oldUser.Email = userParam.Email;
            oldUser.IsLocked = userParam.IsLocked;
            oldUser.InitUpdate(_currentContext.User?.Name);
            var oldRoles = oldUser.Roles.ToList();
            //新增
            userParam.RoleIDs.Where(w => oldRoles.All(a => a.ID != w)).ToList().ForEach(
                a =>
                {
                    oldUser.Roles.Add(_roleRepo.Get(a));
                });
            //删除
            oldRoles.Where(w => !userParam.RoleIDs.Contains(w.ID)).ToList().ForEach(
                a =>
                {
                    oldUser.Roles.Remove(a);
                });
            if (!string.IsNullOrWhiteSpace(userParam.Password))
            {
                oldUser.Password = userParam.Password;
                oldUser.EncryptPassword();
            }
            _userRepo.Update(oldUser);
            _unitOfWork.Commit();

        }

        public void ResetPassword(int userID, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("密码不能为空。");
            }

            User user = _userRepo.Get(p => p.UserID == userID);
            user.Password = password;
            user.EncryptPassword();

            _userRepo.Update(user);
            _unitOfWork.Commit();
        }

        public void Delete(Guid id)
        {
            User user = _userRepo.Get(id);
            _userRepo.Delete(user);
            _unitOfWork.Commit();
        }

        public void Delete(List<Guid> ids)
        {
            _userRepo.DeleteOnDemand(ids);
        }

        public User GetUserByName(string name)
        {
            return _userRepo.Get(x => x.Name == name);
        }

        public MsgResult<User> Login(string userName, string password)
        {
            User user = this.GetUserByName(userName);
            return Login(user, password);
        }

        public MsgResult<User> Login(int userID, string password)
        {
            User user = _userRepo.Get(t => t.UserID == userID);
            return Login(user, password);
        }

        private MsgResult<User> Login(User user,string password)
        {
            if (user == null)
            {
                return new MsgResult<User>(false, null, "用户不存在");
            }
            if (user.PasswordEqual(password))
            {
                if (user.IsLocked)
                {
                    return new MsgResult<User>(false, null, "用户不存在");
                }
                return new MsgResult<User>(user);
            }
            return new MsgResult<User>(false, null, "用户不存在");
        }

        public IEnumerable<User> FindAll()
        {
            return _userRepo.GetAll();
        }

        public IEnumerable<User> Query(UserDTO user, IPageParam pageParam)
        {
            Guard.ArgumentNotNull(user, "user");
            Guard.ArgumentNotNull(pageParam, "pageParam");
            var expression = ExpressionBuilder.True<User>();
            expression = expression.AndIf(t => t.Name.Contains(user.Name), !string.IsNullOrEmpty(user.Name));
            return _userRepo.Query(expression, t => t.CreatedOn, true, pageParam);
        }

    }
}
