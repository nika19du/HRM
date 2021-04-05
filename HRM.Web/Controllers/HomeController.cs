using HRM.Data;
using HRM.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly Context context;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(Context cnt, RoleManager<IdentityRole> role)
        {
            this.context = cnt;
            this.roleManager = role;
        }

        public async Task<IActionResult> Index()
        {
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

            return View();
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
