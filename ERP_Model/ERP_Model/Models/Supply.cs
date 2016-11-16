﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_Model.Models
{
    public class Supply
    {
        [Key]
        public int SupplyId { get; set; }
        public virtual ICollection<Product> SupplyProducts { get; set; }
        public virtual ApplicationUser SupplyPerson { get; set; }
        public virtual ApplicationUser Supplier { get; set; }
    }

    public class SupplyItem
    {
        [Key]
        public Guid SupplyItemGuid { get; set; }
        public virtual Product SupplyItemProduct { get; set; }
        public virtual Supply SupplyItemSupply { get; set; }
        public int SupplyQuantity { get; set; }
    }
}