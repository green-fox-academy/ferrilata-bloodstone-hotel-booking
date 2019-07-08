using AutoMapper.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;

namespace HotelBookingApp.Configs
{
    public static class LocalizationConfigs
    {
        private static string DEFAULT_LANGUAGE = "hu";
        public static IServiceCollection SetLocalization(this IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("hu")
                    };

                options.DefaultRequestCulture = new RequestCulture(DEFAULT_LANGUAGE);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            return services;
        }

        public static IApplicationBuilder SetLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("hu"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(DEFAULT_LANGUAGE),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });
            return app;
        }
    }
}
