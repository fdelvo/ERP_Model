using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ERP_Model.Models;

namespace ERP_Model.ViewModels
{
    public class OrderViewModel
    {
        [Key]
        public Guid OrderGuid { get; set; }

        public virtual Customer OrderCustomer { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderDeliveryDate { get; set; }
        public float OrderValue { get; set; }
    }

    public class NewOrderViewModel
    {
        [Key]
        public Guid OrderGuid { get; set; }

        [Required]
        public virtual Customer OrderCustomer { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime OrderDeliveryDate { get; set; }

        [Required]
        public List<OrderItemProductViewModel> OrderItems { get; set; }
    }

    public class OrderItemProductViewModel
    {
        [Key]
        public Guid ProductGuid { get; set; }

        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public int OrderQuantity { get; set; }
    }

    public class OrderDetailsViewModel
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}