﻿using AutoMapper;
using HotelBookingApp.Data;
using HotelBookingApp.Models.Account;
using HotelBookingApp.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
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
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/login";
                opt.AccessDeniedPath = "/access-denied";
            });
            return services;
        }

        public static IServiceCollection AddAuthentications(this IServiceCollection services, IConfiguration configuration)
        {
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
               .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
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

        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Endpoints", Version = "v1" });
                c.EnableAnnotations();

                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                var commentsFile = Path.Combine(baseDirectory, commentsFileName);

                c.IncludeXmlComments(commentsFile);
            });
            return services;
        }
    }
}
