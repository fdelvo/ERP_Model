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
        public Guid OrderGuid { get; set; }
        public virtual ApplicationUser OrderCustomer { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderDeliveryDate { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public Guid OrderItemGuid { get; set; }
        public virtual Product OrderItem { get; set; }
        public virtual Order OrderItemOrder { get; set; }
        public int OrderQuantity { get; set; }
    }
}