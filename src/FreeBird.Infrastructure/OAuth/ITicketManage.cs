using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBird.Infrastructure.OAuth
{
    public interface ITicketManage
    {
        IEnumerable<GrantData> GetAll();

        bool GetTicketValue(ref GrantData tokenValue);

        bool GetToken(GrantData grantData);

        bool RemoveOldTicket(GrantData grantData);

        bool RemoveTicketValue(ref GrantData tokenValue);

        bool SetTicketValue(GrantData tokenValue);

    }
}
