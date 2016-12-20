// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.SimpleSSO;

namespace Owin
{
    /// <summary>
    /// Extension methods for using <see cref="MicrosoftAccountAuthenticationMiddleware"/>
    /// </summary>
    public static class SimpleSSOAccountAuthenticationExtensions
    {
        /// <summary>
        /// Authenticate users using Microsoft Account
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="options">Middleware configuration options</param>
        /// <returns>The updated <see cref="IAppBuilder"/></returns>
        public static IAppBuilder UseSimpleSSOAccountAuthentication(this IAppBuilder app, SimpleSSOAccountAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            app.Use(typeof(SimpleSSOAccountAuthenticationMiddleware), app, options);
            return app;
        }

        /// <summary>
        /// Authenticate users using Microsoft Account
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="clientId">The application client ID assigned by the Microsoft authentication service</param>
        /// <param name="clientSecret">The application client secret assigned by the Microsoft authentication service</param>
        /// <returns></returns>
        public static IAppBuilder UseSimpleSSOAccountAuthentication(
            this IAppBuilder app,
            string clientId,
            string clientSecret)
        {
            return UseSimpleSSOAccountAuthentication(
                app,
                new SimpleSSOAccountAuthenticationOptions
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                });
        }
    }
}
