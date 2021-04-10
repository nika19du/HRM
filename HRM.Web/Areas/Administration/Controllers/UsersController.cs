using AutoMapper;
using HRM.Data.Models;
using HRM.InputModel.Users;
using HRM.Services.Users;
using HRM.Services.Users.Model; 
using HRM.ViewModel.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace HRM.Web.Areas.Administration.Controllers
{
    public class UsersController : AdminController
    {
        private readonly IUsersService service;
        private readonly IMapper mapper;

        public UsersController(IUsersService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UsersCreateInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            await service.CreateAsync(model.UserName,model.EGN,model.PhoneNumber,model.Email,model.Password,model.ConfirmPassword,model.FirstName,model.MiddleName,model.Surname,model.IsItActive,model.IsItOld);
            return this.RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All(string search = null,int?i=null)  
        {
            var users = await this.service.GetAllAsync<UsersInfoViewModel>(search);
            var viewModel = new UsersAllViewModel
            {
                Search = search,
                Users=users
            };
            IPagedList<UsersInfoViewModel> usrList = viewModel.Users.ToList().ToPagedList(i ?? 1,10);
            viewModel.Users = usrList;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var isExisting = await this.service.IsExistingAsync(id);
            if (isExisting == false)
            {
                return this.NotFound();
            }
            await this.service.DeleteAsync(id);
            return this.RedirectToAction(nameof(All));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.service.GetByIdAsync<User>(id);
            var mappedItem = mapper.Map<UsersInfoViewModel>(user);
            return View(mappedItem);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UsersInfoViewModel model)
        {
            if (!this.ModelState.IsValid)
            { 
                return this.View(model);
            }
            UserServiceModel usrServiceModel = new UserServiceModel(); 
            var mappedItem= mapper.Map(model,usrServiceModel);
            await this.service.ModifyAsync(mappedItem);
            return this.RedirectToAction("Details", "Users", new { id = model.Id, area = "Administration" });
        }
        public async Task<IActionResult> Details(string id)
        {
            var user = await service.GetByIdAsync<User>(id);
            var mappedItem = mapper.Map<UsersInfoViewModel>(user);
            return View(mappedItem);
        }
    }
}
