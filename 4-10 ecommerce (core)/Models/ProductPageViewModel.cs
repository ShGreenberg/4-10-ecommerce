using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data;

namespace _4_10_ecommerce__core_.Models
{
    public class ProductPageViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
