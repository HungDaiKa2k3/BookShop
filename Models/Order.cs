using System;
using System.Collections.Generic;

namespace BookShop.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? TotalPrice { get; set; }

    public int? CustomerCusto { get; set; }

    public int? PaymentPayme { get; set; }

    public int? ShipmentShipm { get; set; }

    public virtual Customer? CustomerCustoNavigation { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Payment? PaymentPaymeNavigation { get; set; }

    public virtual Shipment? ShipmentShipmNavigation { get; set; }
}
