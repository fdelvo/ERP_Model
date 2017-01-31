using ERP_Model.Models;

namespace ERP_Model.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public StockItem ProductStockItem { get; set; }
    }
}