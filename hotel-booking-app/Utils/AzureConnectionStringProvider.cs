using Microsoft.AspNetCore.Hosting;

namespace HotelBookingApp.Utils
{
    public class AzureConnectionStringProvider
    {
        public static string GetAzureConnectionString(IHostingEnvironment env)
        {
            return env.IsProduction() 
                ? "AzureStorageKey" 
                : "AzureEmulatedStorageKey";
        }
    }
}
