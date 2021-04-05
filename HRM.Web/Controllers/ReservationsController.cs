using AutoMapper;
using HRM.Data;
using HRM.Data.Models;
using HRM.InputModel.Reservations;
using HRM.Services.Cloudinaries;
using HRM.Services.Reservations;
using HRM.Services.Rooms;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Web.Controllers
{
    [Area("Home")]
    public class ReservationsController : Controller
    {
        private readonly IReservationsService service;
        private readonly IMapper mapper;
        private ICloudinaryService cloudinaryService;
        private readonly Context context;
        private readonly IRoomsService roomsService;
        public ReservationsController(IReservationsService service, IMapper mapper, ICloudinaryService cloudinaryService, Context context, IRoomsService roomsService)
        {
            this.service = service;
            this.mapper = mapper;
            this.cloudinaryService = cloudinaryService;
            this.context = context;
            this.roomsService = roomsService;
        }
        [HttpGet]
        public async Task<IActionResult> Book(string Id)
        {
            var room = await this.roomsService.GetByIdAsync<Room>(Id);
            BookCreateInputViewModel viewModel = new BookCreateInputViewModel();

            var maping = this.mapper.Map(room, viewModel); 
            //{
            //    Id = roomId,
            //    AdultPrice = room.AdultPrice,
            //    ChildPrice = room.ChildPrice,
            //    Number = room.Number,
            //    Type = room.RoomType.Name
            //};
            return this.View("~/Views/Reservations/Book.cshtml",maping);
        }
        [HttpPost]
        public async Task<IActionResult> Book(BookCreateInputViewModel booking)
        {
            return Redirect(nameof(Index));
        }
    }
}
