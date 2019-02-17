using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace neu.Middlewares
{
    public class BangumiOAuth
    {
        public static void AddAuthentication(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Bangumi";
            })
            .AddCookie()
            .AddOAuth("Bangumi", options =>
            {
                options.ClientId = Configuration["Bangumi:ClientId"];
                options.ClientSecret = Configuration["Bangumi:ClientSecret"];
                options.CallbackPath = new PathString("/bgmauth");

                options.AuthorizationEndpoint = "https://bgm.tv/oauth/authorize";

            });
        }
    }
}
