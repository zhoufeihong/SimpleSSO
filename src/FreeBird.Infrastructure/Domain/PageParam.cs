using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBird.Infrastructure.Domain
{
    public class PageParam : IPageParam
    {
        public int Offset
        {
            get;
            set;
        }
        /// <summary>
        /// 每页显示的记录数（小于等于0时表示不分页）。
        /// </summary>
        public int Limit
        {
            get;
            set;
        }
        /// <summary>
        /// 符合条件的总记录数。
        /// </summary>
        public int TotalRecordCount
        {
            get;
            set;
        }
    }
}
