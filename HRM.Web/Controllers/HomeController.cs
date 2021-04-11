using HRM.Data;
using HRM.Data.Models;
using HRM.Services.Rooms;
using HRM.ViewModel.Rooms;
using HRM.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace HRM.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly Context context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IRoomsService roomService;
        public HomeController(Context cnt, RoleManager<IdentityRole> role, UserManager<User> userManager,IRoomsService roomService)
        {
            this.context = cnt;
            this.roleManager = role;
            this.userManager = userManager;
            this.roomService = roomService;
        }
        
        //showing in index page info about rooms 
        public async Task<IActionResult> Index(string search=null, int? i = null)
        {
            //set-up user roles, for only one time
            IdentityRole userRole = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Client"
            };
            IdentityRole adminRole = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin"
            };
            await this.roleManager.CreateAsync(userRole);
            await this.roleManager.CreateAsync(adminRole);
            ViewData["UserCount"] =context.Users.Count();
            ViewData["RoomCount"] = context.Rooms.Count();
            ViewData["RoomTypesCount"] =context.RoomTypes.Count();

            IEnumerable<RoomsInfoViewModel> rooms = await this.roomService.GetAllAsync<RoomsInfoViewModel>(search);
            var viewModel = new RoomsAllViewModel
            {
                Search = search,
                Rooms = rooms.Where(x=>x.IsItAvailable==true)
            };
            IPagedList<RoomsInfoViewModel> roomList = viewModel.Rooms.ToList().ToPagedList(i ?? 1, 6);
            viewModel.Rooms = roomList;
            return View(viewModel); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
