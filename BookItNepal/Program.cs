using BookIT.Application.Abstractions.Context;
using BookIT.Application.Abstractions.Repository;
using BookIT.Application.Abstractions.Services;
using BookIT.Application.Services;
using BookIT.Infrastructure.Services;
using BookIT.Persistence.Data;
using BookIT.Persistence.Repositories;
using BookItNepal.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookItNepal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<BookItDbContext>(options => options
                .UseNpgsql(builder.Configuration.GetConnectionString("BookIt")
                    ?? builder.Configuration["Database:ConnectionString"])
                .UseSnakeCaseNamingConvention());
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IOrganizationContext, OrganizationContext>();
            builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            builder.Services.AddScoped<IStaffRepository, StaffRepository>();
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IStaffService, StaffService>();
            builder.Services.AddScoped<IServiceManagementService, ServiceManagementService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPasswordHasher, PasswordService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT secret is not configured.");
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1)
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

            app.UseRouting();
            app.UseAuthentication();
            app.UseMiddleware<TenantResolutionMiddleware>();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
