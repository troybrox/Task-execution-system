using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskExecutionSystem.Identity.Contracts
{
    public static class IdentityPolicyContract
    {
        public const string AdministratorPolicy = nameof(AdministratorPolicy);

        public const string TeacherUserPolicy = nameof(TeacherUserPolicy);

        public const string StudentUserPolicy = nameof(StudentUserPolicy);
    }
}
