using System;

using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.BLL.DTO;

namespace TaskExecutionSystem.BLL.DTO.Task
{
    public class TaskDTO
    {
        public string Type { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public string ContentText { get; set; }
        public bool IsOpen { get; set; }
        public string FileURI { get; set; }
        
        public string TeacherName { get; set; }
        public string TeacherSurname { get; set; }
        public string TeacherPatronymic { get; set; }
        
        public DateTime BeginDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int TimeBar { get; set; }

        public string Group { get; set; }
        public List<StudentDTO> Students { get; set; }
        
    }
}
