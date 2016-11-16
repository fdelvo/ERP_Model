using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ERP_Model.Models;

namespace ERP_Model.Models
{
    public class Stock
    {
        [Key]
        public Guid StockGuid { get; set; }
        public string StockName { get; set; }
        public string StockLocation { get; set; }
        public string StockMethod { get; set; }
    }

    public class StockItem
    {
        [Key]
        public Guid StockItemGuid { get; set; }
        public virtual Product StockItemProduct { get; set; }
        public int StockItemMinimumQuantity { get; set; }
        public int StockItemMaximumQuantity { get; set; }
    }

    public class StockTransaction
    {
        [Key]
        public Guid StockTransactionGuid { get; set; }
        public virtual StockItem StockTransactionItem { get; set; }
        public int StockTransactionQuantity { get; set; }
        public DateTime StockTransactionDate { get; set; }
        public virtual ApplicationUser StockTransactionUser { get; set; }
    }
}