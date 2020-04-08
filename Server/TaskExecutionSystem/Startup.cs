using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using TaskExecutionSystem.Application.Initialization;
using TaskExecutionSystem.Application.Options;
using TaskExecutionSystem.Identity;
using static TaskExecutionSystem.Identity.Contracts.IdentityPolicyContract;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.BLL.Services;
using TaskExecutionSystem.DAL.Entities;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Data;

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

        // добавление и настройка сервисов, используемых приложением
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(optionsBuilder => optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<SeedOptions>(Configuration.GetSection("Seed"));

            services.AddAsyncInitializer<IdentityInitializer>();

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

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();


            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy(TeacherUserPolicy, policy => policy.RequireRole(nameof(Role.Types.Teacher)));
                options.AddPolicy(StudentUserPolicy, policy => policy.RequireRole(nameof(Role.Types.Student)));
                options.AddPolicy(AdministratorPolicy, policy => policy.RequireRole(nameof(Role.Types.Administrator)));
            });

            services.AddTransient<IAccountService, AccountService>();
        }


        // данный методом вызывается при запуске, используется для настройки конфигурации конвейера http запросов 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCookiePolicy(new CookiePolicyOptions 
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            } );

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
