using System.Collections.Generic;

namespace TaskExecutionSystem.BLL.DTO.Auth
{
    public class UserSignInDetailDTO
    {
        public UserDTO UserDTO { get; set; }

        public IList<string> UserRoles { get; set; }
    }
}
