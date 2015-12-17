using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Samples.Models.Northwind {

    [Table("Orders")]
    public partial class Order {
        public Order() {
            Order_Details = new HashSet<Order_Details>();
        }

        [Key]
        public int OrderID { get; set; }
        [MaxLength(5)]
        public string CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public decimal? Freight { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        [MaxLength(60)]
        public string ShipAddress { get; set; }
        [MaxLength(15)]
        public string ShipCity { get; set; }
        [MaxLength(15)]
        public string ShipCountry { get; set; }
        [MaxLength(40)]
        public string ShipName { get; set; }
        public DateTime? ShippedDate { get; set; }
        [MaxLength(10)]
        public string ShipPostalCode { get; set; }
        [MaxLength(15)]
        public string ShipRegion { get; set; }
        public int? ShipVia { get; set; }

        [InverseProperty("Order")]
        public virtual ICollection<Order_Details> Order_Details { get; set; }

        [ForeignKey("CustomerID")]
        [InverseProperty("Orders")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("EmployeeID")]
        [InverseProperty("Orders")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("ShipVia")]
        [InverseProperty("Orders")]
        public virtual Shipper Shipper { get; set; }
    }
}
