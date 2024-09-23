using System;
using System.Collections.Generic;

namespace BookShop.Models;

public partial class Sale
{
    public int SaleId { get; set; }

    public int? DiscountPercentage { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Image { get; set; }

    public int? ProductProductId { get; set; }

    public virtual Product? ProductProduct { get; set; }
}
