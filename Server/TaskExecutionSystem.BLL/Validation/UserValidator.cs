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

            if (String.IsNullOrEmpty(dto.Patronymic) || dto.Password.Length >= 50)
            {
                messages.Add("Укажите корректно отчество. Только буквы, максимальная длина - 50.");
                res = false;
            }

            if (String.IsNullOrEmpty(dto.Position) || dto.Name.Length >= 50)
            {
                messages.Add("Укажите корректно должность. Только буквы, максимальная длина - 50.");
                res = false;
            }

            if (String.IsNullOrEmpty(dto.UserName))
            {
                messages.Add("Укажите корректно имя пользователя");
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

            if (String.IsNullOrEmpty(dto.Patronymic) || dto.Password.Length >= 50)
            {
                messages.Add("Укажите корректно отчество. Только буквы, максимальная длина - 50.");
                res = false;
            }

            if (String.IsNullOrEmpty(dto.UserName))
            {
                messages.Add("Укажите корректно имя пользователя");
                res = false;
            }

            return res;
        }
    }
}
