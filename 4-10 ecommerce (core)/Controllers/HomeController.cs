using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _4_10_ecommerce__core_.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ecommerce.data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace _4_10_ecommerce__core_.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connString;
        public HomeController(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            DbManager mgr = new DbManager(_connString);
            return View(mgr.GetCategories());

        }

        public IActionResult ProductPage(int id)
        {
            DbManager mgr = new DbManager(_connString);
            ProductPageViewModel vm = new ProductPageViewModel
            {
                Product = mgr.GetProduct(id),
                Categories = mgr.GetCategories()
            };
            return View(vm);
        }

        public IActionResult GetProducts(int id)
        {
            DbManager mgr = new DbManager(_connString);
            return Json(mgr.GetProductsForCategory(id));
        }

        public IActionResult GetCartItems()
        {
            int id = HttpContext.Session.Get<int>("id");
            DbManager mgr = new DbManager(_connString);
            IEnumerable<Product> products = mgr.GetProductsInCart(id);
            return Json(products);
        }

        [HttpPost]
        public IActionResult AddToCart(CartItem cartItem)
        {
            DbManager mgr = new DbManager(_connString);
            int id = HttpContext.Session.Get<int>("id");
            if (id == 0)
            {
                id = mgr.AddShopper();
                HttpContext.Session.Set("id", id);
            }
            cartItem.CartId = id;
            List<Product> items = mgr.GetProductsInCart(id).ToList();
            bool added = true;
            if(items.FirstOrDefault(i => i.Id == cartItem.ProductId) == null)
            {
                mgr.AddItemToCart(cartItem);
            }
            else
            {
                added = false;
            }

            return Json(added);
        }

        public IActionResult ViewCart()
        {
            int id = HttpContext.Session.Get<int>("id");
            DbManager mgr = new DbManager(_connString);

            return View(mgr.GetProductsInCart(id));
        }

        //next to json not sure what to return
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            CartItem cartItem = new CartItem
            {
                ProductId = id,
                CartId = HttpContext.Session.Get<int>("id"),
            };
            DbManager mgr = new DbManager(_connString);
            mgr.RemoveFromCart(cartItem);
            return Json("removed");
        }

        [HttpPost]
        public IActionResult EditQuantity(CartItem cartItem)
        {
            cartItem.CartId = HttpContext.Session.Get<int>("id");
            DbManager mgr = new DbManager(_connString);
            mgr.EditQuantity(cartItem);
            return Json("edited");
        }



    }


    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
