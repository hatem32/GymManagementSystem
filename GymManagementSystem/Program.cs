using GymManagementSystem.BLL;
using GymManagementSystem.BLL.Services.AttachmentService;
using GymManagementSystem.BLL.Services.Classes;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.PL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IMemberShipRepository, MemberShipRepository>();
            builder.Services.AddScoped<IMemberShipService, MemberShipService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Config =>
            {
                //Config.Password.RequiredLength = 6;
                //Config.Password.RequireLowercase = true;    
                //Config.Password.RequireUppercase = true;
                Config.User.RequireUniqueEmail = true;
                Config.Lockout.MaxFailedAccessAttempts = 5;
                Config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);

            }).AddEntityFrameworkStores<GymDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // redirect unauthenticated users (401)
                options.LoginPath = "/Account/Login";
                // redirect forbidden users (403)
                options.AccessDeniedPath = "/Account/AccessDenied";
            });// Default Paths

            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            await app.MigrateAndSeedAsync();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
