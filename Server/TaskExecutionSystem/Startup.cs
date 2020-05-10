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
using TaskExecutionSystem.Identity.JWT.Options;

namespace TaskExecutionSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // äîáàâëåíèå è íàñòðîéêà ñåðâèñîâ, èñïîëüçóåìûõ ïðèëîæåíèåì
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(optionsBuilder => 
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Connection")));

            var section = Configuration.GetSection("AuthOptions");
            var options = section.Get<AuthOptions>();
            var jwtOptions = new JWTOptions(options.Issuer, options.Audience, options.Secret, options.Lifetime);

            services.AddApiJwtAuthentication(jwtOptions);

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
                .AddTransient<ITeacherService, TeacherService>()
                .AddTransient<IStudentService, StudentService>()
                .AddTransient<IRepoService, RepoService>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.UseCors(MyAllowSpecificOrigins);
            app.UseSecureJwt();
            app.UseAuthentication();
            
            app.UseRouting();

            app.UseAuthorization();

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
