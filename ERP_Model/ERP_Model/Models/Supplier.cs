using System;
using System.ComponentModel.DataAnnotations;

namespace ERP_Model.Models
{
    public class Supplier
    {
        [Key]
        public Guid SupplierGuid { get; set; }
        public string SupplierForName { get; set; }
        public string SupplierLastName { get; set; }
        [Required]
        public string SupplierCompany { get; set; }
        [Required]
        public virtual Address SupplierAddress { get; set; }
        public bool SupplierDeleted { get; set; }
    }
}