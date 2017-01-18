using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_Model.Models
{
    public class Address
    {
        [Key]
        public Guid AddressGuid { get; set; }
        public string AddressDescription { get; set; }
        public string AddressStreet { get; set; }
        public string AddressZipCode { get; set; }
        public string AddressCity { get; set; }
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