﻿using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using TradeSuiteApp.Web.Models;
using Microsoft.Owin.Security.OAuth;
using TradeSuiteApp.Web.Utils;
using System.Web.Http;
using WebApplication1.App_Start;
using TokenBaseAuthentication;

namespace TradeSuiteApp.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864

        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);


            ////Default
            //// Enable the application to use a cookie to store information for the signed in user
            //// and to use a cookie to temporarily store information about a user logging in with a third party login provider
            //// Configure the sign in cookie
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    Provider = new CookieAuthenticationProvider
            //    {
            //        // Enables the application to validate the security stamp when the user logs in.
            //        // This is a security feature which is used when you change a password or add an external login to your account.  
            //        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
            //            validateInterval: TimeSpan.FromMinutes(30),
            //            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))

            //    },
            //    SlidingExpiration = false,  //If true then re-issued on any request, if false then logged out after specific time
            //    ExpireTimeSpan = TimeSpan.FromSeconds(10)

            //});   

            //Added
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnResponseSignIn = context =>
                    {
                        context.Properties.AllowRefresh = true;
                        context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60);
                    }
                },
                SlidingExpiration = true,  //This is optional. If true then re-issued on each and every request, if false then logged out after specific time
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(60));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
            //////////////////////////////////////////////////////////////
            /////////////////FOR API//////////////////////////////////////
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            //var myProvider = new MyAuthProvider();
            //OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            //{
            //    AllowInsecureHttp = true,
            //    TokenEndpointPath = new PathString("/token"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
            //    Provider = myProvider
            //}; 
            //app.UseOAuthAuthorizationServer(options);
            //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            //HttpConfiguration config = new HttpConfiguration();
            //WebApiConfig.Register(config);

        }
    }
}