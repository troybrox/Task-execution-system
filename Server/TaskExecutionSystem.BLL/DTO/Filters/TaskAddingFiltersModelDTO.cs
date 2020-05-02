using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.BLL.DTO.Studies;
using TaskExecutionSystem.BLL.DTO.Task;

namespace TaskExecutionSystem.BLL.DTO.Filters
{
    public class TaskAddingFiltersModelDTO
    {
        public List<SubjectDTO> Subjects { get; set; }

        public List<TypeOfTaskDTO> Types { get; set; }

        public List<GroupDTO> Groups { get; set; }


        public TaskAddingFiltersModelDTO()
        {
            Subjects = new List<SubjectDTO>();
            Types = new List<TypeOfTaskDTO>();
            Groups = new List<GroupDTO>();
        }

        public TaskAddingFiltersModelDTO(List<SubjectDTO> subjects, 
            List<TypeOfTaskDTO> types,
            List<GroupDTO> groups)
        {
            Subjects = subjects;
            Types = types;
            Groups = groups;
        }
    }
}
