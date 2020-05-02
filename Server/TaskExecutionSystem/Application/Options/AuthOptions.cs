using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskExecutionSystem.Application.Options
{
    public class AuthOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int Lifetime { get; set; }

        public string Secret { get; set; }
    }
}
