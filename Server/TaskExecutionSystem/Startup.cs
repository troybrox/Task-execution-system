using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TaskExecutionSystem.Application.Initialization;
using TaskExecutionSystem.Application.Options;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.BLL.Services;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.Identity.JWT.Extensions;

namespace TaskExecutionSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // äîáàâëåíèå è íàñòðîéêà ñåðâèñîâ, èñïîëüçóåìûõ ïðèëîæåíèåì
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(optionsBuilder => 
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Connection")));

            services.AddApiJwtAuthentication();
            services.Configure<SeedOptions>(Configuration.GetSection("Seed"));
            services.AddAsyncInitializer<IdentityInitializer>()
                .AddAsyncInitializer<StudyDataInitializer>();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("https://localhost:3000", "http://localhost:3000", "https://localhost:44303")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            services.AddControllers();

            services.AddTransient<IAccountService, AuthService>()
                .AddTransient<IAdminService, AdminService>()
                .AddTransient<ITaskService, TaskService>()
                .AddTransient<ITaskService, TaskService>();
                //.AddTransient<ITeacherService, TeacherService>();
        }


        // äàííûé ìåòîäîì âûçûâàåòñÿ ïðè çàïóñêå, èñïîëüçóåòñÿ äëÿ íàñòðîéêè êîíôèãóðàöèè êîíâåéåðà http çàïðîñîâ 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseAuthorization();

            app.UseCookiePolicy(new CookiePolicyOptions 
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Could Not Find Anything");
            });
        }
    }
}
