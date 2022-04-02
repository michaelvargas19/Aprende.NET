using Introduccion.NET.Ejercicio.Business.Contracts;
using Introduccion.NET.Ejercicio.Business.Implementation;
using Introduccion.NET.Ejercicio.Common.Contracts.Middle;
using Introduccion.NET.Ejercicio.Common.Implement.Middle;
using Introduccion.NET.Ejercicio.Repository;
using Introduccion.NET.Ejercicio.Repository.Contracts;
using Introduccion.NET.Ejercicio.Repository.Implement;
using Introduccion.NET.Ejercicio.Service.Contracts;
using Introduccion.NET.Ejercicio.Service.Implement;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Introduccion.NET.Ejercicio.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });
        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connection = config["ConnectionString:PracticaDB"];

            services.AddDbContext<PracticaDBContext>(options =>
                options.UseSqlServer(connection, optionsSQL => { optionsSQL.CommandTimeout(90000); })
            );

        }

        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration config)
        {
            
            return services;
        }


        public static void ConfigureInterfaces(this IServiceCollection services)
        {

            //Commons
            services.AddScoped<ISenderManager, SenderManager>();
            services.AddScoped<IAuditManager, AuditManager>();
            services.AddSingleton<ISecurityManager, SecurityManager>();

            //Services
            services.AddScoped<ICalculatorService, CalculatorService>();

            //Process
            services.AddScoped<ICalculatorProcess, CalculatorProcess>();

            //Repository
            services.AddScoped<ICalculatorRepository, CalculatorRepository>();

        }

        public static void AddSwagger(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            var enviroment = env.EnvironmentName;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"EBL-backend.core in {enviroment}", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
                                
            });

        }
        
        public static void AddServiceFactory(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            
            var servicespath = config.GetSection("BaseAddress").GetChildren();
            //leerlos del config.
            foreach (IConfigurationSection path in servicespath)
            {
                services.AddHttpClient(path.Key, client => { client.BaseAddress = new Uri(path.Value); });
            }
            
            services.AddHttpContextAccessor();
            services.AddResponseCaching();

        }


    }
}
