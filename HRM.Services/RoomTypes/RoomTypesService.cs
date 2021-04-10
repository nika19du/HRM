using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRM.Data;
using HRM.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.RoomTypes
{
    public class RoomTypesService : IRoomTypesService
    {
        private readonly Context context;
        private readonly IMapper mapper;
        public RoomTypesService(Context context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task CreateAsync(string name)
        {
            RoomType roomType = new RoomType()
            {
                Id = Guid.NewGuid().ToString(),
                Name=name
            };
            context.RoomTypes.Add(roomType);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var type =await this.GetByIdAsync<RoomType>(id); 
            foreach (var room in context.Rooms)
            {
                if (room.Type==type.Id)
                {
                    context.Rooms.Remove(room);
                }
            }
            this.context.RoomTypes.Remove(type);
            await this.context.SaveChangesAsync();
        }

        public async Task EditAsync(string id, string name)
        {
            var type =await this.GetByIdAsync<RoomType>(id);
            type.Name = name;
            await context.SaveChangesAsync(); 
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null)
        {
            var queryable = this.context.RoomTypes
               .AsNoTracking();

            if (!String.IsNullOrWhiteSpace(search))
            {
                queryable = queryable.Where(x => x.Name.Contains(search));
            }

            var roomTypes = await queryable.ProjectTo<TModel>(this.mapper.ConfigurationProvider).ToListAsync();

            return roomTypes;
        }

        public async Task<TModel> GetByIdAsync<TModel>(string id)
        {
            return await this.context.RoomTypes
               .AsNoTracking()
               .Where(x => x.Id == id)
               .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
               .FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistingAsync(string id)
          => await this.context.RoomTypes.AnyAsync(x => x.Id == id);
    }
}
