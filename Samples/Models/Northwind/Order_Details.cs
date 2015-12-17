using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Samples.Models.Northwind {
    [Table("Order Details")]
    public partial class Order_Details {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public float Discount { get; set; }
        public short Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        [ForeignKey("OrderID")]
        [InverseProperty("Order_Details")]
        public virtual Order Order { get; set; }

        [ForeignKey("ProductID")]
        [InverseProperty("Order_Details")]
        public virtual Product Product { get; set; }
    }
}
