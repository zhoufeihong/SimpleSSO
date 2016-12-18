using FreeBird.Infrastructure.Core.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleSSO.Code.Authorize
{
    public class SimpleSSOAuthorize : FreeBirdAuthorize
    {
        protected override bool IsAuthorized(FreeBirdAuthorizeModel model)
        {
            return true;
        }
    }
}