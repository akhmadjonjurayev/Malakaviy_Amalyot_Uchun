using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingleWebIdentityAplication.Models
{
    public static class ClaimsType
    {
        public static string UserName = "UserName";
        public static string UserRole = "UserRole";
        public static string Email = "Email";
        public static string Password = "Password";
    }
    public static class RolesBase
    {
        public static string? Admin = "Admin";
        public static string User = "User";
        public static string Staff = "Staff";
    }
}
