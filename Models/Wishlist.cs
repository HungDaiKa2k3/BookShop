using System;
using System.Collections.Generic;

namespace BookShop.Models;

public partial class Wishlist
{
    public int WishlistId { get; set; }

    public int? CustomerCustomerId { get; set; }

    public int? ProductProductId { get; set; }

    public virtual Customer? CustomerCustomer { get; set; }

    public virtual Product? ProductProduct { get; set; }
}
