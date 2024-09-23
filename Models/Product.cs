using System;
using System.Collections.Generic;

namespace BookShop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? Description { get; set; }

    public int? Price { get; set; }

    public int? Stock { get; set; }

    public int? CategoryCatego { get; set; }

    public string? Publish { get; set; }

    public string? Author { get; set; }

    public DateOnly? ProductDate { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category? CategoryCategoNavigation { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
