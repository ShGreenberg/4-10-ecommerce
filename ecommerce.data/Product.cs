using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        public string DescriptionShort { get; set; }
        public int Quantity { get; set; }
    }
}
