using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingleWebIdentityAplication.Models
{
    public class SignInViewModel : LoginViewModel
    {
        public string EmailAddress { get; set; }
    }
    public class LoginViewModel : IModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public interface IModel
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}
