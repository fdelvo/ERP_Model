using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}