using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TaskExecutionSystem.Application.Options;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.Application
{
    public class SeedInitializer : DataInitializerBase
    {
        private readonly IOptions<SeedOptions> _options;
        private readonly UserManager<User> _userManager;

        public SeedInitializer(IServiceProvider serviceProvider, IOptions<SeedOptions> options,
            UserManager<User> userManager, IPasswordHasher<User> hasher)
            : base(serviceProvider)
        {
            _options = options;
            _userManager = userManager;
        }

        protected override async Task InitializeAsync(DataContext dbContext)
        {
            await InitializeUsers();
        }

        private async Task InitializeUsers()
        {
            var users = _options
                .Value
                .Teachers
                .Select(x => new User
                {
                    UserName = x.UserName,
                    NormalizedUserName = x.UserName.ToUpperInvariant()
                })
                .ToList();

            var existingUsers = await _userManager.GetUsersInRoleAsync(Role.Types.Administrator.ToString());

            if (users.Any() && users.Count > existingUsers.Count)
            {
                var newUsers = users
                    .Where(x => existingUsers.All(u => u.UserName != x.UserName))
                    .ToList();

                foreach (var user in newUsers)
                {
                    await _userManager.CreateAsync(user);
                    await _userManager.AddToRoleAsync(user, Role.Types.Teacher.ToString());
                }
            }
        }
    }
}
