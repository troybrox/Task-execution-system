using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using static TaskExecutionSystem.BLL.Infrastructure.Contracts.DirectoryContract;
using static TaskExecutionSystem.BLL.Infrastructure.Contracts.ErrorMessageContract;
using TaskExecutionSystem.BLL.DTO.Task;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities.Task;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Relations;
using TaskExecutionSystem.DAL.Entities.File;
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.BLL.DTO.Studies;

namespace TaskExecutionSystem.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _context;
        public TaskService(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationDetailDTO> AddFileToTaskAsync(int taskID, string userFileName, string fileName = null)
        {
            var detail = new OperationDetailDTO();
            try
            {
                var task = await _context.TaskModels.FindAsync(taskID);
                if (task != null)
                {
                    TaskFile newFile;
                    if(fileName != null)
                    {
                        newFile = new TaskFile
                        {
                            TaskModel = task,
                            FileName = fileName,
                            Path = TaskFilePath + userFileName,
                            FileURI = TaskFileURI + userFileName,
                        };
                    }
                    else
                    {
                        newFile = new TaskFile
                        {
                            TaskModel = task,
                            FileName = userFileName,
                            Path = TaskFilePath + userFileName,
                            FileURI = TaskFileURI + userFileName,
                        };
                    }
                    
                    await _context.TaskFiles.AddAsync(newFile);
                    await _context.SaveChangesAsync();
                    detail.Succeeded = true;
                }
                else
                {
                    detail.ErrorMessages.Add("Ошибка при добавлении файла: задание не найдено.");
                }
                return detail;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add("Ошибка при добавлении файла к заданию. " + e.Message);
                return detail;
            }
        }

        public async Task<OperationDetailDTO> AddFileToSolutionAsync(int solutionID, string userFileName, string fileName = null)
        {
            var detail = new OperationDetailDTO();
            try
            {
                var solution = await _context.Solutions.FindAsync(solutionID);

                if (solution != null)
                {
                    SolutionFile newFile;
                    if (fileName != null)
                    {
                        newFile = new SolutionFile
                        {
                            Solution = solution,
                            FileName = userFileName,
                            Path = SolutionFilePath + fileName,
                            FileURI = SolutionFileURI + fileName,
                        };
                    }
                    else
                    {
                        newFile = new SolutionFile
                        {
                            Solution = solution,
                            FileName = userFileName,
                            Path = SolutionFilePath + userFileName,
                            FileURI = SolutionFileURI + userFileName,
                        };
                    }
                    
                    await _context.SolutionFiles.AddAsync(newFile);
                    await _context.SaveChangesAsync();
                    detail.Succeeded = true;
                }
                else
                {
                    detail.ErrorMessages.Add("Ошибка при добавлении файла: решениие задачи не найдено.");
                }
                return detail;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add("Ошибка при добавлении файла к решению задачи. " + e.Message);
                return detail;
            }
        }

        public void GetCurrentTimePercentage(ref TaskDTO dto)
        {
            int timeProgressPercentage = 100;

            var totalInterval = dto.FinishDate - dto.BeginDate;
            var pastInterval = DateTime.Now - dto.BeginDate;

            if (totalInterval.TotalMinutes > 0)
            {
                if(pastInterval.TotalMinutes > 0)
                {
                    timeProgressPercentage = (int)(Math.Abs((1 - (pastInterval.TotalMinutes / totalInterval.TotalMinutes)) * 100));
                }
            }

            dto.TimeBar = timeProgressPercentage;
        }

        public Task<OperationDetailDTO<List<TaskDTO>>> GetTasksFromDBAsync(FilterDTO[] filters)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationDetailDTO<TaskDTO>> GetTaskByIDAsync(int id)
        {
            var detail = new OperationDetailDTO<TaskDTO>();
            try
            {
                var entity = await _context.TaskModels.FindAsync(id);
                if (entity != null)
                {
                    var dto = TaskDTO.Map(entity);
                    detail.Succeeded = true;
                    detail.Data = dto;
                }
                else
                {
                    detail.ErrorMessages.Add("Задание не найдено.");
                }
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
            }
            return detail;
        }

        private async Task AddStudentsToTaskAsync(int taskID, int[] studentIDs)
        {
            var task = await _context.TaskModels.FindAsync(taskID);
            var students = new List<Student>();

            foreach(var studentID in studentIDs)
            {
                var taskStudentItem = new TaskStudentItem();
                var student = await _context.Students.FindAsync(studentID);
                if(student != null)
                {
                    taskStudentItem.Student = student;
                    taskStudentItem.Task = task;
                    task.TaskStudentItems.Add(taskStudentItem);
                }
            }
        }

        public Task<OperationDetailDTO> UpdateTaskAsync()
        {
            throw new NotImplementedException();
        }

        
    }
}
