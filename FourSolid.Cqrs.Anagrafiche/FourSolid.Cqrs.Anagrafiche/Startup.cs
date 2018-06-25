using System;
using System.IO;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Hubs;
using FourSolid.Cqrs.Anagrafiche.Mediator;
using FourSolid.Cqrs.Anagrafiche.Shared.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace FourSolid.Cqrs.Anagrafiche
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        public IContainer ApplicationContainer { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// 
        /// </summary>
        public InstantiateEventDispatcher Dispatcher { get; private set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", corsBuilder =>
                    corsBuilder.AllowAnyOrigin().
                        AllowAnyMethod().
                        AllowAnyHeader().
                        AllowCredentials());
            });
            #endregion

            #region Authentication via JWT
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.GetSecretKey()));
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = this.Configuration["4Solid:TokenAuthentication:Issuer"],

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = this.Configuration["4Solid:TokenAuthentication:Audience"],

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            //https://github.com/aspnet/Security/blob/dev/samples/JwtBearerSample/Startup.cs
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = tokenValidationParameters;
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";

                        return c.Response.WriteAsync("An error occurred processing your authentication.");
                    }
                };
            });
            #endregion

            #region MVC
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #endregion

            #region SignalR
            services.AddSignalR();
            #endregion

            #region Configuration
            services.Configure<FourSettings>(options =>
                this.Configuration.GetSection("4Solid").Bind(options));
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "4Solid",
                    Version = "v1",
                    Description = "Web Api Services for Anagrafiche",
                    TermsOfService = ""
                });

                var pathDoc = "FourSolid.Cqrs.Anagrafiche.xml";

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, pathDoc);
                if (File.Exists(filePath))
                    c.IncludeXmlComments(filePath);

                c.DescribeAllEnumsAsStrings();
            });
            #endregion

            #region Autofac Create the container builder.
            var builder = AutofacBootstrapper.RegisterModules(this.Configuration);
            builder.Populate(services);
            this.ApplicationContainer = builder.Build();
            #endregion

            #region EventsDispather
            // Start Dispatcher events from External Services / Internal Comand
            this.Dispatcher = new InstantiateEventDispatcher();
            this.Dispatcher.StartDispatcher(this.ApplicationContainer);
            #endregion

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="applicationLifetime"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            #region Logging
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddSerilog();
            loggerFactory.AddFile("Logs/Anagrafiche-{Date}.txt");
            loggerFactory.AddDebug();

            // Ensure any buffered events are sent at shutdown
            applicationLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
            applicationLifetime.ApplicationStopped.Register(this.Dispatcher.Dispose);
            #endregion

            #region CORS
            app.UseCors("CorsPolicy");
            #endregion

            #region Authentication
            app.UseAuthentication();
            #endregion

            #region MVC
            app.UseMvc();
            #endregion

            #region Start Index
            app.UseDefaultFiles();
            #endregion

            #region SignalR
            app.UseSignalR(routes =>
            {
                routes.MapHub<ArticoliHub>("/articoli");
                routes.MapHub<ClientiHub>("/clienti");
            });
            #endregion

            #region Swagger
            app.UseStaticFiles();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "documentation/{documentName}/documentation.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/documentation/v1/documentation.json", "Anagrafiche Web API");
            });
            #endregion
        }

        #region Helpers
        private string GetSecretKey()
        {
            var secretKey = this.Configuration["4Solid:TokenAuthentication:SecretKey"];
            var validSymmetricKey = Convert.FromBase64String(Convert.ToBase64String(Encoding.Unicode.GetBytes(secretKey)));

            return Encoding.UTF8.GetString(validSymmetricKey);
        }
        #endregion
    }
}
