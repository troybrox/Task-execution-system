using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.DAL.Interfaces;

namespace TaskExecutionSystem.DAL.Entities.Identity
{
    public class Role : IdentityRole<long>, IEntity
    {
        public enum Types
        {
            Administrator,
            Teacher,
            Student
        }

        protected Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
            NormalizedName = roleName.ToUpperInvariant();
        } 
    }
}
