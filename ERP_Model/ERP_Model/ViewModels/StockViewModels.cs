using System;
using System.ComponentModel.DataAnnotations;
using ERP_Model.Models;

namespace ERP_Model.ViewModels
{
    public class StockItemViewModel
    {
        public Guid StockItemGuid { get; set; }
        public virtual Product StockItemProduct { get; set; }
        public virtual Stock StockItemStock { get; set; }
        public int StockItemMinimumQuantity { get; set; }
        public int StockItemMaximumQuantity { get; set; }
        public int StockItemQuantity { get; set; }
        public Order Order { get; set; }
        public Supply Supply { get; set; }
    }

    public class StockViewModel
    {
        public Guid StockGuid { get; set; }

        [Required]
        public string StockName { get; set; }

        [Required]
        public virtual Address StockAddress { get; set; }

        public string StockMethod { get; set; }
        public float StockValue { get; set; }
    }

    public class EditStockViewModel
    {
        public Guid StockGuid { get; set; }
        public string StockName { get; set; }
        public Guid StockAddress { get; set; }
        public string StockMethod { get; set; }
    }
}