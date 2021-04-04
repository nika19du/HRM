using HRM.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRM.Services.Users.Model;

namespace HRM.Services.Users
{
    public interface IUsersService
    {
        Task CreateAsync(string username, string EGN, string phoneNumber, string email, string password, string confirmPassword,
            string firstName, string middleName, string surname,bool isItActive);
        Task ModifyAsync(UserServiceModel user);
        Task DeleteAsync(string id);
        Task<bool> IsExistingAsync(string id);
        Task<TModel> GetByIdAsync<TModel>(string id); 
        Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null);
        Task<bool> IsUsernameUsedAsync(string username);
    }
}
