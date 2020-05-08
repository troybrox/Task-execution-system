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
        private readonly RoleManager<Role> _roleManager;

        public IdentityInitializer(IServiceProvider serviceProvider, IConfiguration configuration, UserManager<User> userManager, RoleManager<Role> roleManager)
            : base(serviceProvider)
        {
            Configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // инициализация ролей 
        protected override async Task InitializeAsync(DataContext context)
        {
            await InitializeRoles(context);
            await InitializeAdmin(context);
        }

        private async Task InitializeRoles(DataContext context)
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
                await context.SaveChangesAsync();
            }
        }


        // инициализация адиминистратора | при запуске приложения проверяется его существование, если нет - добавляется в систему;
        // пароль и логин админа в файле appsettings.json
        private async Task InitializeAdmin(DataContext context)
        {
            var adminUserName = Configuration.GetSection("AdminAuthOptions")["UserName"];
            var user = await _userManager.FindByNameAsync(adminUserName);
            var existingAdminRole = await _roleManager.FindByNameAsync(Role.Types.Administrator.ToString());

            if (user == null)
            {
                if (existingAdminRole != null)
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

            else
            {
                if(!await _userManager.IsInRoleAsync(user, Role.Types.Administrator.ToString()))
                {
                    if(existingAdminRole != null)
                    {
                        await _userManager.AddToRoleAsync(user, Role.Types.Administrator.ToString());
                    }
                }
            }
        }
    }
}
