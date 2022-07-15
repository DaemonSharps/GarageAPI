using GarageAPI.Services;
using GarageAPI.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using GarageDataBase;

namespace GarageAPI;

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
#if DEBUG
        services.AddDbContext<GarageDBContext>(builder => builder.UseInMemoryDatabase("Garage_DB_Test"));
#else
        services.AddDbContext<GarageDBContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),
                b => b.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name)));
#endif

        services.AddTransient<IRecordsService, RecordsService>();
        services.AddTransient<ICustomerService, CustomerService>();

        services.AddCors();

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
            var filePath = Path.Combine(AppContext.BaseDirectory, "GarageApiDocumentation.xml");
            setup.IncludeXmlComments(filePath);
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
#if DEBUG
        using (var scope = app.ApplicationServices.CreateScope())
        using (var context = scope.ServiceProvider.GetRequiredService<GarageDBContext>())
            context.Database.EnsureCreated();
#endif

        app.UseSwagger();
        app.UseRouting();
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Garage API");
            opt.RoutePrefix = string.Empty;
        });

        app.UseAuthorization();
        app.UseCors(options =>
        {
            options
            .WithOrigins(Configuration.GetSection("Cors:Origins").Get<string[]>())
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}
