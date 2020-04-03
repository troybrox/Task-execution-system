using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.Application
{
    public class IdentityInitializer : DataInitializerBase
    {
        public IdentityInitializer(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override async Task InitializeAsync(DataContext context)
        {
            var roleNames = Enum
                .GetNames(typeof(Role.Types));

            var existingRoles = context
                .Roles
                .Select(x => x.Name)
                .ToArray();

            if(roleNames.Length != existingRoles.Length)
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
        }
    }
}
