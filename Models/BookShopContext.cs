using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Models;

public partial class BookShopContext : DbContext
{
    public BookShopContext()
    {
    }

    public BookShopContext(DbContextOptions<BookShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=G15\\SQLEXPRESS;Initial Catalog=BookShop;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__2EF52A27FAF15F43");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.CustomerCustomerId).HasColumnName("Customer_customer_id");
            entity.Property(e => e.ProductProductId).HasColumnName("Product_product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.CustomerCustomer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CustomerCustomerId)
                .HasConstraintName("FK__Cart__Customer_c__59063A47");

            entity.HasOne(d => d.ProductProduct).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductProductId)
                .HasConstraintName("FK__Cart__Product_pr__59FA5E80");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B437DA6B4E");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB8594BCBBA8");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__4659622908114795");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CustomerCusto).HasColumnName("Customer_custo");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.PaymentPayme).HasColumnName("Payment_payme");
            entity.Property(e => e.ShipmentShipm).HasColumnName("Shipment_shipm");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");

            entity.HasOne(d => d.CustomerCustoNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerCusto)
                .HasConstraintName("FK__Order__Customer___5AEE82B9");

            entity.HasOne(d => d.PaymentPaymeNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentPayme)
                .HasConstraintName("FK__Order__Payment_p__5BE2A6F2");

            entity.HasOne(d => d.ShipmentShipmNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShipmentShipm)
                .HasConstraintName("FK__Order__Shipment___5CD6CB2B");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__Order_It__3764B6BC6BECADED");

            entity.ToTable("Order_Item");

            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.OrderOrderI).HasColumnName("Order_order_i");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductProdu).HasColumnName("Product_produ");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.OrderOrderINavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderOrderI)
                .HasConstraintName("FK__Order_Ite__Order__5EBF139D");

            entity.HasOne(d => d.ProductProduNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductProdu)
                .HasConstraintName("FK__Order_Ite__Produ__5DCAEF64");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__ED1FC9EA3F316B62");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CustomerCustome).HasColumnName("Customer_custome");
            entity.Property(e => e.PaymentDate)
                .HasColumnType("datetime")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(100)
                .HasColumnName("payment_method");

            entity.HasOne(d => d.CustomerCustomeNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CustomerCustome)
                .HasConstraintName("FK__Payment__Custome__5FB337D6");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__47027DF54692571A");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Author)
                .HasMaxLength(200)
                .HasColumnName("author");
            entity.Property(e => e.CategoryCatego).HasColumnName("Category_catego");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductDate).HasColumnName("product_date");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("product_name");
            entity.Property(e => e.Publish)
                .HasMaxLength(200)
                .HasColumnName("publish");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.CategoryCategoNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryCatego)
                .HasConstraintName("FK__Product__Categor__60A75C0F");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__Sale__E1EB00B29CA30EE2");

            entity.ToTable("Sale");

            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.DiscountPercentage).HasColumnName("discount_percentage");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.ProductProductId).HasColumnName("Product_product_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.ProductProduct).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ProductProductId)
                .HasConstraintName("FK__Sale__Product_pr__7D439ABD");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.ShipmentId).HasName("PK__Shipment__41466E59306DCA3B");

            entity.ToTable("Shipment");

            entity.Property(e => e.ShipmentId).HasColumnName("shipment_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.CustomerCustom).HasColumnName("Customer_custom");
            entity.Property(e => e.ShipmentDate)
                .HasColumnType("datetime")
                .HasColumnName("shipment_date");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("zip_code");

            entity.HasOne(d => d.CustomerCustomNavigation).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.CustomerCustom)
                .HasConstraintName("FK__Shipment__Custom__619B8048");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.WishlistId).HasName("PK__Wishlist__6151514ED3A9788B");

            entity.ToTable("Wishlist");

            entity.Property(e => e.WishlistId).HasColumnName("wishlist_id");
            entity.Property(e => e.CustomerCustomerId).HasColumnName("Customer_customer_id");
            entity.Property(e => e.ProductProductId).HasColumnName("Product_product_id");

            entity.HasOne(d => d.CustomerCustomer).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.CustomerCustomerId)
                .HasConstraintName("FK__Wishlist__Custom__628FA481");

            entity.HasOne(d => d.ProductProduct).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.ProductProductId)
                .HasConstraintName("FK__Wishlist__Produc__6383C8BA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
