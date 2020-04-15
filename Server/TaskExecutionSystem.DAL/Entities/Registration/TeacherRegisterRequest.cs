using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskExecutionSystem.DAL.Interfaces;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.DAL.Entities.Registration
{
    public class TeacherRegisterRequest : RegisterRequestBase
    {
        public int DepartmentId { get; set; }
    }
}
