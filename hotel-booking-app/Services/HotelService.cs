﻿using AutoMapper;
using HotelBookingApp.Data;
using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.API;
using HotelBookingApp.Models.HotelModels;
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
        private readonly IMapper mapper;

        public HotelService(ApplicationContext applicationContext,
            IImageService imageService,
            IThumbnailService thumbnailService,
            IStringLocalizer<HotelService> localizer,
            IMapper mapper)
        {
            this.applicationContext = applicationContext;
            this.imageService = imageService;
            this.thumbnailService = thumbnailService;
            this.localizer = localizer;
            this.mapper = mapper;
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

        public async Task<Hotel> FindByIdAsync(int id)
        {
            return await FindByIdAsync(id, new QueryParams());
        }

        public async Task<Hotel> FindByIdAsync(int id, QueryParams queryParams)
        {
            var hotel = await applicationContext.Hotels
                .Include(h => h.Location)
                .Include(h => h.PropertyType)
                .Include(h => h.Rooms)
                    .ThenInclude(r => r.RoomBeds)
                        .ThenInclude(b => b.Bed)
                .SingleOrDefaultAsync(h => h.HotelId == id)
                ?? throw new ItemNotFoundException(localizer["Hotel with id: {0} is not found.", id]);
            hotel.Reviews = await FindAllReviews(id, queryParams);
            return hotel;
        }

        public async Task<PaginatedList<Review>> FindAllReviews(int hotelId, QueryParams queryParams)
        {
            var reviews = applicationContext.Reviews
                .Include(r => r.ApplicationUser)
                .Where(r => r.HotelId == hotelId);
            return await PaginatedList<Review>.CreateAsync(reviews, queryParams.CurrentPage, queryParams.PageSize);
        }

        public async Task<PaginatedList<Hotel>> FindWithQuery(QueryParams queryParams)
        {
            var filteredHotels = GetHotels();
            return await GetPaginatedHotels(filteredHotels, queryParams);
        }

        public async Task<PaginatedList<Hotel>> FindWithQuery(QueryParams queryParams, string userId)
        {
            var filteredHotels = GetHotels()
                .Where(h => h.ApplicationUserId == userId);
            return await GetPaginatedHotels(filteredHotels, queryParams);
        }

        private IQueryable<Hotel> GetHotels()
        {
            return applicationContext.Hotels
                .Include(h => h.Location)
                .Include(h => h.Rooms)
                    .ThenInclude(r => r.RoomBeds)
                        .ThenInclude(rb => rb.Bed);
        }

        private async Task<PaginatedList<Hotel>> GetPaginatedHotels(IQueryable<Hotel> hotels, QueryParams queryParams)
        {
            var filteredHotels = FilterByParams(hotels, queryParams);
            var orderedHotels = QueryableUtils<Hotel>.OrderCustom(filteredHotels, queryParams);
            return await PaginatedList<Hotel>.CreateAsync(orderedHotels, queryParams.CurrentPage, queryParams.PageSize);
        }

        private IQueryable<Hotel> FilterByParams(IQueryable<Hotel> hotels, QueryParams queryParams)
        {
            return hotels
                .Where(h => h.Rooms
                    .Sum(r => r.RoomBeds
                    .Sum(rb => rb.Bed.Size * rb.BedNumber)) >= queryParams.GuestNumber
                    && (string.IsNullOrEmpty(queryParams.Search)
                    || h.Location.City.Contains(queryParams.Search)));
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

        public async Task<Review> AddReviewAsync(Review review)
        {
            await applicationContext.AddAsync(review);
            await applicationContext.SaveChangesAsync();
            return review;
        }

        public async Task DeleteReview(int reviewId)
        {
            var review = applicationContext.Reviews
                .SingleOrDefault(r => r.ReviewId == reviewId)
                ?? throw new ItemNotFoundException(localizer["Review with id: {0} is not found.", reviewId]);
            applicationContext.Reviews.Remove(review);
            await applicationContext.SaveChangesAsync();
        }

        public async Task DeleteReview(int reviewId, string userId)
        {
            var review = applicationContext.Reviews
                .SingleOrDefault(r => r.ReviewId == reviewId && r.ApplicationUserId == userId)
                ?? throw new ItemNotFoundException(localizer["Review with id: {0} is not found.", reviewId]);
            applicationContext.Reviews.Remove(review);
            await applicationContext.SaveChangesAsync();
        }

        public HotelsDTO GetHotelDTOs(PaginatedList<Hotel> paginatedHotels)
        {
            return new HotelsDTO
            {
                PageCount = paginatedHotels.TotalPages,
                CurrentPage = paginatedHotels.CurrentPage,
                Hotels = mapper.Map<PaginatedList<Hotel>, List<HotelDTO>>(paginatedHotels)
            };
        }
    }
}
