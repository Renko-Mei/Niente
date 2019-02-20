using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Niente.Middlewares
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
                // Add application information
                options.ClientId = Configuration["Bangumi:ClientId"];
                options.ClientSecret = Configuration["Bangumi:ClientSecret"];
                options.CallbackPath = new PathString("/bgmauth");

                // Add Bangumi endpoints
                options.AuthorizationEndpoint = Configuration["Bangumi:AuthEndPoint"];
                options.TokenEndpoint = Configuration["Bangumi:TokenEndPoint"];
                options.UserInformationEndpoint = Configuration["Bangumi:UserInfoEndPoint"];

                //options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                //options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                //options.ClaimActions.MapJsonKey("urn:github:login", "login");
                //options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
                //options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");
                //options.ClaimActions.MapJsonKey();

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        // Create the request message to get user data via the backchannel
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                        // Query user data via the backchannel
                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        // Parse user data into an object
                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                        // Store the token in a cookie
                        context.HttpContext.Response.Cookies.Append("Bangumi-Token", context.AccessToken);

                        // Execute the defined mapping action by using the received user object
                        context.RunClaimActions(user);
                    }
                };
            });
        }
    }
}
