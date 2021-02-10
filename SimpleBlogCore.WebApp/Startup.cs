using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleBlogCore.DataProvider;
using SimpleBlogCore.DataProvider.Identity;
using SimpleBlogCore.DataProvider.Repositories;
using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.Domain.Interfaces;
using System;

namespace SimpleBlogCore.WebApp
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
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.Configure<RazorViewEngineOptions>(options => {
                options.ViewLocationExpanders.Add(new ExtendedViewEngine());
            });

            services.AddDbContext<SimpleBlogDbContext>(options => {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<SimpleBlogDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IUserClaimsPrincipalFactory<User>, AppClaimsPrincipalFactory>();

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfAsyncBaseRepository<>));
            services.AddScoped(typeof(IPostAsyncRepository), typeof(PostAsyncRepository));
            services.AddScoped(typeof(ICommentAsyncRepository), typeof(CommentAsyncRepository));

            ConfigurateIdentity(services);
        }

        private void ConfigurateIdentity(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Home/NoAccess";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "defaultArea",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            SimpleBlogContextSeedInitializer.ApplySeed(serviceProvider);
        }
    }
}
