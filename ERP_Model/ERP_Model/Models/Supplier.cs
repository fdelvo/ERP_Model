using System;
using System.ComponentModel.DataAnnotations;

namespace ERP_Model.Models
{
    public class Supplier
    {
        [Key]
        public Guid SupplierGuid { get; set; }

        public string SupplierName { get; set; }
        public virtual Address SupplierAddress { get; set; }
        public bool SupplierDeleted { get; set; }
    }
}