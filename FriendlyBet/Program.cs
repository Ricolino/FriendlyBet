using FriendlyBet.Data;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FriendlyBet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
                    facebookOptions.AccessDeniedPath = "/Identity/Account/AccessDeniedPathInfo";

                });
            //             .AddGoogle(options =>
            //{
            //    IConfigurationSection googleAuthNSection =
            //    config.GetSection("Authentication:Google");
            //    options.ClientId = googleAuthNSection["ClientId"];
            //    options.ClientSecret = googleAuthNSection["ClientSecret"];
            //})

            //.AddMicrosoftAccount(microsoftOptions =>
            //{
            //    microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
            //    microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
            //})
            //.AddTwitter(twitterOptions =>
            //{
            //    twitterOptions.ConsumerKey = config["Authentication:Twitter:ConsumerAPIKey"];
            //    twitterOptions.ConsumerSecret = config["Authentication:Twitter:ConsumerSecret"];
            //    twitterOptions.RetrieveUserDetails = true;
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}