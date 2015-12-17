using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Samples.Models.Northwind {

    [Table("Employees")]
    public partial class Employee {
        public Employee() {
            Orders = new HashSet<Order>();
        }

        [Key]
        public int EmployeeID { get; set; }
        [MaxLength(60)]
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        [MaxLength(15)]
        public string City { get; set; }
        [MaxLength(15)]
        public string Country { get; set; }
        [MaxLength(4)]
        public string Extension { get; set; }
        [Required]
        [MaxLength(10)]
        public string FirstName { get; set; }
        public DateTime? HireDate { get; set; }
        [MaxLength(24)]
        public string HomePhone { get; set; }
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }
        public string Notes { get; set; }
        public byte[] Photo { get; set; }
        [MaxLength(255)]
        public string PhotoPath { get; set; }
        [MaxLength(10)]
        public string PostalCode { get; set; }
        [MaxLength(15)]
        public string Region { get; set; }
        public int? ReportsTo { get; set; }
        [MaxLength(30)]
        public string Title { get; set; }
        [MaxLength(25)]
        public string TitleOfCourtesy { get; set; }

        [InverseProperty("Employee")]
        public virtual ICollection<Order> Orders { get; set; }

        [ForeignKey("ReportsTo")]
        [InverseProperty("SubordinateEmployees")]
        public virtual Employee SupervisorEmployee { get; set; }

        [InverseProperty("SupervisorEmployee")]
        public virtual ICollection<Employee> SubordinateEmployees { get; set; }
    }
}
