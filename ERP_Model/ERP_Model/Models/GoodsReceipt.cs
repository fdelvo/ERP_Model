using System;
using System.ComponentModel.DataAnnotations;

namespace ERP_Model.Models
{
    public class GoodsReceipt
    {
        [Key]
        public Guid GoodsReceiptGuid { get; set; }

        public virtual Supply GoodsReceiptSupply { get; set; }
        public bool GoodsReceiptDeleted { get; set; }
    }

    public class GoodsReceiptItem
    {
        [Key]
        public Guid GoodsReceiptItemGuid { get; set; }

        public virtual GoodsReceipt GoodsReceiptItemGoodsReceipt { get; set; }
        public virtual SupplyItem GoodsReceiptItemSupplyItem { get; set; }
        public int GoodsReceiptItemQuantity { get; set; }
        public bool GoodsReceiptItemDeleted { get; set; }
    }
}