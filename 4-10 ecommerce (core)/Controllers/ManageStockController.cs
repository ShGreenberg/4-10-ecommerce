using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _4_10_ecommerce__core_.Models;
using ecommerce.data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace _4_10_ecommerce__core_.Controllers
{
    public class ManageStockController : Controller
    {
        private readonly string _connString;
        private readonly IHostingEnvironment _enviroment;
        public ManageStockController(IConfiguration configuration, IHostingEnvironment environment)
        {
            _connString = configuration.GetConnectionString("ConStr");
            _enviroment = environment;
        }

        public IActionResult Categories()
        {
            DbManager mgr = new DbManager(_connString);
            return View(mgr.GetCategories());
        }

        public IActionResult GetCategories()
        {
            DbManager mgr = new DbManager(_connString);
            return Json(mgr.GetCategories().ToList());
        }

        [HttpPost]
        public IActionResult AddCategory(string name)
        {
            DbManager mgr = new DbManager(_connString);
            mgr.AddCategory(new Category { Name = name });
            return Json(name);
        }
        //json not sure what to return
        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            DbManager mgr = new DbManager(_connString);
            mgr.UpdateCategory(category);
            return Json("updated");
        }
        //json not sure what to return
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            DbManager mgr = new DbManager(_connString);
            mgr.DeleteCategory(id);
            return Json("deleted");
        }

        public IActionResult Products(int categoryId)
        {
            DbManager mgr = new DbManager(_connString);
            List<Product> products = mgr.GetProductsForCategory(categoryId).ToList();
            foreach(Product p in products)
            {
                if(p.Description.Length > 100)
                {
                    string hold = "";
                    for(int i = 0; i < 100; i++)
                    {
                        hold += p.Description[i];
                    }
                    p.DescriptionShort =  hold + "...";
                }
                else
                {
                    p.DescriptionShort = p.Description;
                }
            }
            ProductsViewModel vm = new ProductsViewModel
            {
                Products = products,
                Category = mgr.GetCategory(categoryId)
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult AddProduct(Product product, IFormFile picFile)
        {

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(picFile.FileName)}";
            string fullPath = Path.Combine(_enviroment.WebRootPath, "productImages", fileName);
            using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                picFile.CopyTo(stream);
            }
            product.Image = fileName;
            DbManager mgr = new DbManager(_connString);
            mgr.AddProduct(product);
            return Redirect($"/managestock/products?categoryid={product.CategoryId}");
        }

        [HttpPost]
        public IActionResult EditProduct(Product product, IFormFile picFile)
        {
            if(picFile != null)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(picFile.FileName)}";
                string fullPath = Path.Combine(_enviroment.WebRootPath, "productImages", fileName);
                using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
                {
                    picFile.CopyTo(stream);
                }
                product.Image = fileName;
            }
           
  
            DbManager mgr = new DbManager(_connString);
            mgr.EditProduct(product);
            return Redirect($"/managestock/products?categoryid={product.CategoryId}");
        }

        public IActionResult DeleteProduct(int id, int catId)
        {
            DbManager mgr = new DbManager(_connString);
            mgr.DeleteProduct(id);
            return Redirect($"/managestock/products?categoryid={catId}");
        }
    }
}