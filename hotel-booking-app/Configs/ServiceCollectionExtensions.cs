﻿using AutoMapper;
using HotelBookingApp.Models.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HotelBookingApp.Configs
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomDatabase(this IServiceCollection services, IConfiguration config)
        {
            var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
            var connectionString = isProduction ? "ProductionConnection" : "DefaultConnection";

            services.AddDbContext<ApplicationContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString(connectionString)));
            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}
