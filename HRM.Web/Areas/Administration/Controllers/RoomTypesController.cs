using HRM.InputModel.RoomTypes;
using HRM.Services.RoomTypes;
using HRM.ViewModel.RoomTypes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Web.Areas.Administration.Controllers
{
    public class RoomTypesController : AdminController
    {
        private readonly IRoomTypesService service;

        public RoomTypesController(IRoomTypesService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> All(string search = null)
        {
            var types = await this.service.GetAllAsync<RoomTypesInfoViewModel>(search);
            var viewModel = new RoomTypesAllViewModel
            {
                Search = search,
                Types = types
            };
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomTypesCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            await this.service.CreateAsync(model.Name);
            return this.RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var category = await this.service.GetByIdAsync<RoomTypesEditInputModel>(id);

            if (category == null)
            {
                return this.NotFound();
            }
            return this.View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoomTypesEditInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.service.EditAsync(model.Id, model.Name);
            return this.RedirectToAction("All", "RoomTypes", new { id = model.Id, area = "Administration" });
        }
         
        public async Task<IActionResult> Delete(string id)
        {//when i delete type of room also is deleted a room with this type.:)
            var isExisting = await this.service.IsExistingAsync(id);
            if (isExisting == false)
            {
                return this.NotFound();
            }
            await this.service.DeleteAsync(id);
            return this.RedirectToAction(nameof(All));
        }
    }
}
