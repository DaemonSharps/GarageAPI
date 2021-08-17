using GarageAPI.DataBase;
using GarageAPI.Services;
using GarageAPI.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI
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
            services.AddControllers();

            services.AddDbContext<GarageDBContext>(builder =>
                    builder.UseMySql(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

            services.AddTransient<IRecordsService, RecordsService>();

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(
                    "v1.0",
                    new OpenApiInfo
                    {
                        Title = "Garage API",
                        Version = "1.0"
                    });
                setup.CustomSchemaIds(type => type.ToString());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseRouting();
            app.UseSwaggerUI(opt => 
            {
                opt.SwaggerEndpoint("/swagger/v1.0/swagger.json","Garage API");
                opt.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
