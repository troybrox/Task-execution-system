using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Registration;
using TaskExecutionSystem.BLL.DTO.Task;

namespace TaskExecutionSystem.BLL.DTO
{
    public class StudentDTO : UserDTO
    {
        public int GroupId { get; set; }

        public string GroupNumber { get; set; }


        public SolutionDTO Solution { get; set; }

        public List<SolutionDTO> Solutions { get; set; }

        public List<TaskDTO> Tasks { get; set; }


        public StudentDTO()
        {
            Solutions = new List<SolutionDTO>();
            Tasks = new List<TaskDTO>();
        }



        public static StudentDTO Map(StudentRegisterRequest entity)
        {
            var dto = new StudentDTO
            {
                Id = entity.Id,
                Faculty = entity?.Group?.Faculty.Name,
                Name = entity.Name,
                Surname = entity.Surname,
                Patronymic = entity.Patronymic,
                GroupId = entity.GroupId,
                UserName = entity.UserName,
                Email = entity.Email
            };
            return dto;
        }
        

        public static StudentDTO Map(Student entity)
        {
            var dto = new StudentDTO()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Name = entity.Name,
                Surname = entity.Surname,
                Patronymic = entity.Patronymic,
                GroupId = entity.GroupId
            };
            if(entity.Group != null)
            {
                dto.GroupNumber = entity.Group.NumberName;
                if (entity.Group.Faculty != null)
                {
                    dto.Faculty = entity.Group.Faculty.Name;
                }
            }
            if(entity.User != null)
            {
                dto.UserName = entity.User.UserName;
                dto.Email = entity.User.Email;
            }
            return dto;
        }
    }
}
