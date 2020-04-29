using System;
using System.Collections.Generic;
using System.Text;

namespace TaskExecutionSystem.BLL.Infrastructure.Contracts
{
    public static class ErrorMessageContract
    {
        public const string _serverErrorMessage = "Ошибка, произошло исключение на сервере. Подробнее: ";

        public const string _signInErrorMessage = "Ошибка при авторизации. Неверное имя пользователя/электронная " +
            "почта или пароль. Проверьте правильность ввода и повторите попытку.";
    }
}
