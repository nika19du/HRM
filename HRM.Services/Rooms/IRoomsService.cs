using HRM.Data.Models;
using HRM.Services.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Rooms
{
    public interface IRoomsService
    {
        Task CreateAsync(int Capacity, bool isItAvailable, string type, string image, decimal adultPrice, decimal childPrice,int number);
        Task ModifyAsync(RoomServiceModel room);
        Task DeleteAsync(string id);
        Task<bool> IsExistingAsync(string id);
        Task<TModel> GetByIdAsync<TModel>(string id);
        Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null );
        Task<bool> IsUsernameUsedAsync(string username); 
    }
}
