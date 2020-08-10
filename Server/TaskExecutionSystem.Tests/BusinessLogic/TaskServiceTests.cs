using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TaskExecutionSystem.BLL.Services;
using Moq;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.BLL.DTO.Task;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;

namespace TaskExecutionSystem.Tests.BusinessLogic
{
    public class TaskServiceTests
    {
        private DataContext context = InMemoryDBContext.GetDBContext();

        [Fact]
        public void TaskService_GetCurrentTimePercentage_InvalidDates()
        {
            // Arrange
            var taskDTO = new TaskDTO
            {
                BeginDate = new DateTime(2020, 06, 15, 0, 0, 0),
                FinishDate = new DateTime(2020, 05, 15, 0, 0, 0)
            };
            // Create instance
            TaskService taskService = new TaskService(context);

            // Act
            var timeBarValue = taskService.GetCurrentTimePercentageValue(ref taskDTO);

            // Assert
            Assert.Equal(100, timeBarValue);
        }

        [Fact]
        public void TaskService_GetCurrentTimePercentage_ValidDates_StartInFuture()
        {
            // Arrange
            var taskDTO = new TaskDTO
            {
                BeginDate = new DateTime(2020, 05, 30, 0, 0, 0),
                FinishDate = new DateTime(2020, 06, 10, 0, 0, 0)
            };

            // Create instance
            TaskService taskService = new TaskService(context);

            // Act
            var timeBarValue = taskService.GetCurrentTimePercentageValue(ref taskDTO);

            // Assert
            Assert.Equal(100, timeBarValue);
        }

        [Fact]
        public void TaskService_GetCurrentTimePercentage_ValidDates_StartInPast()
        {
            // Arrange
            var taskDTO = new TaskDTO
            {
                BeginDate = new DateTime(2020, 05, 10, 0, 0, 0),
                FinishDate = new DateTime(2020, 06, 10, 0, 0, 0)
            };
            // Create instance
            TaskService taskService = new TaskService(context);

            // Act
            var timeBarValue = taskService.GetCurrentTimePercentageValue(ref taskDTO);

            // Assert
            Assert.True(timeBarValue >= 0 && timeBarValue <= 100);
        }

        [Fact]
        public void TaskService_GetCurrentTimePercentage_ValidDates_SameDates()
        {
            // Arrange
            var taskDTO = new TaskDTO
            {
                BeginDate = new DateTime(2020, 06, 10, 0, 0, 0),
                FinishDate = new DateTime(2020, 06, 10, 0, 0, 0)
            };
            // Create instance
            TaskService taskService = new TaskService(context);

            // Act
            var timeBarValue = taskService.GetCurrentTimePercentageValue(ref taskDTO);

            // Assert
            Assert.Equal(100, timeBarValue);
        }
    }
}
