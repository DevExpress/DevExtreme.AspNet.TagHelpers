using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Samples.Models.Northwind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Samples {

    public class Startup {

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();

            services
                .AddLogging()
                .AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<NorthwindContext>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole();

            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();
            app.UseMvc(routes => routes.MapRoute(name: "default", template: "{controller=Widgets}/{action=DataGrid}"));
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }

}
