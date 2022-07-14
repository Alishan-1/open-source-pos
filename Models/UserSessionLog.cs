using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    /// <summary>
    /// Maps to Table UserLog
    /// </summary>
    public class UserSessionLog
    {
        public int UserLogID { get; set; }
        public int UserID { get; set; }
        public bool RememberUser { get; set; }
        public string SessionToken { get; set; }
        
        public DateTimeOffset SessStart { get; set; }
        public DateTimeOffset SessEnd { get; set; }
        public DateTimeOffset TokenExpirationDate { get; set; }
    }
}
