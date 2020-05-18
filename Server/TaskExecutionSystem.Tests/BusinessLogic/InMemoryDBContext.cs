using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Data;

namespace TaskExecutionSystem.Tests.BusinessLogic
{
    public static class InMemoryDBContext
    {
        public static DataContext GetDBContext()
        {
            DbContextOptions<DataContext> options = new DbContextOptionsBuilder<DataContext>()
                            .UseInMemoryDatabase("TaskDatabase")
                            .Options;

            return new DataContext(options);
        }
    }
}
