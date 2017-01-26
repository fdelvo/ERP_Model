using System;
using System.ComponentModel.DataAnnotations;

namespace ERP_Model.Models
{
    public class Product
    {
        [Key]
        public Guid ProductGuid { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public float ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public bool ProductDeleted { get; set; }
    }
}