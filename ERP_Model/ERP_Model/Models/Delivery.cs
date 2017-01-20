using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_Model.Models
{
    public class Delivery
    {
        [Key]
        public Guid DeliveryGuid { get; set; }

        [Index(IsUnique = true)]
        public virtual Order DeliveryOrder { get; set; }

        public bool DeliveryDeleted { get; set; }
    }

    public class DeliveryItem
    {
        [Key]
        public Guid DeliveryItemGuid { get; set; }

        public virtual Delivery DeliveryItemDelivery { get; set; }
        public virtual OrderItem DeliveryItemOrderItem { get; set; }
        public int DeliveryItemQuantity { get; set; }
        public bool DeliveryItemDeleted { get; set; }
    }
}