using System;
using System.ComponentModel.DataAnnotations;

namespace ERP_Model.Models
{
    public class Customer
    {
        [Key]
        public Guid CustomerGuid { get; set; }
        [Required]
        public string CustomerForName { get; set; }
        [Required]
        public string CustomerLastName { get; set; }
        public string CustomerCompany { get; set; }
        [Required]
        public virtual Address CustomerAddress { get; set; }
        public bool CustomerDeleted { get; set; }
    }
}