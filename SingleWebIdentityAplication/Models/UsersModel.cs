using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SingleWebIdentityAplication.Models
{
    public class UsersModel
    {
        [Key]
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
