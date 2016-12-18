using FreeBird.Infrastructure.Utilities;
using SimpleSSO.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace SimpleSSO.Code
{
    public static class ClaimsIdentityCreate
    {
        public static ClaimsIdentity GenerateUserIdentity(User user, string authenticationType)
        {
            var claimsIdentity = new ClaimsIdentity(authenticationType, ClaimTypes.Name, ClaimTypes.Role);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Name.ToString()));
            claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
            return claimsIdentity;
        }

        public static ClaimsIdentity AddRole(this ClaimsIdentity claimsIdentity, string role)
        {
            Guard.ArgumentNotNull(claimsIdentity, nameof(claimsIdentity));
            if (string.IsNullOrEmpty(claimsIdentity.RoleClaimType))
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            else
            {
                claimsIdentity.AddClaim(new Claim(claimsIdentity.RoleClaimType, role));
            }

            return claimsIdentity;
        }

        public static ClaimsIdentity GenerateAppIdentity(string clientID, string clientName, string authenticationType)
        {
            var claimsIdentity = new ClaimsIdentity(authenticationType, ClaimTypes.Name, ClaimTypes.Role);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, clientID.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, clientName.ToString()));
            claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
            return claimsIdentity;
        }
    }
}