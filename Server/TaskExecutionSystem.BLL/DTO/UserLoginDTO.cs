using System.ComponentModel.DataAnnotations;

namespace TaskExecutionSystem.BLL.DTO
{
    public class UserLoginDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
