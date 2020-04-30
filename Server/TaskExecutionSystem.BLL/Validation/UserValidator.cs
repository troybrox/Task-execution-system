using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.BLL.DTO;

namespace TaskExecutionSystem.BLL.Validation
{
    public class UserValidator
    {
        public static bool Validate(TeacherRegisterDTO dto, out List<string> messages)
        {
            bool res = true;
            messages = new List<string>();

            if (String.IsNullOrEmpty(dto.Name) || dto.Name.Length >= 50)
            {
                messages.Add("Укажите корректно имя. Только буквы, максимальная длина - 50.");
                res = false;   
            }

            if (String.IsNullOrEmpty(dto.Surname) || dto.Surname.Length >= 50)
            {
                messages.Add("Укажите корректно фамилию. Только буквы, максимальная длина - 50.");
                res = false;
            }

            if (String.IsNullOrEmpty(dto.Patronymic) || dto.Patronymic.Length >= 50)
            {
                messages.Add("Укажите корректно отчество. Только буквы, максимальная длина - 50.");
                res = false;
            }

            if (String.IsNullOrEmpty(dto.Position) || dto.Position.Length >= 50)
            {
                messages.Add("Укажите корректно должность. Только буквы, максимальная длина - 50.");
                res = false;
            }

            if (String.IsNullOrEmpty(dto.UserName) || dto.UserName.Length >= 50)
            {
                messages.Add("Укажите корректно имя пользователя. Максимальная длина - 50.");
                res = false;
            }

            if (dto.Password.Length < 6)
            {
                messages.Add("Укажите корректно пароль. Минимальная длина - 6.");
                res = false;
            }

            if (!EmailValidator.IsCorrect(dto.Email))
            {
                messages.Add("Укажите корректно электронную почту.");
                res = false;
            }

            return res;
        }

        public static bool Validate(StudentRegisterDTO dto, out List<string> messages)
        {
            bool res = true;
            messages = new List<string>();

            if (String.IsNullOrEmpty(dto.Name) || dto.Name.Length >= 50)
            {
                messages.Add("Укажите корректно имя. Только буквы, максимальная длина - 50.");
                res = false;
            }

            if (String.IsNullOrEmpty(dto.Surname) || dto.Surname.Length >= 50)
            {
                messages.Add("Укажите корректно фамилию. Только буквы, максимальная длина - 50.");
                res = false;
            }

            if (String.IsNullOrEmpty(dto.Patronymic) || dto.Patronymic.Length >= 50)
            {
                messages.Add("Укажите корректно отчество. Только буквы, максимальная длина - 50.");
                res = false;
            }

            if (String.IsNullOrEmpty(dto.UserName) || dto.UserName.Length >= 50)
            {
                messages.Add("Укажите корректно имя пользователя. Максимальная длина - 50.");
                res = false;
            }

            if (dto.Password.Length < 6)
            {
                messages.Add("Укажите корректно пароль. Минимальная длина - 6.");
                res = false;
            }

            if (!EmailValidator.IsCorrect(dto.Email))
            {
                messages.Add("Укажите корректно электронную почту.");
                res = false;
            }

            return res;
        }
    }
}
