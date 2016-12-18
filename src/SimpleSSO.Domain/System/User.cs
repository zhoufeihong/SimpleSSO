using FreeBird.Infrastructure.Domain;
using FreeBird.Infrastructure.Utilities.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSSO.Domain.System
{
    public class User : EntityBase
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsLocked { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public virtual ICollection<Role> Roles
        {
            get;
            set;
        }
        public void EncryptPassword()
        {
            this.Password = EncryptionUtility.EncryptSHA1(this.Password, this.Salt);
        }
        public override void Init(string userName)
        {
            base.Init(userName);
            this.Salt = EncryptionUtility.GenerateSalt();
        }
        public bool PasswordEqual(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            return this.Password.Equals(EncryptionUtility.EncryptSHA1(password, this.Salt));
        }

    }
}
