using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ERP_Model.Models;

namespace ERP_Model.ViewModels
{
    public class GoodsReceiptViewModel
    {
        [Key]
        public Guid GoodsReceiptGuid { get; set; }

        public Guid GoodsReceiptSupply { get; set; }
        public List<GoodsReceiptItemViewModel> GoodsReceiptItems { get; set; }
    }

    public class GoodsReceiptItemViewModel
    {
        [Key]
        public Guid GoodsReceiptItemGuid { get; set; }

        public Guid GoodsReceiptItemGoodsReceipt { get; set; }
        public Guid GoodsReceiptItemSupplyItem { get; set; }
        public int GoodsReceiptItemQuantity { get; set; }
    }

    public class GoodsReceiptDetailsViewModel
    {
        public GoodsReceipt GoodsReceipt { get; set; }
        public List<GoodsReceiptItem> GoodsReceiptItems { get; set; }
    }
}