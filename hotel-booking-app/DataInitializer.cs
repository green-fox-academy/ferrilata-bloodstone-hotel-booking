using HotelBookingApp.Models.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp
{
    public class DataInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            SeedPropertyTypes(context);
            SeedBeds(context);
            SeedLocations(context); 
        }

        public static void SeedPropertyTypes(ApplicationContext context)
        {
            if (context.PropertyTypes.Any())
            {
                return;
            }

            var propetyTypes = new PropertyType[]
            {
                new PropertyType { PropertyTypeId = 1, Type = "Apartment" },
                new PropertyType { PropertyTypeId = 2, Type = "Hostel" },
                new PropertyType { PropertyTypeId = 3, Type = "Hotel" },
                new PropertyType { PropertyTypeId = 4, Type = "Guesthouse" }
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
                new Bed { Type = "full bed", Size = 1},
                new Bed { Type = "twin bed", Size = 2},
                new Bed { Type = "single bed", Size = 1},
                new Bed { Type = "sofa bed", Size = 1},
           };
            foreach (Bed b in beds)
            {
                context.Beds.Add(b);
            }
            context.SaveChanges();
        }

        public static void SeedLocations(ApplicationContext context)
        {
            if (context.Locations.Any())
            {
                return;
            }
            var locations = new Location[]
          {
                new Location { LocationId = 1, Country = "Moon", City = "Moon", Region = "Moon District?", Address = "a street 1" },
                new Location { LocationId = 2, Country = "Mars", City = "Mars", Region = "Mars District?", Address = "a street mars" },
                new Location { LocationId = 3, Country = "Pluto", City = "p", Region = "Pluto District?", Address = "a street p" }
          };
            foreach (Location l in locations)
            {
                context.Locations.Add(l);
            }
            context.SaveChanges();
        }
    }
}
