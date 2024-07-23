using AFSolution.Domain.Interfaces;
using AFSolution.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AFSolution.Infrastructure.Repositories;
using AFSolution.Application.Interfaces;
using AFSolution.Application.Services;
using AFSolution.Domain.Entities;
using AFSolution.Application.Mappings;
using AFSolution.Application.DTOs;

namespace AFSolution.Api
{
    public static class Setup
    {
        public static void AddRegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            builder.Services.AddScoped<IBaseRepository<User>, UserRepository>();
            builder.Services.AddScoped<IBaseService<UserDTO>, UserService>();
            //builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            //builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void AddSignalR(this WebApplicationBuilder builder)
        {
            // builder.Services.AddSignalR();
        }

        public static void AddEntityFrameworkServices(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("AFSolutionDb");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        public static void AddAutoMapperServices(this WebApplicationBuilder builder)
        {
            // builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly); // Registra AutoMapper e seus perfis

        }

        public static void AddJwtBearerSecurity(this WebApplicationBuilder builder)
        {
             //builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            // builder.Services.AddTransient<IAuthorizationSecurity, AuthorizationSecurity>();

            builder.Services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtSettings").GetSection("SecretKey").Value ?? "")
                    ),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
        }

        public static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AF Solution API",
                    Description = ""
                });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
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

        public static void AddCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(s => s.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyOrigin() // qualquer origem pode acessar a API
                       .AllowAnyMethod() // qualquer método (POST, PUT, DELETE, GET)
                       .AllowAnyHeader(); // qualquer informação de cabeçalho
            }));
        }

        public static void UseCors(this WebApplication app)
        {
            app.UseCors("DefaultPolicy");
        }
    }
}
