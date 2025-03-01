﻿using System;
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

        public string FacultyName { get; set; }

        public string Password { get; set; }


        public static TeacherDTO Map(TeacherRegisterRequest entity) => new TeacherDTO
        {
            Id = entity.Id,
            Faculty = entity.Department.Faculty.Name,
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
            Faculty = entity?.Department.Faculty.Name,
            UserId = entity.UserId,
            Name = entity.Name,
            Surname = entity.Surname,
            Patronymic = entity.Patronymic,
            Position = entity.Position,
            DepartmentId = entity.DepartmentId,
            DepartmentName = entity?.Department.Name,
            UserName = entity.User.UserName,
            Email = entity.User.Email,
            FacultyName = entity?.Department.Faculty.Name,
            Password = null
        };

        public static TeacherDTO MapProfile(Teacher entity) => new TeacherDTO
        {
            Id = entity.Id,
            Faculty = entity?.Department.Faculty.Name,
            FacultyName = entity?.Department.Faculty.Name,
            UserId = entity.UserId,
            Name = entity.Name,
            Surname = entity.Surname,
            Patronymic = entity.Patronymic,
            Position = entity.Position,
            DepartmentId = entity.DepartmentId,
            DepartmentName = entity?.Department.Name,
            UserName = entity.User.UserName,
            Email = entity.User.Email,
        };
    }
}
