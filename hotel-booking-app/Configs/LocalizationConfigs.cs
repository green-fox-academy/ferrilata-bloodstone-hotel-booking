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
        private static string DEFAULT_LANGUAGE = "en-US";
        private static List<CultureInfo> supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo(DEFAULT_LANGUAGE),
                        new CultureInfo("hu-HU")
                    };

    public static IServiceCollection SetLocalizationSource(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            return services;
        }
        public static IServiceCollection SetLocalization(this IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(DEFAULT_LANGUAGE);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });
            return services;
        }
    }
}
