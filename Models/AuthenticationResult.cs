using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class AuthenticationResult
    {

        public bool? IsAuthorisedCurrently { get; set; }
        public bool? IsCredentialValid { get; set; }
        public bool? IsLoginSuccessful { get; set; }
        public bool? IsEmailCorrect { get; set; }
        public bool? IsPasswordExpired { get; set; }
        public bool? IsLockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEndTime { get; set; }
    }
}
