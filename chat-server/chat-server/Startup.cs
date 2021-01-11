using chat_server.Services;
using DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace chat_server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSignalR().AddMessagePackProtocol();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(
                        "http://localhost")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod();
                });
            });


            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            //         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //         services.AddSession(options =>
            //{
            //	options.IdleTimeout = TimeSpan.FromDays(1);
            //	//options.Cookie.HttpOnly = true;
            //	//options.Cookie.IsEssential = true;
            //});
            var connection = Configuration.GetConnectionString("ChatDatabase");
            services.AddDbContext<SignalRChatContext>(options => options.UseSqlServer(connection));

            ExtractServices.ExtractChatServices(services);
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddSession(); // added to enable session
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //services.AddSession(options => {
            //    options.Cookie.Name = ".WebTrainingRoom.Session";
            //    options.IdleTimeout = TimeSpan.FromMinutes(100);
            //    options.Cookie.IsEssential = true;

            //});



            // Add the cookie to the response cookie collection

            //services.AddHttpContextAccessor();

            //services.AddDistributedMemoryCache();

            //services.AddDistributedMemoryCache();//To Store session in Memory, This is default implementation of IDistributedCache    
            ////services.AddSession();
            //services.AddSession(options => {
            //    options.IdleTimeout = TimeSpan.FromDays(7);
            //});
            //services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/signalr");
            });
        }
    }
}
