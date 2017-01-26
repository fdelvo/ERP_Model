using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ERP_Model.Models;

namespace ERP_Model.ViewModels
{
    public class DeliveryViewModel
    {
        [Key]
        public Guid DeliveryGuid { get; set; }
        [Required]
        public Guid DeliveryOrder { get; set; }
        [Required]
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

    public class DeliveryNoteDetailsViewModel
    {
        public Delivery DeliveryNote { get; set; }
        public List<DeliveryItem> DeliveryItems { get; set; }
    }
}