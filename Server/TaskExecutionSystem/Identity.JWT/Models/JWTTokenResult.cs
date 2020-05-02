using System;

namespace TaskExecutionSystem.Identity.JWT.Models
{
    public class JWTTokenResult
    {
        public string AccessToken { get; internal set; }

        public TimeSpan Expires { get; set; }
    }
}
