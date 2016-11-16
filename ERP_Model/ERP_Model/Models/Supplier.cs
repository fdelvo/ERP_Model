using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_Model.Models
{
    public class Supplier
    {
        [Key]
        public Guid SupplierGuid { get; set; }
        public string SupplierName { get; set; }
        public virtual Address SupplierAddress { get; set; }
    }
}