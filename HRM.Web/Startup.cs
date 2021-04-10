using CloudinaryDotNet;
using HRM.Data;
using HRM.Data.Models;
using HRM.Services.Cloudinaries;
using HRM.Services.Mapping;
using HRM.Services.Providers;
using HRM.Services.Reservations;
using HRM.Services.Rooms;
using HRM.Services.RoomTypes;
using HRM.Services.Users;
using HRM.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection; 

namespace HRM.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(options =>
           options.UseSqlServer(
               Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("HRM.Web")));

            services.AddIdentity<User, IdentityRole>
           (options =>
           {
               options.Password.RequiredLength = 3;
               options.Password.RequiredUniqueChars = 0;
               options.Password.RequireLowercase = false;
               options.Password.RequireNonAlphanumeric = false;
               options.Password.RequireUppercase = false;
           })
           .AddEntityFrameworkStores<Context>()
           .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider)
               .AddDefaultUI()
               .AddEntityFrameworkStores<Context>();

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AppProfile());
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            var emailConfig = Configuration
              .GetSection("EmailConfiguration")
              .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.AddScoped<Services.Providers.IEmailSender, EmailSender>();
            services.AddTransient<IRoomTypesService, RoomTypesService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IRoomsService, RoomsService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IReservationsService,ReservationsService>();

            Account cloudinaryCredentials = new Account(
               this.Configuration["Cloudinary:CloudName"],
               this.Configuration["Cloudinary:ApiKey"],
               this.Configuration["Cloudinary:ApiSecret"]);
            Cloudinary cloudinaryUtility = new Cloudinary(cloudinaryCredentials); 
            services.AddSingleton(cloudinaryUtility);

            services.AddAuthentication();
            services.AddRazorPages();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider
                      .GetRequiredService<Context>();

                context.Database.EnsureCreated();
            }
                AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                           "forumCategory",
                           "f/{name:minlength(3)}",
                           new { controller = "RoomTypes", action = "ByName" });
                endpoints.MapControllerRoute(
                    "areaRoute",
                    "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute(
                //   name: "default",
                //   pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
