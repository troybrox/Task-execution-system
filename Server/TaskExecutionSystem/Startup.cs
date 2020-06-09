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
using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem
{
    // класс - точка входа программы
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // настройка сервисов
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

            //
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});


            services.AddControllers();

            services.AddTransient<IAccountService, AuthService>()
                .AddTransient<IAdminService, AdminService>()
                .AddTransient<ITaskService, TaskService>()
                .AddTransient<ITeacherService, TeacherService>()
                .AddTransient<IStudentService, StudentService>()
                .AddTransient<IRepoService, RepoService>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<IUserValidator<User>, UserValidator<User>>();
        }


        // основная настройка работы веб-приложения 
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

            //
            app.UseCookiePolicy();

            //app.UseCookiePolicy(new CookiePolicyOptions
            //{
            //    MinimumSameSitePolicy = SameSiteMode.None,
            //    HttpOnly = HttpOnlyPolicy.Always,
            //    Secure = CookieSecurePolicy.Always
            //});

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
