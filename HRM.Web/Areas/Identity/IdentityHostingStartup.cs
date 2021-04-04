using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(HRM.Web.Areas.Identity.IdentityHostingStartup))]
namespace HRM.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}