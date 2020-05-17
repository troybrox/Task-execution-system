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
using Microsoft.EntityFrameworkCore;

namespace TaskExecutionSystem.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _context;
        public TaskService(DataContext context)
        {
            _context = context;
        }

        // узнать о текущем файле решения
        public async Task<(bool Succeeded, string filePath, int fileId)> GetSolutionFileNameAsync(int solutionID)
        {
            bool succeeded = false;
            string filePath = null;
            int fileId = 0;

            Solution solution;
            if((solution = await _context.Solutions
                .Include(s => s.File)
                .FirstOrDefaultAsync(s => s.Id == solutionID)) != null)
            {
                if(solution.File != null)
                {
                    succeeded = true;
                    filePath = solution.File.Path;
                    fileId = solution.File.Id;
                }
            }

            return (succeeded, filePath, fileId);
        }

        // узнать о текущем файле задачи
        public async Task<(bool Succeeded, string filePath, int fileId)> GetTaskFileNameAsync(int taskId)
        {
            bool succeeded = false;
            string filePath = null;
            int fileId = 0;

            TaskModel task;
            if ((task = await _context.TaskModels
                .Include(t => t.File)
                .FirstOrDefaultAsync(t => t.Id == taskId)) != null)
            {
                if (task.File != null)
                {
                    succeeded = true;
                    filePath = task.File.Path;
                    fileId = task.File.Id;
                }
            }

            return (succeeded, filePath, fileId);
        }


        // добавить файл к задаче
        public async Task<OperationDetailDTO> AddFileToTaskAsync(int taskID, string userFileName, string uniquefileName = null)
        {
            var detail = new OperationDetailDTO();
            try
            {
                var task = await _context.TaskModels.FindAsync(taskID);
                if (task != null)
                {
                    TaskFile newFile;
                    if(uniquefileName != null)
                    {
                        newFile = new TaskFile
                        {
                            TaskModel = task,
                            FileName = userFileName,
                            Path = TaskFilePath + uniquefileName,
                            FileURI = TaskFileURI + uniquefileName,
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

        // добавить файл к решению
        public async Task<OperationDetailDTO> AddFileToSolutionAsync(int solutionID, string userFileName, string uniquefileName = null)
        {
            var detail = new OperationDetailDTO();
            try
            {
                var solution = await _context.Solutions.FindAsync(solutionID);

                if (solution != null)
                {
                    SolutionFile newFile;
                    if (uniquefileName != null)
                    {
                        newFile = new SolutionFile
                        {
                            Solution = solution,
                            FileName = userFileName,
                            Path = SolutionFilePath + uniquefileName,
                            FileURI = SolutionFileURI + uniquefileName,
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

        // обновить файл задачи
        public async Task<OperationDetailDTO> UpdateTaskFileAsync(int fileID, string newUserFileName, string newUniqueFileName)
        {
            var detail = new OperationDetailDTO();

            try
            {
                var file = await _context.TaskFiles.FindAsync(fileID);

                if (file == null)
                {
                    detail.ErrorMessages.Add("Ошибка при обновлении файла задачи: файл не найден.");
                }

                file.FileName = newUserFileName;
                file.Path = TaskFilePath + newUniqueFileName;
                file.FileURI = TaskFileURI + newUniqueFileName;

                _context.TaskFiles.Update(file);
                await _context.SaveChangesAsync();

                detail.Succeeded = true;
                return detail;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add("Ошибка при обновлени файла задачи: " + e.Message);
                return detail;
            }

        }

        // обновить файл решения
        public async Task<OperationDetailDTO> UpdateSolutionFileAsync(int fileID, string newUserFileName, string newUniqueFileName)
        {
            var detail = new OperationDetailDTO();

            try
            {
                var file = await _context.SolutionFiles.FindAsync(fileID);

                if (file == null)
                {
                    detail.ErrorMessages.Add("Ошибка при обновлении файла решения задачи: файл не найден.");
                }

                file.FileName = newUserFileName;
                file.Path = SolutionFilePath + newUniqueFileName;
                file.FileURI = SolutionFileURI + newUniqueFileName;

                _context.SolutionFiles.Update(file);
                await _context.SaveChangesAsync();

                detail.Succeeded = true;
                return detail;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add("Ошибка при обновлени файла решения задачи: " + e.Message);
                return detail;
            }
            
        }


        // присвоить полю объекта задачи процентное количество времени
        public void GetCurrentTimePercentage(ref TaskDTO dto)
        {
            int timeProgressPercentage = 100;

            if(DateTime.Now <= dto.BeginDate)
            {
                dto.TimeBar = timeProgressPercentage;
                return;
            }
            else
            {
                var totalInterval = dto.FinishDate - dto.BeginDate;
                var pastInterval = DateTime.Now - dto.BeginDate;

                if (pastInterval >= totalInterval)
                {
                    dto.TimeBar = 0;
                    return;
                }

                if (totalInterval.TotalMinutes > 0)
                {
                    if (pastInterval.TotalMinutes > 0)
                    {
                        timeProgressPercentage = (int)(Math.Abs((1 - (pastInterval.TotalMinutes / totalInterval.TotalMinutes)) * 100));
                    }
                }
            }

            if(timeProgressPercentage >= 0 && timeProgressPercentage <= 100)
            {
                dto.TimeBar = timeProgressPercentage;
            }
        }


        // не используется:
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
