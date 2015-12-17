using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Samples.Models.Northwind {

    [Table("Products")]
    public partial class Product {
        public Product() {
            Order_Details = new HashSet<Order_Details>();
        }

        [Key]
        public int ProductID { get; set; }
        public int? CategoryID { get; set; }
        public bool Discontinued { get; set; }
        [Required]
        [MaxLength(40)]
        public string ProductName { get; set; }
        [MaxLength(20)]
        public string QuantityPerUnit { get; set; }
        public short? ReorderLevel { get; set; }
        public int? SupplierID { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection<Order_Details> Order_Details { get; set; }

        [ForeignKey("CategoryID")]
        [InverseProperty("Products")]
        public virtual Category Category { get; set; }

        [ForeignKey("SupplierID")]
        [InverseProperty("Products")]
        public virtual Supplier Supplier { get; set; }
    }
}
