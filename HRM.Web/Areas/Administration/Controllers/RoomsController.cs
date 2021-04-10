using AutoMapper;
using HRM.Data;
using HRM.Data.Models;
using HRM.InputModel.Rooms;
using HRM.Services.Cloudinaries;
using HRM.Services.Rooms;
using HRM.Services.Rooms.Model;
using HRM.ViewModel.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace HRM.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class RoomsController : Controller
    {
        private readonly IRoomsService service;
        private readonly IMapper mapper;
        private ICloudinaryService cloudinaryService;
        private readonly Context context;
        public RoomsController(IRoomsService service, IMapper mapper, ICloudinaryService cloudinaryService, Context context)
        {
            this.service = service;
            this.mapper = mapper;
            this.cloudinaryService = cloudinaryService;
            this.context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            ViewData["Type"] = new SelectList(this.context.RoomTypes, "Id", "Name");
            return this.View(new RoomsCreateInputViewModel());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RoomsCreateInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Type"] = new SelectList(context.RoomTypes, "Id", "Name", model.Type);
                return this.View(model);
            }
            string pictureUrl = await this.cloudinaryService.UploadPictureAsync(model.Image, model.Type);

            await service.CreateAsync(model.Capacity, model.IsItAvailable, model.Type, pictureUrl, model.AdultPrice, model.ChildPrice, model.Number);
            return this.RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All(string submitBtn, string asc, string desc, string search = null, int? i = null)
        {
            RoomType data = new RoomType();
            List<Room> activeOrNo = new List<Room>();
            IEnumerable<RoomsInfoViewModel> roomsInfoViewModel = new List<RoomsInfoViewModel>();
            var types = context.RoomTypes.ToList();
            var rooms = await this.service.GetAllAsync<Room>(search);

            if (submitBtn == "NotActive" || submitBtn != null)
            {
                foreach (var item in rooms.Where(x => x.IsItAvailable == false))
                {
                    data = types.Where(x => x.Id == item.Type).FirstOrDefault();
                    item.RoomType = data;
                    activeOrNo.Add(item);
                }
                var map = this.mapper.Map(activeOrNo, roomsInfoViewModel);
                if (asc == "Ascending")
                {
                    map.OrderBy(x => x.Capacity);
                }
                else if (desc == "Descending")
                {
                    map.OrderByDescending(x => x.Capacity);
                }
                var vM = new RoomsAllViewModel
                {
                    Search = search,
                    Rooms = map
                };
                IPagedList<RoomsInfoViewModel> rmList = vM.Rooms.ToList().ToPagedList(i ?? 1, 10);
                vM.Rooms = rmList;
                submitBtn = "NotActive";
                return View(vM);
            }
            else if (submitBtn == null || submitBtn == "Active")
            {
                if (asc == "Ascending")
                   rooms= rooms.OrderBy(x => x.Capacity);
                if (desc == "Descending")
                    rooms = rooms.OrderByDescending(x=>x.Capacity);
                foreach (var room in rooms.Where(x => x.IsItAvailable == true))
                {
                    data = types.Where(x => x.Id == room.Type).FirstOrDefault();
                    room.RoomType = data;
                    activeOrNo.Add(room);
                }
            }
            var maping = this.mapper.Map(activeOrNo, roomsInfoViewModel);
             
            var viewModel = new RoomsAllViewModel
            {
                Search = search,
                Rooms = maping
            };
            IPagedList<RoomsInfoViewModel> roomList = viewModel.Rooms.ToList().ToPagedList(i ?? 1, 10);
            viewModel.Rooms = roomList;
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var room = await this.service.GetByIdAsync<Room>(id);
            var roomsCreateInputViewModel = new RoomsCreateInputViewModel()
            {
                AdultPrice = room.AdultPrice,
                ChildPrice = room.ChildPrice,
                Capacity = room.Capacity,
                IsItAvailable = room.IsItAvailable,
                Number = room.Number
            };
            ViewData["Type"] = new SelectList(context.RoomTypes, "Id", "Name");
            return View(roomsCreateInputViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(RoomsCreateInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                ViewData["Type"] = new SelectList(context.RoomTypes, "Id", "Name", model.Type);
                return this.View(model);
            }
            var id = context.Rooms.FirstOrDefault(x => x.RoomType.Name == model.Type.ToLower());
            string pictureUrl = null;
            RoomServiceModel roomServiceModel = new RoomServiceModel
            {
                Id = model.Id,
                Type = model.Type,
                AdultPrice = model.AdultPrice,
                IsItAvailable = model.IsItAvailable,
                ChildPrice = model.ChildPrice,
                Capacity = model.Capacity,
                Number = model.Number
            };
            if (model.Image != null)
            {
                pictureUrl = await this.cloudinaryService.UploadPictureAsync(model.Image, model.Type);
                roomServiceModel.Image = pictureUrl;
            }

            await service.ModifyAsync(roomServiceModel);
            return this.RedirectToAction(nameof(All));
        }
    }
}