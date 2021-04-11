using AutoMapper;
using HRM.Data;
using HRM.Data.Models;
using HRM.InputModel.Reservations;
using HRM.Services.Cloudinaries;
using HRM.Services.Reservations;
using HRM.Services.Rooms;
using HRM.ViewModel.Reservations;
using HRM.ViewModel.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

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
        private readonly UserManager<User> userManager;

        public ReservationsController(IReservationsService service, IMapper mapper, ICloudinaryService cloudinaryService, Context context, IRoomsService roomsService, UserManager<User> userManager)
        {
            this.service = service;
            this.mapper = mapper;
            this.cloudinaryService = cloudinaryService;
            this.context = context;
            this.roomsService = roomsService;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index(int? i = null)
        {
            var user = await userManager.GetUserAsync(User);
            var reservations = await service.GetReservationsForUser<ReservationViewModel>(user.Id);

            ReservationIndexViewModel model = new ReservationIndexViewModel()
            {
                Reservations = reservations
            };
            IPagedList<ReservationViewModel> reservationList = model.Reservations.ToList().ToPagedList(i ?? 1, 10);
            model.Reservations = reservationList;
            return View("~/Views/Reservations/Index.cshtml", model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Book(string Id)
        {
            var room = await this.roomsService.GetByIdAsync<Room>(Id);
            BookCreateInputViewModel viewModel = new BookCreateInputViewModel(); 
            var maping = this.mapper.Map(room, viewModel);
            TempData["Max"] = room.Capacity;

            if (TempData["CheckBooleans"] == null)
                TempData["CheckBooleans"] = "";

            if (TempData["CustomError"]==null)
            TempData["CustomError"] = "";
            return this.View("~/Views/Reservations/Book.cshtml",maping);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var reservation = await service.GetReservation<ReservationInputModel>(id);
            var user = await userManager.GetUserAsync(User);
            if (reservation == null || !User.IsInRole("Admin"))
            {
                this.NotFound();
            }

            await service.DeleteReservation(id);
            user.Reservations.Remove(reservation);
            await context.SaveChangesAsync();
            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Book(BookCreateInputViewModel booking)
        {
            if (ModelState.IsValid==false)
            {
                return this.BadRequest();
            }
            if(booking.IncludeBreakfast==true && booking.IsItAllInclusive == true)
            {
                TempData["CheckBooleans"]= "Select only one: all-inclusive or breakfast!";
                this.ModelState.AddModelError("Name", TempData["CheckBooleans"].ToString());
                return RedirectToAction("Book", "Reservations", new { Id = booking.Id });
            }
            int max = Convert.ToInt32(TempData["Max"]);
            if((booking.AddultCount ==max && max== booking.ChildCount) ||(booking.AddultCount==max && booking.ChildCount!=0))
            {
                TempData["CustomError"] = "Both child and adult cannot have a max value!";
                this.ModelState.AddModelError("Name", TempData["CustomError"].ToString()); 

                return RedirectToAction("Book", "Reservations", new { Id = booking.Id}); 
            }
            var user = await userManager.GetUserAsync(User);  
            await service.Reserve(booking.CheckIn,booking.CheckOut,booking.Id,user,booking.IncludeBreakfast,booking.IsItAllInclusive,booking.AddultCount,booking.ChildCount);

            return RedirectToAction("Index","Reservations");
        }
        //Details 
        public async Task<IActionResult> Details(string id)
        {
            var user = await userManager.GetUserAsync(User);
            var viewModel = await this.service.GetReservation<Reservation>(id);
            var room = await this.roomsService.GetByIdAsync<Room>(viewModel.RoomId);
            var type = context.RoomTypes.FirstOrDefault(x => x.Id == room.Type).Name;
            ViewData["RoomName"] = type.ToUpper();
            if (viewModel == null || user.Id != viewModel.CustomerId)
            {
                return this.NotFound();
            } 
            return this.View("~/Views/Reservations/Details.cshtml",viewModel);
        } 
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await userManager.GetUserAsync(User);

            var reservation = await service.GetReservation<Reservation>(id);
            if (reservation == null || !(user.Id == reservation.CustomerId || User.IsInRole("Admin")))
            {
                return this.NotFound();
            } 
            var room = await roomsService.GetByIdAsync<Room>(reservation.RoomId);
            var roomIsEmpty = await service.AreDatesAcceptable(room.Id, reservation.CheckIn, reservation.CheckOut);
            if (!roomIsEmpty)
            {
                this.ModelState.AddModelError(nameof(reservation.CheckIn), "Room is already reserved at that time");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(reservation);
            }
            reservation.Customer = user;
            reservation.CustomerId = user.Id; 
            reservation.RoomId = room.Id;
            reservation.Room = room;
            TempData["name"] = room.Type.ToString(); 
            return this.View("~/Views/Reservations/Update.cshtml", reservation);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Reservation reservation) 
        {
            var user = await userManager.GetUserAsync(User);

            var success = await service.UpdateReservation(reservation.Id,reservation.CheckIn, reservation.CheckOut,reservation.IsItAllInclusive,reservation.IncludeBreakfast,reservation.AddultCount, reservation.ChildCount,user);

            if (!success)
            {
                ModelState.AddModelError("", "Error updating room");
                return this.View(reservation);
            } 
            return this.RedirectToAction("Details", new { id = reservation.Id });
        } 
    }
}
