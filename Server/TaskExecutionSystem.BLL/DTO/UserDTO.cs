using System.ComponentModel.DataAnnotations;

namespace TaskExecutionSystem.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public long UserId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }
    }
}
