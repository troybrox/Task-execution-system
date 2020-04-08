using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.Application.Initialization
{
    public class IdentityInitializer : DataInitializerBase
    {
        public IConfiguration Configuration { get; }
        private readonly UserManager<User> _userManager;

        public IdentityInitializer(IServiceProvider serviceProvider, IConfiguration configuration, UserManager<User> userManager)
            : base(serviceProvider)
        {
            Configuration = configuration;
            _userManager = userManager;
        }

        // инициализация ролей 
        protected override async Task InitializeAsync(DataContext context)
        {
            var roleNames = Enum
                .GetNames(typeof(Role.Types));

            var existingRoles = context
                .Roles
                .Select(x => x.Name)
                .ToArray();

            if (roleNames.Length != existingRoles.Length)
            {
                var newRoles = roleNames
                    .Where(role => existingRoles.All(x => x != role))
                    .Select(x => new Role(x)
                    {
                        NormalizedName = x.ToUpperInvariant()
                    })
                    .ToList();

                await context.AddRangeAsync(newRoles);
            }

            await InitializeAdmin();
        }

        // инициализация адиминистратора | при запуске приложения проверяется его существование, если нет - добавляется
        // пароль и логин админа в appsettings.json
        public async Task InitializeAdmin()
        {
            string adminUserName = Configuration.GetSection("AdminAuthOptions")["UserName"];
            var user = await _userManager.FindByNameAsync(adminUserName);

            if (user == null)
            {
                var administrator = new User
                {
                    UserName = adminUserName,
                };
                string adminPassword = Configuration.GetSection("AdminAuthOptions")["UserPassword"];
                var createAdminRes = await _userManager.CreateAsync(administrator, adminPassword);

                if (createAdminRes.Succeeded)
                {
                    await _userManager.AddToRoleAsync(administrator, Role.Types.Administrator.ToString());
                }
            }
        }
    }
}
