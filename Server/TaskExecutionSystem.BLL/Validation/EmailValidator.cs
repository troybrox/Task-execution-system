using System.Text.RegularExpressions;

namespace TaskExecutionSystem.BLL.Validation
{
    public class EmailValidator
    {
        public static bool IsCorrect(string email)
        {
            return Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        }
    }
}
