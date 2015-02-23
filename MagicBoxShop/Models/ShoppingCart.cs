using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicBoxShop.Models;

namespace MagicBoxShop.Controllers
{
    public class ShoppingCart
    {
        PresentsEntities presentsDB = new PresentsEntities();

        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Product product, int count)
        {
            var cartItem =
                 presentsDB.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductId == product.ProductId);
            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductId = product.ProductId,
                    CartId = ShoppingCartId,
                    Count = count,
                    DateCreated = DateTime.Now
                };

                presentsDB.Carts.Add(cartItem);
            }
            else
            {
               cartItem.Count+=count;
            }
            presentsDB.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            var cartItem = presentsDB.Carts.Single(cart => cart.CartId == ShoppingCartId && cart.RecordId == id);
            int itemCount = 0;
            if (cartItem != null)
            {
               presentsDB.Carts.Remove(cartItem);
               presentsDB.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = presentsDB.Carts.Where(cart => cart.CartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                presentsDB.Carts.Remove(cartItem);
            }
            presentsDB.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return presentsDB.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            int? count = (from cartItems in presentsDB.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in presentsDB.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Product.Price).Sum();
            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Order = order,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count,
                    Product = item.Product
                };
                orderTotal += (item.Count * item.Product.Price);
                presentsDB.OrderDetails.Add(orderDetail);
            }
            order.Total = orderTotal;
            presentsDB.SaveChanges();
            EmptyCart();
            return order.OrderId;
        }

        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string userName)
        {
            var shoppingCart = presentsDB.Carts.Where(c => c.CartId == ShoppingCartId);
            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            presentsDB.SaveChanges();
        }

        public int ChangeCart(int id, int count)
        {
            var cartItem = presentsDB.Carts.Single(cart => cart.CartId == ShoppingCartId && cart.RecordId == id);
            if (cartItem != null)
            {
                cartItem.Count = count;
                presentsDB.SaveChanges();
            }
            return count;
        }
    }
}