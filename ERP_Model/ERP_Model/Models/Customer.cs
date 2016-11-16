using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_Model.Models
{
    public class Customer
    {
        [Key]
        public Guid CustomerGuid { get; set; }
        public string CustomerName { get; set; }
        public virtual Address CustomerAddress { get; set; }
    }
}