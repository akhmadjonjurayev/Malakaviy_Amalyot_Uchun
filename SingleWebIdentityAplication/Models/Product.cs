using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SingleWebIdentityAplication.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductCost { get; set; }
        public DateTime MadeTime { get; set; }
        public DateTime AvailableTime { get; set; }
        public string CompanyName { get; set; }
    }
}
