using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.RoomTypes
{
    public interface IRoomTypesService
    {
        Task CreateAsync(string name);

        Task EditAsync(string id, string name);

        Task DeleteAsync(string id);

        Task<bool> IsExistingAsync(string name);

        Task<TModel> GetByIdAsync<TModel>(string id);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null);
    }
}
