using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI3_1.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required,DataType(DataType.EmailAddress), MaxLength(255)]
        public string Email { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}