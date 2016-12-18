using SimpleSSO.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSSO.Application.Core
{
    public interface ICurrentContext
    {
        User User{ get; }
    }
}
