using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDo.Data;
using ToDo.Repositories;

namespace ToDo
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
            services.AddDbContext<ToDoContext>(options =>
                  options.UseSqlite("Data Source=todo.db"));

            services.AddTransient<ITaskRepository, TaskRepository>();

            

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/ToDo/Error");
                app.UseHsts();
            }
            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
            if (HybridSupport.IsElectronActive)
            {
                ElectronStatup();
            }
        }

        private async void ElectronStatup()
        {
            CreateMenu();
            
            var window = await Electron.WindowManager.CreateWindowAsync(
                new BrowserWindowOptions
                {
                    Width = 800,
                    Height = 600,
                    Show = false
                }
            );  
            
            await window.WebContents.Session.ClearCacheAsync();

            window.OnReadyToShow += () => window.Show();
            window.SetTitle("Exemplo do Electron.NET");

            window.OnClosed += () => {  
                Electron.App.Quit();  
            };  
        }

        private void CreateMenu()  
        {  
            bool isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);  
            MenuItem[] menu = null;

            MenuItem[] appMenu = new MenuItem[]  
            {  
                new MenuItem { Role = MenuRole.about },  
                new MenuItem { Type = MenuType.separator },  
                new MenuItem { Role = MenuRole.services },  
                new MenuItem { Type = MenuType.separator },  
                new MenuItem { Role = MenuRole.quit }  
            };

            MenuItem[] fileMenu = new MenuItem[]  
            {  
                new MenuItem { Role = isMac ? MenuRole.close : MenuRole.quit }  
            };

            MenuItem[] viewMenu = new MenuItem[]  
            {  
                new MenuItem { Role = MenuRole.reload },  
                new MenuItem { Role = MenuRole.forcereload },  
                new MenuItem { Role = MenuRole.toggledevtools },  
                new MenuItem { Type = MenuType.separator },  
                new MenuItem { Role = MenuRole.resetzoom },  
                new MenuItem { Type = MenuType.separator },  
                new MenuItem { Role = MenuRole.togglefullscreen }  
            };  

            if (isMac)  
            {  
                menu = new MenuItem[]  
                {  
                    new MenuItem { Label = "Electron", Type = MenuType.submenu, Submenu = appMenu },  
                    new MenuItem { Label = "File", Type = MenuType.submenu, Submenu = fileMenu },  
                    new MenuItem { Label = "View", Type = MenuType.submenu, Submenu = viewMenu }  
                };  
            }  
            else  
            {  
                menu = new MenuItem[]  
                {  
                    new MenuItem { Label = "File", Type = MenuType.submenu, Submenu = fileMenu },  
                    new MenuItem { Label = "View", Type = MenuType.submenu, Submenu = viewMenu }  
                };  
            }

            Electron.Menu.SetApplicationMenu(menu);  
        }
    }
}
