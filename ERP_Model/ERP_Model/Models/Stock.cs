using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_Model.Models
{
    public class Stock
    {
        [Key]
        public Guid StockGuid { get; set; }
        public virtual Product StockProduct { get; set; }
        public int StockQuantity { get; set; }
    }
}