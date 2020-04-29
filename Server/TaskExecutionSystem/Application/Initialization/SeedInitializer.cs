using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TaskExecutionSystem.Application.Options;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.Application.Initialization
{
    public class SeedInitializer : DataInitializerBase
    {
        private readonly IOptions<SeedOptions> _options;
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _hasher;

        public SeedInitializer(IServiceProvider serviceProvider, IOptions<SeedOptions> options,
            UserManager<User> userManager, IPasswordHasher<User> hasher)
            : base(serviceProvider)
        {
            _hasher = hasher;
            _options = options;
            _userManager = userManager;
        }

        protected override async Task InitializeAsync(DataContext dbContext)
        {
            await InitializeUsers();
        }

        private async Task InitializeUsers()
        {
            var teacherUsers = _options
                .Value
                .Teachers
                .Select(x => new User
                {
                    UserName = x.UserName,
                    NormalizedUserName = x.UserName.ToUpperInvariant()
                })
                .ToList();

            var studentUsers = _options
                .Value
                .Students
                .Select(x => new User
                {
                    UserName = x.UserName,
                    NormalizedUserName = x.UserName.ToUpperInvariant()
                })
                .ToList();

            var existingTeachers = await _userManager.GetUsersInRoleAsync(Role.Types.Teacher.ToString());
            var existingStudents = await _userManager.GetUsersInRoleAsync(Role.Types.Student.ToString());

            if (teacherUsers.Any() && teacherUsers.Count > existingTeachers.Count)
            {
                var newUsers = teacherUsers
                    .Where(x => existingTeachers.All(u => u.UserName != x.UserName))
                    .ToList();

                foreach (var user in newUsers)
                {
                    user.PasswordHash = _hasher.HashPassword(user, "Qwe123!");
                    await _userManager.CreateAsync(user);
                    await _userManager.AddToRoleAsync(user, Role.Types.Teacher.ToString());
                }
            }

            if (studentUsers.Any() && studentUsers.Count > existingStudents.Count)
            {
                var newUsers = studentUsers
                    .Where(x => existingStudents.All(u => u.UserName != x.UserName))
                    .ToList();

                foreach (var user in newUsers)
                {
                    user.PasswordHash = _hasher.HashPassword(user, "Qwe123!");
                    await _userManager.CreateAsync(user);
                    await _userManager.AddToRoleAsync(user, Role.Types.Student.ToString());
                }
            }
        }
    }
}
