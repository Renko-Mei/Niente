using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Niente.Common;
using Niente.Data;
using Niente.Middlewares;
using Niente.Models;

namespace Niente
{
    public class Startup
    {
        private readonly IHostingEnvironment HostingEvironment;
        private readonly IConfiguration Configuration;
        private readonly ILoggerFactory Logger;

        public Startup(IHostingEnvironment environment,
                       IConfiguration configuration,
                       ILoggerFactory logger)
        {
            HostingEvironment = environment;
            Configuration = configuration;
            Logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var logger = Logger.CreateLogger<Startup>();

            // Identifies the environment upon system start
            if (HostingEvironment.IsDevelopment())
            {
                logger.LogInformation("Start Niente application in development environment");
            }
            else
            {
                logger.LogInformation("Start Niente application in production environment");
            }

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy",
                    new CorsPolicyBuilder()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials()
                    .Build());
            });

            // Register database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            Identity.Configure(services);

            JwtAuthentication.Configure(services, Configuration);

            // Register custom username / password combination validator
            services.AddTransient<IPasswordValidator<ApplicationUser>, PwConstraintValidator>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("SiteCorsPolicy"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                // Host in nginx
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseAuthentication();

            app.UseCors("SiteCorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseMvc();

            context.Database.EnsureCreated();
        }
    }
}
