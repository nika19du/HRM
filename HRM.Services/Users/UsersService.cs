using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRM.Data;
using HRM.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Services.Users.Model;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HRM.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly Context context;
        private readonly IMapper mapper;
        private readonly UserManager<User> _userManager;

        public UsersService(Context context,IMapper mapper, UserManager<User> _userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this._userManager = _userManager;
        }

        public async Task CreateAsync(string username,string EGN,string phoneNumber,string email,string password,string confirmPassword,
            string firstName,string middleName,string surname,bool isItActive)
        {
            User newUser = new User()
            {
                Id=Guid.NewGuid().ToString(),
                UserName=username,
                EGN=EGN,
                Email=email,
                FirstName=firstName,
                MiddleName=middleName,
                Surname=surname, 
                IsItActive=isItActive,
                NominationDate=DateTime.UtcNow
            }; 
            var result = await _userManager.CreateAsync(newUser, password);
            if(result.Succeeded)
            await _userManager.AddToRoleAsync(newUser, "Client"); 
        }

        public async Task DeleteAsync(string id)
        {
            var user = await this.GetByIdAsync<User>(id);
            this.context.Users.Remove(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null)
        {
            var queryable = this.context.Users
              .AsNoTracking();

            if (!String.IsNullOrWhiteSpace(search))
            {
                queryable = queryable.Where(x => x.UserName.Contains(search) || x.FirstName.Contains(search)|| x.MiddleName.Contains(search) || x.Surname.Contains(search) || x.Email.Contains(search));
            }

            var users = await queryable.ProjectTo<TModel>(this.mapper.ConfigurationProvider).ToListAsync();

            return users;
        }

        public async Task<TModel> GetByIdAsync<TModel>(string id)
        {
            return await this.context.Users
                 .AsNoTracking()
                 .Where(x => x.Id == id)
                 .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync();
        }

        public async Task ModifyAsync(UserServiceModel user)
        {
            var existUsr = context.Users.FirstOrDefault(x=>x.Id==user.Id);
            var usr= mapper.Map(user,existUsr);
            context.Users.Update(usr);
            await context.SaveChangesAsync(); 
            await context.SaveChangesAsync(); 
        }
        public async Task<bool> IsUsernameUsedAsync(string username)
          => await this.context.Users.AnyAsync(u => u.UserName == username);
        public async Task<bool> IsExistingAsync(string id)
        => await this.context.RoomTypes.AnyAsync(x => x.Id == id);
    }
}
