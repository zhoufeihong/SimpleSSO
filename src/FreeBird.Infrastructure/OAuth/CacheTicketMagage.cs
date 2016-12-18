using FreeBird.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBird.Infrastructure.OAuth
{
    public class CacheTicketMagage : ITicketManage
    {
        protected ITicketStore TicketStore => EngineContext.Current.Resolve<ITicketStore>();

        public IEnumerable<GrantData> GetAll()
        {
            foreach (var obj in TicketStore.GetAll())
            {
                if (obj is GrantData)
                    yield return obj as GrantData;
            }
        }

        public bool GetTicketValue(ref GrantData tokenValue)
        {
            if (TicketStore.GetValue(tokenValue.Token, out tokenValue))
            {
                return true;
            }
            tokenValue = new GrantData();
            return false;
        }

        public bool GetToken(GrantData grantData)
        {
            foreach (var val in this.GetAll())
            {
                if (IsOneToken(grantData, val))
                {
                    grantData.Token = val.Token;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveOldTicket(GrantData grantData)
        {
            foreach (var val in this.GetAll())
            {
                if (IsOneToken(grantData, val))
                {
                    return TicketStore.Remove(val.Token);
                }
            }
            return false;
        }

        public bool RemoveTicketValue(ref GrantData tokenValue)
        {
            if (TicketStore.Remove(tokenValue.Token, out tokenValue))
            {
                return true;
            }
            tokenValue = new GrantData();
            return false;
        }

        public bool SetTicketValue(GrantData tokenValue)
        {
            TicketStore.Set(tokenValue.Token, tokenValue, (tokenValue.ExpiresUtc - tokenValue.IssuedUtc).Minutes);
            return true;
        }

        private bool IsOneToken(GrantData grantData, GrantData grantDataN)
        {
            if (grantData.ClientID == grantDataN.ClientID
                && grantData.UserID == grantDataN.UserID
                && grantData.TokenType == grantDataN.TokenType)
            {
                return true;
            }
            return false;
        }

    }
}
