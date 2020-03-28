using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TaskExecutionSystem.DAL.Entities.Identity
{
    public class Role : IdentityRole<long>
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
