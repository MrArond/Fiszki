using API.DATA.Context;
using API.Repositories.Implementation;
using API.Repositories.Interfaces;
using API.Services;
using API.Services.Implementations;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace API
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Rejestracja serwisów
            builder.Services.AddTransient<IForgotService, ForgotService>();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<ICardsRepository, CardsRepository>();
            builder.Services.AddTransient<ICardsService, CardsService>();
            builder.Services.AddTransient<ICardsListService, CardsListService>();
            builder.Services.AddTransient<IForgotRepository, ForgotRepository>();
            builder.Services.AddTransient<IAuthRepository, AuthRepository>();
            builder.Services.AddTransient<ICardsListRepository, CardsListRepository>();
            builder.Services.AddDbContext<Datacontext>();
            
            // JwtServices generuje tokeny - musi u¿ywaæ TEGO SAMEGO klucza co validacja
            builder.Services.AddScoped<JwtServices>();

            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            
            // Swagger z JWT
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            // ===== JWT CONFIGURATION =====
            var jwtSecret = builder.Configuration["JWT_SECRET"];
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new InvalidOperationException("JWT_SECRET is not configured");
            }

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
                    ValidAudience = builder.Configuration["Authentication:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                };
            });



            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            app.UseAuthentication();  
            app.UseAuthorization();  

            app.MapControllers();

            app.Run();
        }
    }
}
