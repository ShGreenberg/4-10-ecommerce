using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.data
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
    }
}
