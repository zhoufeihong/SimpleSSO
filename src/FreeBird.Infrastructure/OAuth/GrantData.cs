using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeBird.Infrastructure.OAuth
{
    public class GrantData
    {
        public string ResponseType
        {
            get;
            set;
        }

        public string GrantType
        {
            get;
            set;
        }

        public string TokenType {
            get;
            set;
        }

        public string Token
        {
            get;
            set;
        }

        public string RoleScope {
            get;
            set;
        }

        public string ClientID
        {
            get;
            set;
        }

        public string UserID
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public DateTime IssuedUtc
        {
            get;
            set;
        }

        public DateTime ExpiresUtc
        {
            get;
            set;
        }

        public string Ticket {
            get;
            set;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public GrantData Deserialize(string str)
        {
            return JsonConvert.DeserializeObject<GrantData>(str);
        }

    }
}