using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ERP_Model.Models;

namespace ERP_Model.ViewModels
{
    public class SupplyViewModel
    {
        [Key]
        public Guid SupplyGuid { get; set; }

        public virtual Supplier SupplySupplier { get; set; }
        public DateTime SupplyDate { get; set; }
        public DateTime SupplyDeliveryDate { get; set; }
        public float SupplyValue { get; set; }
    }

    public class NewSupplyViewModel
    {
        [Key]
        public Guid SupplyGuid { get; set; }

        [Required]
        public virtual Supplier SupplySupplier { get; set; }

        public DateTime SupplyDate { get; set; }

        [Required]
        public DateTime SupplyDeliveryDate { get; set; }

        [Required]
        public List<SupplyItemProductViewModel> SupplyItems { get; set; }
    }

    public class SupplyItemProductViewModel
    {
        [Key]
        public Guid ProductGuid { get; set; }

        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public int SupplyQuantity { get; set; }
    }

    public class SupplyDetailsViewModel
    {
        public Supply Supply { get; set; }
        public List<SupplyItem> SupplyItems { get; set; }
    }
}