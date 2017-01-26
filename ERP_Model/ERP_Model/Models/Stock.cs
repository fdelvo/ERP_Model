using System;
using System.ComponentModel.DataAnnotations;

namespace ERP_Model.Models
{
    public class Stock
    {
        [Key]
        public Guid StockGuid { get; set; }
        [Required]
        public string StockName { get; set; }
        [Required]
        public virtual Address StockAddress { get; set; }
        public string StockMethod { get; set; }
        public bool StockDeleted { get; set; }
    }

    public class StockItem
    {
        [Key]
        public Guid StockItemGuid { get; set; }

        public virtual Product StockItemProduct { get; set; }
        public virtual Stock StockItemStock { get; set; }
        public int StockItemMinimumQuantity { get; set; }
        public int StockItemMaximumQuantity { get; set; }
        public bool StockItemDeleted { get; set; }
    }

    public class StockTransaction
    {
        [Key]
        public Guid StockTransactionGuid { get; set; }

        public virtual StockItem StockTransactionItem { get; set; }
        public int StockTransactionQuantity { get; set; }
        public DateTime StockTransactionDate { get; set; }
        public virtual ApplicationUser StockTransactionUser { get; set; }
        public bool StockTransactionDeleted { get; set; }
        public Order StockTransactionOrder { get; set; }
        public Supply StockTransactionSupply { get; set; }
    }
}