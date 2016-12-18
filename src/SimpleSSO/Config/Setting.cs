using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleSSO
{
    public static class Setting
    {
        public static class RoleConfig
        {

            #region Web

            public const string UserRole = "user";
            public const string AdminRole = "admin";
            public const string InUserAdminRole = UserRole + "," + AdminRole;

            #endregion

            #region API

            public const string AppRole = "app";
            public const string AppUserAllRole = "app-user-all";
            public const string AppUserBaseRole = "app-user-base";

            public const string InAppUserAllRole = "app-user-all," + AdminRole + "," + UserRole;
            public const string InAppUserBaseRole = "app-user-base," + AppUserAllRole + "," + AdminRole + "," + UserRole;

            #endregion
        }

        #region

        public static class ResponseTypes
        {
            public const string Code = "code";
            public const string Token = "token";
        }

        public static class GrantTypes
        {
            public const string AuthorizationCode = "authorization_code";
            public const string ClientCredentials = "client_credentials";
            public const string RefreshToken = "refresh_token";
            public const string Password = "password";
        }

        #endregion

        #region

        public const string TokenKeyPrefix = "IsToken";

        #endregion

        #region endpoint

        public static class EndPointConfig
        {
            public const string TokenEndpointPath = "/Token";

            public const string AuthorizeEndpointPath = "/GrantCode/Authorize";

            public const string AuthorizeGrant = "OAuth/Grant";

            public const string AuthorizeGrantAccess = AuthorizeGrant + "/Access";
        }

        #endregion
    }
}