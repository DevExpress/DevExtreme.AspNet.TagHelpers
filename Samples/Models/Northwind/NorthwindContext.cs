using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Samples.Models.Northwind {
    public partial class NorthwindContext : DbContext {
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseSqlServer(@"data source=.\SQLEXPRESS;initial catalog=NORTHWND;integrated security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Category>(entity => {
                entity.HasIndex(e => e.CategoryName).HasName("CategoryName");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Picture).HasColumnType("image");
            });

            modelBuilder.Entity<Customer>(entity => {
                entity.HasIndex(e => e.City).HasName("City");

                entity.HasIndex(e => e.Region).HasName("Region");

                entity.HasIndex(e => new { e.CompanyName }).HasName("CompanyName");

                entity.Property(e => e.CustomerID).HasColumnType("nchar(5)");
            });

            modelBuilder.Entity<Employee>(entity => {
                entity.HasIndex(e => e.LastName).HasName("LastName");

                entity.HasIndex(e => new { e.PostalCode }).HasName("PostalCode");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.HireDate).HasColumnType("datetime");

                entity.Property(e => e.Notes).HasColumnType("ntext");

                entity.Property(e => e.Photo).HasColumnType("image");
            });

            modelBuilder.Entity<Order_Details>(entity => {
                entity.HasKey(e => new { e.OrderID, e.ProductID });

                entity.HasIndex(e => e.OrderID).HasName("OrdersOrder_Details");

                entity.HasIndex(e => e.ProductID).HasName("ProductsOrder_Details");

                entity.Property(e => e.Discount).HasDefaultValueSql("0");

                entity.Property(e => e.Quantity).HasDefaultValueSql("1");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("money")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<Order>(entity => {
                entity.HasIndex(e => e.CustomerID).HasName("CustomersOrders");

                entity.HasIndex(e => e.EmployeeID).HasName("EmployeesOrders");

                entity.HasIndex(e => e.OrderDate).HasName("OrderDate");

                entity.HasIndex(e => e.ShipPostalCode).HasName("ShipPostalCode");

                entity.HasIndex(e => e.ShipVia).HasName("ShippersOrders");

                entity.HasIndex(e => e.ShippedDate).HasName("ShippedDate");

                entity.Property(e => e.CustomerID).HasColumnType("nchar(5)");

                entity.Property(e => e.Freight)
                    .HasColumnType("money")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.RequiredDate).HasColumnType("datetime");

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Product>(entity => {
                entity.HasIndex(e => e.CategoryID).HasName("CategoryID");

                entity.HasIndex(e => e.ProductName).HasName("ProductName");

                entity.HasIndex(e => e.SupplierID).HasName("SuppliersProducts");

                entity.Property(e => e.Discontinued).HasDefaultValueSql("0");

                entity.Property(e => e.ReorderLevel).HasDefaultValueSql("0");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("money")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UnitsInStock).HasDefaultValueSql("0");

                entity.Property(e => e.UnitsOnOrder).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<Supplier>(entity => {
                entity.Property(e => e.HomePage).HasColumnType("ntext");
            });
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Order_Details> Order_Details { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
    }
}