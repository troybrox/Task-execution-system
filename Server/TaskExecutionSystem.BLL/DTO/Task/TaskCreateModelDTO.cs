using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Task;
using TaskExecutionSystem.DAL.Entities.Relations;

namespace TaskExecutionSystem.BLL.DTO.Task
{
    public class TaskCreateModelDTO
    {
        public string Name { get; set; }

        public string ContentText { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime FinishDate { get; set; }

        public int TeacherId { get; set; }

        public int TypeId { get; set; }

        public int SubjectId { get; set; }

        public int GroupId { get; set; }

        public int[] StudentIds { get; set; }


        public static TaskModel Map(TaskCreateModelDTO dto) 
        {
            var newTask = new TaskModel
            {
                Name = dto.Name,
                ContentText = dto.ContentText,
                BeginDate = dto.BeginDate,
                FinishDate = dto.FinishDate,
                TypeId = dto.TypeId,
                TeacherId = dto.TeacherId,
                SubjectId = dto.SubjectId,
                GroupId = dto.GroupId
            };
            //foreach(var studentId in dto.StudentIds)
            //{
            //    newTask.TaskStudentItems.Add(new TaskStudentItem
            //    {
            //        StudentId = studentId
            //    });
            //}
            return newTask;
        }
    }
}
