using ecommerce.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _4_10_ecommerce__core_.Models
{
    public class ProductsViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public Category Category { get; set; }
    }
}
