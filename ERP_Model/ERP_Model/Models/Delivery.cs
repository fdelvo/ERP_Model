using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_Model.Models
{
    public class Delivery
    {
        [Key]
        public Guid DeliveryGuid { get; set; }
        public virtual Order DeliveryOrder { get; set; }
    }

    public class DeliveryItem
    {
        [Key]
        public Guid DeliveryItemGuid { get; set; }
        public virtual Delivery DeliveryItemDelivery { get; set; }
        public virtual OrderItem DeliveryItemOrderItem { get; set; }
        public int DeliveryItemQuantity { get; set; }
    }
}