using System;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Boy8.Models;
using Boy8.DAL;

namespace Boy8
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(Baby7DbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, Boy7User>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            app.UseTwitterAuthentication(
               consumerKey: ConfigurationManager.AppSettings["TwitterClientId"],  //"d3nK7tbc7dnSCwrv7M9NHn0Ot",
               consumerSecret: ConfigurationManager.AppSettings["TwitterClientSecret"]  //"Ir1YCMC0DuWEsVLDEA27DQFwKT0gekkRnSJiJlM8lFUFwGprCi");
            );

            //localhost
            app.UseFacebookAuthentication(
               appId: ConfigurationManager.AppSettings["FacebookClientId"],         //"1500725923474073",
               appSecret: ConfigurationManager.AppSettings["FacebookClientSecret"]  //"577d3c15de6ca70437b60336dd01dc30");
            );

            //localhost
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["GoogleClientId"].ToString(),  // "729988413782-f4fej2kmnmu29pf26j63ampb5b88ut0j.apps.googleusercontent.com",
                ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"] //"5yR9-aeEHoqNymAZTjsDT9NQ"
            });

            //Azure
            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "729988413782-cfhk0oescuouklj0tjs645qgkauobfei.apps.googleusercontent.com",
            //    ClientSecret = "6c3FrrOxZsyldXgydwPn55ad"
            //});
        }
    }
}