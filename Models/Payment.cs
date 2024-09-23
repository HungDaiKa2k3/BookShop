using System;
using System.Collections.Generic;

namespace BookShop.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public int? Amount { get; set; }

    public int? CustomerCustome { get; set; }

    public virtual Customer? CustomerCustomeNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
