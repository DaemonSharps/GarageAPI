using ExternalApiClients.Extentions;
using GarageAPI.Options;
using GarageDataBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;

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
        services.AddRouting(ops => ops.LowercaseUrls = true);
#if DEBUG
        services.AddDbContext<GarageDBContext>(builder => builder.UseInMemoryDatabase("Garage_DB_Test"));
#else
        services.AddDbContext<GarageDBContext>(builder =>
                builder.UseNpgsql(Environment.GetEnvironmentVariable("GARAGE_DB_CONNECTION_STRING"),
                    b => b.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name)));
#endif

        services.AddCors();

        services.AddSwaggerGen(setup =>
        {
            setup.EnableAnnotations();
            setup.SwaggerDoc(
                "v1.0",
                new OpenApiInfo
                {
                    Title = "Garage API",
                    Version = "1.0"
                });
            setup.CustomSchemaIds(type => type.ToString());

            var basePath = AppContext.BaseDirectory + "/Documentation";
            var filePath = Path.Combine(basePath, "GarageApiDocumentation.xml");
            setup.IncludeXmlComments(filePath);
            filePath = Path.Combine(basePath, "GarageDataBaseDocumentation.xml");
            setup.IncludeXmlComments(filePath);

            setup.DescribeAllParametersInCamelCase();
            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Description = "Please insert JWT token into field"
            });
            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddJwtProviderClient();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        var tokenOptions = Configuration.GetSection(TokenOptions.Section).Get<TokenOptions>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(cfg => cfg.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512 },
                ValidateIssuer = true,
                ValidIssuer = tokenOptions.Issuer,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.AccessKey))
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


        app.UseAuthentication();
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
