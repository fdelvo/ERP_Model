using ERP_Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_Model.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public StockItem ProductStockItem { get; set; }
    }
}