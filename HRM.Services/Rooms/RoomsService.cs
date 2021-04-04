using AutoMapper;
using HRM.Data;
using HRM.Data.Models;
using HRM.Services.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace HRM.Services.Rooms
{
    public class RoomsService : IRoomsService
    {
        private readonly Context context;
        private readonly IMapper mapper; 
        public RoomsService(Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task CreateAsync(int capacity, bool isItAvailable, string type, string image, decimal adultPrice, decimal childPrice, int number)
        { 
            Room newRoom = new Room()
            {
                Id = Guid.NewGuid().ToString(),
                Type=type,
                Capacity=capacity,
                IsItAvailable=isItAvailable,
                Image=image,
                AdultPrice=adultPrice,
                ChildPrice=childPrice,
                Number=number 
            };
            context.Rooms.Add(newRoom);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        { 
            var room = await this.GetByIdAsync<Room>(id);
            this.context.Rooms.Remove(room);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null)
        {
            var queryable = this.context.Rooms
             .AsNoTracking();

            if (!String.IsNullOrWhiteSpace(search))
            {
                queryable = queryable.Where(x => x.Type == search);
            } 
            var rooms = await queryable.ProjectTo<TModel>(this.mapper.ConfigurationProvider).ToListAsync(); 
            return rooms;
        }

        public async Task<TModel> GetByIdAsync<TModel>(string id)
        {
            return await this.context.Rooms
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistingAsync(string id)
              => await this.context.Rooms.AnyAsync(x => x.Id == id);

        public async Task<bool> IsUsernameUsedAsync(string username)
             => await this.context.Rooms.AnyAsync(u => u.Type == username);

        public async Task ModifyAsync(RoomServiceModel room)
        {
            var data = await context.Rooms.FirstOrDefaultAsync(x => x.Id == room.Id); 
            Room updatedRoom = new Room();
            if (room.Image != null)
            {
                updatedRoom = this.mapper.Map(room, data);
                context.Rooms.Update(updatedRoom);
            }
            else
            { 
                data.Capacity = room.Capacity;
                data.AdultPrice = room.AdultPrice;
                data.ChildPrice = room.ChildPrice;
                data.IsItAvailable = room.IsItAvailable;
                data.Number = room.Number;
                data.Type = room.Type;
                data.RoomType = room.RoomType;
                context.Rooms.Update(data);
            }
            await context.SaveChangesAsync();
        }
    }
}
