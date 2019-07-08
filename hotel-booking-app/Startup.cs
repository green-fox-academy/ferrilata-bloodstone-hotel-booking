using HotelBookingApp.Configs;
using HotelBookingApp.Data;
using HotelBookingApp.Models.Account;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;

namespace HotelBookingApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCustomDatabase(Configuration);
            services.AddCustomIdentity();
            services.AddAutoMapper();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IThumbnailService, ThumbnailService>();
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("hu")
                    };

                options.DefaultRequestCulture = new RequestCulture("hu");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext applicationContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                applicationContext.Database.Migrate();
                app.UseHsts();
            }
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("hu"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("hu"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });
            app.UseHttpsRedirection();
            app.UseAuthentication();
            ApplicationUserInitializer.SeedData(userManager, roleManager);
            app.UseStaticFiles();
            app.UseRequestLocalization();
            app.UseMvc();
        }
    }
}
