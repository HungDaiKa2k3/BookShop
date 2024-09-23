using BookShop.Models;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace BookShop.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BookShopContext db;
        public CustomerController(BookShopContext context)
        {
            db = context;
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var cus = db.Customers.Where(c => c.Email == model.Email && c.Password == model.Password).FirstOrDefault();
                if (cus != null)
                {
                    HttpContext.Session.SetInt32("UserId", cus.CustomerId);
                    return Redirect("/products");
                }
                else
                {
                    ViewBag.msg = "Tên đăng nhập hoặc mật khẩu không chính xác";
                }
            }
            return View();
        }


        public IActionResult addWishlist(int productId)
        {
            var customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId != null)
            {
                var wishItem = db.Wishlists.Where(w => w.ProductProductId == productId && w.CustomerCustomerId == customerId).FirstOrDefault();
                if (wishItem == null)
                {
                    db.Wishlists.Add(new Wishlist()
                    {
                        CustomerCustomerId = customerId,
                        ProductProductId = productId,
                    });
                    db.SaveChanges();
                }
            } 
            return ViewComponent("Wishlist");
        }

        public IActionResult removeWishlist(int productId)
        {
            var customerId = HttpContext.Session.GetInt32("UserId");

            var wishItem = db.Wishlists.Where(w => w.ProductProductId == productId && w.CustomerCustomerId == customerId).FirstOrDefault();
            if (wishItem != null)
            {
                db.Remove(wishItem);
                db.SaveChanges();
            }
            return ViewComponent("Wishlist");
        }

        [HttpGet("/mywishlist")]
        public IActionResult MyWishlist()
        {
            var customerId = HttpContext.Session.GetInt32("UserId");
            var result = db.Wishlists.Where(w => w.CustomerCustomerId.Equals(customerId)).OrderByDescending(w => w.WishlistId)
                .Join(db.Products, w => w.ProductProductId, p => p.ProductId, (w, p) => new WishlistVM()
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Image = p.Image,
                });

            var result2 = db.Wishlists.Where(w => w.CustomerCustomerId.Equals(customerId)).OrderByDescending(w => w.WishlistId)
                .Select(w => new WishlistVM()
                {
                    ProductId = w.ProductProduct.ProductId,
                    ProductName = w.ProductProduct.ProductName,
                    Price = w.ProductProduct.Price,
                    Image = w.ProductProduct.Image,
                });
            return View(result2.ToList());
        }

        public IActionResult addCart(int productId, int quantity)
        {
            var customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId != null)
            {
                var cartItem = db.Carts.Where(c => c.ProductProductId == productId && c.CustomerCustomerId == customerId).FirstOrDefault();

                if (cartItem == null)
                {
                    db.Carts.Add(new Cart()
                    {
                        CustomerCustomerId = customerId,
                        ProductProductId = productId,
                        Quantity = quantity,
                    });
                    db.SaveChanges();
                }
                else
                {
                    cartItem.Quantity += quantity;
                    db.SaveChanges();
                }
            }
            var newItem = from c in db.Carts
                          where c.CustomerCustomerId == customerId && c.ProductProductId == productId
                          join p in db.Products on c.ProductProductId equals p.ProductId
                          select new CartVM()
                          {
                              CartId = c.CartId,
                              CustomerId = customerId,
                              ProductId = productId,
                              Quantity = c.Quantity,
                              Image = p.Image,
                              Price = p.Price,
                              ProductName = p.ProductName,
                          };

            return Json(new {result=newItem.Single()});
        }
        public IActionResult removeCart(int productId)
        {
            var customerId = HttpContext.Session.GetInt32("UserId");

            var cartItem = db.Carts.Where(c => c.ProductProductId == productId && c.CustomerCustomerId == customerId).FirstOrDefault();
            if (cartItem != null)
            {
                var product = db.Products.Where(p => p.ProductId == productId).FirstOrDefault();
                var delItem=new CartVM() { 
                    CartId = cartItem.CartId,
                    ProductId = productId,
                    CustomerId = customerId,
                    Image=product.Image,
                    Price=product.Price,
                    ProductName = product.ProductName,
                    Quantity=cartItem.Quantity,
                };
                db.Remove(cartItem);
                db.SaveChanges();
                return Json(new { status = "success",result=delItem});
            }
            return Json(new {status="failed"});
        }
        [HttpGet("/mycart")]
        public IActionResult MyCart()
        {
            var customerId = HttpContext.Session.GetInt32("UserId");
            var result = db.Carts.Where(c => c.CustomerCustomerId.Equals(customerId)).OrderByDescending(c => c.CartId)
                .Join(db.Products, c => c.ProductProductId, p => p.ProductId, (c, p) => new CartVM()
                {
                    ProductId = p.ProductId,
                    CustomerId=customerId,
                    CartId=c.CartId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Image = p.Image,
                    Quantity=c.Quantity,
                });
            return View(result.ToList());
        }

        public IActionResult removeCart2(int productId)
        {
            var customerId = HttpContext.Session.GetInt32("UserId");

            var cartItem = db.Carts.Where(c => c.ProductProductId == productId && c.CustomerCustomerId == customerId).FirstOrDefault();
            if (cartItem != null)
            {
                db.Remove(cartItem);
                db.SaveChanges();
            }
      
            var result = db.Carts.Where(c => c.CustomerCustomerId == customerId).OrderByDescending(c => c.CartId).
                Join(db.Products, c => c.ProductProductId, p => p.ProductId, (c, p) => new CartVM()
                {
                    ProductId = p.ProductId,
                    CustomerId = customerId,
                    CartId = c.CartId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Image = p.Image,
                    Quantity = c.Quantity,
                }).ToList();
            return PartialView("CartList",result);

        }

        public IActionResult updateCart(int productId, int quantity)
        {
            var customerId = HttpContext.Session.GetInt32("UserId");

            var cartItem = db.Carts.Where(c => c.ProductProductId == productId && c.CustomerCustomerId == customerId).FirstOrDefault();
            if (cartItem != null)
            {
                if (quantity == 0)
                {
                    db.Remove(cartItem);
                }else if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                }         
                db.SaveChanges();
            }

            var result = db.Carts.Where(c => c.CustomerCustomerId == customerId).OrderByDescending(c => c.CartId).
                Join(db.Products, c => c.ProductProductId, p => p.ProductId, (c, p) => new CartVM()
                {
                    ProductId = p.ProductId,
                    CustomerId = customerId,
                    CartId = c.CartId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Image = p.Image,
                    Quantity = c.Quantity,
                }).ToList();
            return PartialView("CartList", result);

        }

        [HttpGet("/myorder")]
        public IActionResult MyOrder()
        {
            var customerId = HttpContext.Session.GetInt32("UserId");

            var result = from o in db.Orders
            where o.CustomerCusto == customerId
            select new OrderVM()
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                CustomerId = o.CustomerCustoNavigation.CustomerId, 
                CustomerName = o.CustomerCustoNavigation.Name, 
                PhoneNumber = o.CustomerCustoNavigation.PhoneNumber,
                ShipmentId = o.ShipmentShipmNavigation.ShipmentId, 
                State = o.ShipmentShipmNavigation.State
            };

            var result2 = from o in db.Orders
            where o.CustomerCusto == customerId
            join s in db.Shipments on o.ShipmentShipm equals s.ShipmentId
            join c in db.Customers on o.CustomerCusto equals c.CustomerId
            select new OrderVM()
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                CustomerId = c.CustomerId,
                CustomerName = c.Name,
                PhoneNumber = c.PhoneNumber,
                ShipmentId = s.ShipmentId,
                State = s.State
            };

            var result3 = db.Orders.Where(o => o.CustomerCusto.Equals(customerId))
                .Join(db.Shipments, o => o.ShipmentShipm, s => s.ShipmentId, (o, s) => new { o, s })
                .Join(db.Customers, o2 => o2.o.CustomerCusto, c => c.CustomerId, (o2, c) => new OrderVM()
                {
                    OrderId = o2.o.OrderId,
                    OrderDate = o2.o.OrderDate,
                    TotalPrice = o2.o.TotalPrice,
                    CustomerId = c.CustomerId,
                    CustomerName = c.Name,
                    PhoneNumber = c.PhoneNumber,
                    ShipmentId = o2.s.ShipmentId,
                    State = o2.s.State
                });

            var result4 = db.Orders.Where(o => o.CustomerCusto.Equals(customerId))
                .Select(o => new OrderVM()
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    CustomerId = o.CustomerCustoNavigation.CustomerId,
                    CustomerName = o.CustomerCustoNavigation.Name,
                    PhoneNumber = o.CustomerCustoNavigation.PhoneNumber,
                    ShipmentId = o.ShipmentShipmNavigation.ShipmentId,
                    State = o.ShipmentShipmNavigation.State
                });

            var result5 = db.Orders.Where(o => o.CustomerCusto.Equals(customerId))
                .Include(o=>o.ShipmentShipmNavigation)
                .Include(o=>o.CustomerCustoNavigation)
                .Select(o => new OrderVM()
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    CustomerId = o.CustomerCustoNavigation.CustomerId,
                    CustomerName = o.CustomerCustoNavigation.Name,
                    PhoneNumber = o.CustomerCustoNavigation.PhoneNumber,
                    ShipmentId = o.ShipmentShipmNavigation.ShipmentId,
                    State = o.ShipmentShipmNavigation.State,
                });

            return View(result5); 
        }

        [HttpGet("/orderdetail")]
        public IActionResult OrderDetail(int orderId)
        {
            var customerId = HttpContext.Session.GetInt32("UserId");
            var result = db.Orders
               .Include(o => o.OrderItems)               
               .ThenInclude(oi => oi.ProductProduNavigation) 
               .Where(o => o.OrderId == orderId && o.CustomerCusto == customerId)
               .Select(o => new OrderDetailVM()
               {
                   OrderId = o.OrderId,
                   TotalPrice = o.TotalPrice,
                   CustomerName = o.CustomerCustoNavigation.Name,
                   OrderItems = o.OrderItems
               })
               .FirstOrDefault();

            var result2 = from o in db.Orders
                          join oi in db.OrderItems on o.OrderId equals oi.OrderOrderI
                          join p in db.Products on oi.ProductProdu equals p.ProductId
                          where o.OrderId == orderId && o.CustomerCusto == customerId
                         select new OrderDetailVM()
                         {
                             OrderId = 1,
                             TotalPrice = o.TotalPrice,                           
                             CustomerName = o.CustomerCustoNavigation.Name,
                             ShipmentAddress=o.ShipmentShipmNavigation.Address,
                             PaymentMethod=o.PaymentPaymeNavigation.PaymentMethod,
                             OrderItems = o.OrderItems
                         };
            return View(result2.FirstOrDefault());
        }

        [HttpGet("/checkout")]
        public IActionResult Checkout()
        {
            var customerId = HttpContext.Session.GetInt32("UserId");
            var result = db.Carts.Where(c => c.CustomerCustomerId.Equals(customerId)).OrderByDescending(c => c.CartId)
               .Join(db.Products, c => c.ProductProductId, p => p.ProductId, (c, p) => new CartVM()
               {
                   ProductId = p.ProductId,
                   CustomerId = customerId,
                   CartId = c.CartId,
                   ProductName = p.ProductName,
                   Price = p.Price,
                   Image = p.Image,
                   Quantity = c.Quantity,                 
               });
            var kq = new CheckoutVM()
            {
                Customer=db.Customers.Where(c=>c.CustomerId==customerId).FirstOrDefault(),
                Shipment=null,
                Payment=null,
                CartVMs=result.ToList(),
            };



            return View(kq);
        }

        [HttpPost("/checkout")]
        public IActionResult Checkout(CheckoutVM model)
        {
            var customerId = HttpContext.Session.GetInt32("UserId");
            Payment payment = new Payment()
            {
                PaymentMethod = model.Payment.PaymentMethod,
                Amount = model.CartVMs.Sum(c => c.Total),
                CustomerCustome = customerId,
            };

            db.Add(payment);
            db.SaveChanges();

            Shipment shipment = new Shipment()
            {
                Address = model.Shipment.Address,
                CustomerCustom = customerId,
                State = "Chờ xử lý",
            };
            db.Add(shipment);
            db.SaveChanges();

            Order order = new Order()
            {
                OrderDate = DateTime.Now,
                TotalPrice = model.CartVMs.Sum(c => c.Total),
                CustomerCusto = customerId,
                PaymentPayme = payment.PaymentId,
                ShipmentShipm = shipment.ShipmentId,
            };
            db.Add(order);
            db.SaveChanges();

            foreach (var item in model.CartVMs)
            {
                var orderItem = new OrderItem()
                {
                    OrderOrderI = order.OrderId,
                    ProductProdu = item.ProductId,
                    Price = item.Price,
                    Quantity = item.Quantity,

                };
                db.Add(orderItem);
            }
            db.SaveChanges();

            // Xóa giỏ hàng
            var cartItems = db.Carts.Where(c => c.CustomerCustomerId == customerId).ToList();
            db.Carts.RemoveRange(cartItems);
            db.SaveChanges();

            return Redirect("/");
        }

    }
}
