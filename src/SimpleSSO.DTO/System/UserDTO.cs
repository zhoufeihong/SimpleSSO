using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSSO.DTO.System
{
    public class UserDTO : EntityBaseDTO
    {
        public int UserID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 3)]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsLocked { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsAdmin { get; set; }
        public List<RoleDTO> Roles
        {
            get;
            set;
        }
        private List<Guid> _roleIDs
        {
            get;
            set;
        }
        public List<Guid> RoleIDs
        {
            get
            {
                return (Roles?.Select(s => s.ID).ToList() ?? _roleIDs)?.Where(t => !t.Equals(Guid.Empty)).ToList();
            }
            set
            {
                _roleIDs = value;
            }
        }

    }
}
