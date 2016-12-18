using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSSO.DTO.System
{
    public class AppDTO: EntityBaseDTO
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
