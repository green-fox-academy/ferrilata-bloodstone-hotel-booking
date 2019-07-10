using HotelBookingApp.Configs;
using HotelBookingApp.Data;
using HotelBookingApp.Models.Account;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.SetLocalizationSource();
            services.AddMvcWithLocalization();
            services.AddCustomDatabase(Configuration);
            services.AddCustomIdentity();
            services.AddAutoMapper();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IThumbnailService, ThumbnailService>();
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            services.SetLocalization();
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
            app.UseHttpsRedirection();
            app.UseAuthentication();
            ApplicationUserInitializer.SeedData(userManager, roleManager);
            app.UseStaticFiles();
            app.UseRequestLocalization();
            app.UseMvc();
        }
    }
}
