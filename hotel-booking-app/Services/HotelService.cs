using HotelBookingApp.Data;
using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class HotelService : IHotelService
    {
        private readonly ApplicationContext applicationContext;
        private readonly IImageService imageService;
        private readonly IThumbnailService thumbnailService;
        private readonly IStringLocalizer<HotelService> localizer;

        private readonly IBedService bedService;
        private readonly IRoomService roomService;

        public HotelService(ApplicationContext applicationContext, IImageService imageService, IThumbnailService thumbnailService, IStringLocalizer<HotelService> localizer, 
            IBedService bedService, IRoomService roomService)
        {
            this.applicationContext = applicationContext;
            this.imageService = imageService;
            this.thumbnailService = thumbnailService;
            this.localizer = localizer;
            this.bedService = bedService;
            this.roomService = roomService;
        }

        public async Task<Hotel> Add(Hotel hotel)
        {
            await applicationContext.AddAsync(hotel);
            await applicationContext.SaveChangesAsync();
            return hotel;
        }

        public async Task Delete(int id)
        {
            var hotel = applicationContext.Hotels
                .SingleOrDefault(h => h.HotelId == id)
                ?? throw new ItemNotFoundException(localizer["Hotel with id: {0} is not found.", id]);
            applicationContext.Hotels.Remove(hotel);
            await applicationContext.SaveChangesAsync();
            await thumbnailService.DeleteAsync(id);
            await imageService.DeleteAllFileAsync(id);
        }

        public async Task<IEnumerable<Hotel>> FindAll()
        {
            return await applicationContext.Hotels.ToListAsync();
        }

        public async Task<IEnumerable<Hotel>> FindAllOrderByName()
        {
            return await applicationContext.Hotels
                .OrderBy(h => h.Name)
                .ToListAsync();
        }

        public async Task<Hotel> FindByIdAsync(int id)
        {
            var hotel = await applicationContext.Hotels
                .Include(h => h.Location)
                .Include(h => h.PropertyType)
                .Include(h => h.Rooms)
                    .ThenInclude(r => r.RoomBeds)
                        .ThenInclude(b => b.Bed)
                .SingleOrDefaultAsync(h => h.HotelId == id)
                ?? throw new ItemNotFoundException(localizer["Hotel with id: {0} is not found.", id]);
            return hotel;
        }


        public async Task<PaginatedList<Hotel>> FindWithQuery(QueryParams queryParams)
        {
            var filteredHotels = applicationContext.Hotels
                .Include(h => h.Location)
                .Where(h =>
                    string.IsNullOrEmpty(queryParams.Search)
                    || h.Location.City.Contains(queryParams.Search));

            var orderedHotels = QueryableUtils<Hotel>.OrderCustom(filteredHotels, queryParams);
            return await PaginatedList<Hotel>.CreateAsync(orderedHotels, queryParams.CurrentPage, queryParams.PageSize);
        }

        public async Task<Hotel> Update(Hotel hotel)
        {
            var propertyType = applicationContext.PropertyTypes
                .Find(hotel.PropertyTypeId)
                ?? throw new ItemNotFoundException(localizer["Property with id {0} is not foud!", hotel.PropertyTypeId]);

            hotel.PropertyType = propertyType;
            applicationContext.Update(hotel);
            await applicationContext.SaveChangesAsync();
            return hotel;
        }
    }
}
