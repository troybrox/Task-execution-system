using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.AsyncInitialization;
using Microsoft.Extensions.DependencyInjection;
using TaskExecutionSystem.DAL.Data;

namespace TaskExecutionSystem.Application.Initialization
{
    // родительский класс для инициализаторов
    public abstract class DataInitializerBase : IAsyncInitializer
    {
        private readonly IServiceProvider _serviceProvider;

        protected DataInitializerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<DataContext>())
                {
                    await InitializeAsync(context);
                    await context.SaveChangesAsync();
                }
            }
        }

        protected abstract Task InitializeAsync(DataContext context);
    }
}
