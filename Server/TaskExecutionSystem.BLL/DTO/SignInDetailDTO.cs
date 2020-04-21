using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.BLL.DTO
{
    public class SignInDetailDTO
    {
        public string Role { get; set; }

        public string IdToken { get; set; }
    }
}
