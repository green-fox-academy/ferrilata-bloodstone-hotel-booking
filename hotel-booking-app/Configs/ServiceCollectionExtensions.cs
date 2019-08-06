using AutoMapper;
using HotelBookingApp.Data;
using HotelBookingApp.Models.Account;
using HotelBookingApp.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace HotelBookingApp.Configs
{
    public static class ServiceCollectionExtensions
    {
        private static readonly string SHARED_RESOURCE_FOLDER_NAME = "SharedResources";

        public static IServiceCollection AddCustomDatabase(this IServiceCollection services, IConfiguration config)
        {
            var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
            var connectionString = isProduction ? "ProductionConnection" : "DefaultConnection";

            services.AddDbContext<ApplicationContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString(connectionString)));
            return services;
        }

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/login";
                opt.AccessDeniedPath = "/access-denied";
            });

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                    configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "Hotel-Booking",
                        ValidAudience = "Hotel-Booking",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["APISecretKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            IMapper mapper = MappingProfiles.GetAutoMapperProfiles().CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        public static IServiceCollection AddMvcWithLocalization(this IServiceCollection services)
        {
            services.AddMvc()
               .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
               .AddDataAnnotationsLocalization(o =>
               {
                   var type = typeof(SharedResources);
                   var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
                   var factory = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                   var localizer = factory.Create(SHARED_RESOURCE_FOLDER_NAME, assemblyName.Name);
                   o.DataAnnotationLocalizerProvider = (t, f) => localizer;
               });
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "Hotel-Booking",
                        ValidAudience = "Hotel-Booking",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["APISecretKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
            return services;
        }
    }
}
