using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Registration;

namespace TaskExecutionSystem.BLL.DTO
{
    public class TeacherDTO : UserDTO
    {
        public string Position { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public static TeacherDTO Map(TeacherRegisterRequest entity) => new TeacherDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Surname = entity.Surname,
            Patronymic = entity.Patronymic,
            Position = entity.Position,
            DepartmentId = entity.DepartmentId,
            DepartmentName = entity.Department.Name,
            UserName = entity.UserName,
            Email = entity.Email,
        };

        public static TeacherDTO Map(Teacher entity) => new TeacherDTO
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Name = entity.Name,
            Surname = entity.Surname,
            Patronymic = entity.Patronymic,
            Position = entity.Position,
            DepartmentId = entity.DepartmentId,
            DepartmentName = entity.Department.Name,
            UserName = entity.User.UserName,
            Email = entity.User.Email,
        };
    }
}
