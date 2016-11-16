﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP_Model.Models
{
    public class GoodsReceipt
    {
        [Key]
        public Guid GoodsReceiptGuid { get; set; }
        public virtual Supply GoodsReceiptSupply { get; set; }
    }

    public class GoodsReceiptItem
    {
        [Key]
        public Guid GoodsReceiptItemGuid { get; set; }
        public virtual GoodsReceipt GoodsReceiptItemGoodsReceipt { get; set; }
        public virtual SupplyItem GoodsReceiptItemSupplyItem { get; set; }
        public int GoodsReceiptItemQuantity { get; set; }
    }
}