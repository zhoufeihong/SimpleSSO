// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Owin.Security.Provider;

namespace Microsoft.Owin.Security.SimpleSSO
{
    /// <summary>
    /// Provides context information to middleware providers.
    /// </summary>
    public class SimpleSSOReturnEndpointContext : ReturnEndpointContext
    {
        /// <summary>
        /// Initializes a new <see cref="MicrosoftAccountReturnEndpointContext"/>.
        /// </summary>
        /// <param name="context">OWIN environment</param>
        /// <param name="ticket">The authentication ticket</param>
        public SimpleSSOReturnEndpointContext(
            IOwinContext context,
            AuthenticationTicket ticket)
            : base(context, ticket)
        {
        }
    }
}
