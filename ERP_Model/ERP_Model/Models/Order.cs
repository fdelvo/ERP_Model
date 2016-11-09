using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_Model.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public virtual ICollection<Product> OrderProducts { get; set; }
        public virtual ApplicationUser OrderPerson { get; set; }
    }
}