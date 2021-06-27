using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingleWebIdentityAplication.Models
{
    public class ResponseData<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
