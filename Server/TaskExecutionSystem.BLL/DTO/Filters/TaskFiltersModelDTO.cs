using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.BLL.DTO.Studies;
using TaskExecutionSystem.BLL.DTO.Task;

namespace TaskExecutionSystem.BLL.DTO.Filters
{
    public class TaskFiltersModelDTO
    {
        public List<SubjectDTO> Subjects { get; set; }

        public List<TypeOfTaskDTO> Types { get; set; }

        public List<GroupDTO> Groups { get; set; }


        public TaskFiltersModelDTO()
        {
            Subjects = new List<SubjectDTO>();
            Types = new List<TypeOfTaskDTO>();
            Groups = new List<GroupDTO>();
        }

        public TaskFiltersModelDTO(List<SubjectDTO> subjects, 
            List<TypeOfTaskDTO> types,
            List<GroupDTO> groups)
        {
            Subjects = subjects;
            Types = types;
            Groups = groups;
        }

        public TaskFiltersModelDTO(List<SubjectDTO> subjects,
            List<TypeOfTaskDTO> types)
        {
            Subjects = subjects;
            Types = types;
        }
    }
}
