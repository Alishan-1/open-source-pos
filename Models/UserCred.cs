using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class UserCred
    {
        public int UserID { get; set; }
        public int? AppID { get; set; }
        public int? AppRoleID { get; set; }
        public string UserPassword { get; set; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime? CreateDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public string UserPhoto { get; set; }
        public string PreviousPassword { get; set; }
        public DateTime? ExpirePassword { get; set; }
        public bool? IsTemp { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsCustomer { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool? LockoutEnabled { get; set; }
        
        /// <summary>
        /// Shows that the password of user is expired or not
        /// </summary>
        public bool? IsPasswordExpired { get; set; }
        /// <summary>
        /// Shows that whather the password entered by user is correct or not
        /// </summary>
        public bool? IsPasswordCorrect { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public int? AccessFailedCount { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string LinkOrCode { get; set; }
        /// <summary>
        /// This field will be true when the password is changed through forget password form
        /// </summary>
        public bool? IsForgetPassword { get; set; }
        /// <summary>
        /// used in change password screen for current password
        /// </summary>
        public string CurrentPassword { get; set; }

        public AuthenticationResult authenticationResult { get; set; }
        /// <summary>
        /// Users Current location from where login or logout request is generated
        /// </summary>
        public UserGeoLocation UsersGeoLocation { get; set; }
        /// <summary>
        /// represents device detected information including browser, os and other useful information regarding the device. The information is based on user-agent.
        /// </summary>
        public DeviceInfo DeviceInfo { get; set; }

        public bool? RememberUser { get; set; }
        public string SessionToken { get; set; }
        public DateTimeOffset TokenExpirationDate { get; set; }
        /// <summary>
        /// on login this field will be used as session start date time,
        /// on logout this field will be used as session end date time
        /// </summary>
        public DateTimeOffset SessionDate { get; set; }


    }

}
