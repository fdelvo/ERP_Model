using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_Model.ViewModels
{
    public class DeliveryViewModel
    {
        [Key]
        public Guid DeliveryGuid { get; set; }
        public Guid DeliveryOrder { get; set; }
        public List<DeliveryItemViewModel> DeliveryItems { get; set; }
    }

    public class DeliveryItemViewModel
    {
        [Key]
        public Guid DeliveryItemGuid { get; set; }
        public Guid DeliveryItemDelivery { get; set; }
        public Guid DeliveryItemOrderItem { get; set; }
        public int DeliveryItemQuantity { get; set; }
    }
}