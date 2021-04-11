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

        /// <summary>
        /// Adds room to the database
        /// </summary> 
        /// <returns>Task representing the operation</returns>
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

        /// <summary>
        /// Removes room from the database
        /// </summary>
        /// The room to delete id</param>
        /// <returns>Task representing the operation</returns>
        public async Task DeleteAsync(string id)
        { 
            var room = await this.GetByIdAsync<Room>(id);
            this.context.Rooms.Remove(room);
            await this.context.SaveChangesAsync();
        }
        /// <summary>
        /// Finds all rooms that have the searched type
        /// </summary> 
        /// object or null if not found</returns>
        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null)
        {
           
            var queryable = this.context.Rooms
             .AsNoTracking();
            if (search != null)
            {
                var types = context.RoomTypes.FirstOrDefault(x => x.Name == search || x.Name.Contains(search));
                foreach (var roomType in context.RoomTypes)
                {
                    if (roomType.Name == search || roomType.Name.Contains(search))
                    {
                        if (queryable.Any(x => x.Type == roomType.Id))
                        {
                            queryable = queryable.Where(x => x.Type == roomType.Id || x.Type.Contains(roomType.Id));
                        }
                    }
                }
            }
            var rooms = await queryable.ProjectTo<TModel>(this.mapper.ConfigurationProvider).ToListAsync(); 
            return rooms;
        }
        //Find room by id.
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
        //Updating room class
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
