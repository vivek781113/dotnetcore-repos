using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebAPI3_1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}