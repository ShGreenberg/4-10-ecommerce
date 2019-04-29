using ecommerce.data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _4_10_ecommerce__core_.Models
{
    public class AddNewProduct
    {
        public Product Product { get; set; }
        public IFormFile File { get; set; }
    }
}
