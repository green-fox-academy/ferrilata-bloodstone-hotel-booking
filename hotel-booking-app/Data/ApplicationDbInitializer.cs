using HotelBookingApp.Models.HotelModels;
using System.Linq;

namespace HotelBookingApp.Data
{
    public class ApplicatonDbInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            SeedPropertyTypes(context);
            SeedBeds(context);
        }

        public static void SeedPropertyTypes(ApplicationContext context)
        {
            if (context.PropertyTypes.Any())
            {
                return;
            }

            var propetyTypes = new PropertyType[]
            {
                new PropertyType { Type = "Apartment" },
                new PropertyType { Type = "Hostel" },
                new PropertyType { Type = "Hotel" },
                new PropertyType { Type = "Guesthouse" }
            };
            foreach (PropertyType p in propetyTypes)
            {
                context.PropertyTypes.Add(p);
            }
            context.SaveChanges();
        }

        public static void SeedBeds(ApplicationContext context)
        {
            if (context.Beds.Any())
            {
                return;
            }
            var beds = new Bed[]
            {
                new Bed { Type = "full bed", Size = 2},
                new Bed { Type = "twin bed", Size = 2},
                new Bed { Type = "single bed", Size = 1},
                new Bed { Type = "sofa bed", Size = 1}
            };
            foreach (Bed b in beds)
            {
                context.Beds.Add(b);
            }
            context.SaveChanges();
        }
    }
}
