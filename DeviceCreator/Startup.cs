using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceCreator.Pages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceCreator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var hostName = this.Configuration["iotHost"];

            var connectionString = this.Configuration["iotConnection"];

            var registryManager = RegistryManager.CreateFromConnectionString(connectionString);

            services.AddSingleton(new IoTConfig { HostName = hostName, RegistryManager = registryManager });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
