using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using static TaskExecutionSystem.BLL.Infrastructure.Contracts.DirectoryContract;
using TaskExecutionSystem.BLL.DTO.Task;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities.Task;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Relations;
using TaskExecutionSystem.DAL.Entities.File;

namespace TaskExecutionSystem.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _context;
        public TaskService(DataContext context)
        {
            _context = context;

        }

        public Task<OperationDetailDTO> CreateFileAsync()
        {
            throw new NotImplementedException();
        }

        // todo: adding file
        public async Task<OperationDetailDTO> CreateTaskAsync(TaskCreateModelDTO dto)
        {
            var detail = new OperationDetailDTO { Succeeded = false };
            try
            {
                if(dto != null)
                {
                    var newTask = TaskCreateModelDTO.Map(dto);
                    var newTSItems = new List<TaskStudentItem>();

                    await _context.TaskModels.AddAsync(newTask);
                    await _context.SaveChangesAsync();


                    await AddTaskStudentsAsync(newTask.Id, dto.StudentIds);
                    
                    
                    detail.Succeeded = true;
                    return detail;
                }
                else
                {
                    detail.ErrorMessages.Add("Параметр модели создаваемой задачи был равен NULL");
                    return detail;
                }
                
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(e.Message);
                return detail;
            }
        }

        public async Task<OperationDetailDTO> CreateFileAsync(IFormFile uploadedFile = null)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationDetailDTO> AddFileToDBAsync(int taskID, string fileName)
        {
            var detail = new OperationDetailDTO();
            try
            {
                var task = await _context.TaskModels.FindAsync(taskID);
                if (task != null)
                {
                    var newFile = new TaskFile
                    {
                        //TaskItemId = taskID,
                        TaskItem = task,
                        FileName = fileName,
                        Path = TaskFilePath + fileName,
                        FileURI = TaskFileURI + fileName,
                    };
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

        private async Task AddTaskStudentsAsync(int taskID, int[] studentIDs)
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
    }
}
