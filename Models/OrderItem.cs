using System;
using System.Collections.Generic;

namespace BookShop.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public decimal? Total=>Quantity*Price;

    public int? ProductProdu { get; set; }

    public int? OrderOrderI { get; set; }

    public virtual Order? OrderOrderINavigation { get; set; }

    public virtual Product? ProductProduNavigation { get; set; }
}
