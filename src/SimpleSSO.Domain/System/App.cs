using FreeBird.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSSO.Domain.System
{
    public class App : EntityBase
    {
        public string Name
        {
            get;
            set;
        }

        public int ClientID
        {
            get;
            set;
        }

        public string ClientSecret
        {
            get;
            set;
        }

        public string ReturnUrl
        {
            get;
            set;
        }

        public bool IsCredible
        {
            get;
            set;
        }

        public string IconUrl
        {
            get;
            set;
        }

    }
}
