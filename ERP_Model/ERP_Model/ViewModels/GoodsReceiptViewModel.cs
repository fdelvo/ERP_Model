using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ERP_Model.Models;

namespace ERP_Model.ViewModels
{
    public class GoodsReceiptViewModel
    {
        [Key]
        public Guid GoodsReceiptGuid { get; set; }

        [Required]
        public Guid GoodsReceiptSupply { get; set; }

        [Required]
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