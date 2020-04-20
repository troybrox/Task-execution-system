using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Registration;

namespace TaskExecutionSystem.BLL.DTO
{
    public class StudentDTO : UserDTO
    {
        public int GroupId { get; set; }

        public string GroupNumber { get; set; }


        public static StudentDTO Map(StudentRegisterRequest entity) => new StudentDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Surname = entity.Surname,
            Patronymic = entity.Patronymic,
            GroupId = entity.GroupId,
            UserName = entity.UserName,
            Email = entity.Email
        };

        public static StudentDTO Map(Student entity) => new StudentDTO
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Name = entity.Name,
            Surname = entity.Surname,
            Patronymic = entity.Patronymic,
            GroupId = entity.GroupId,
            GroupNumber = entity.Group.NumberName,
            UserName = entity.User.UserName,
            Email = entity.User.Email,
        };
    }
}
