using System;
using System.ComponentModel.DataAnnotations;

namespace ERP_Model.Models
{
    public class Address
    {
        [Key]
        public Guid AddressGuid { get; set; }
        [Required]
        public string AddressDescription { get; set; }
        [Required]
        public string AddressStreet { get; set; }
        [Required]
        public string AddressZipCode { get; set; }
        [Required]
        public string AddressCity { get; set; }
        [Required]
        public string AddressCountry { get; set; }

        [EmailAddress]
        public string AddressEmail { get; set; }

        public long AddressPhone { get; set; }
        public string AddressLastName { get; set; }
        public string AddressForName { get; set; }
        public string AddressCompany { get; set; }
        public bool AddressDeleted { get; set; }
    }
}