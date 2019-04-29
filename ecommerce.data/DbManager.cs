using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;


namespace ecommerce.data
{
    public class DbManager
    {
        private readonly string _connString;

        public DbManager(string connString)
        {
            _connString = connString;
        }

        public object SqlConnection { get; private set; }

        public IEnumerable<Category> GetCategories()
        {
            SqlConnection conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Categories ";
            conn.Open();
            List<Category> categories = new List<Category>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                categories.Add(new Category
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                });
            }
            conn.Close();
            conn.Dispose();
            return categories;
        }

        public void AddCategory(Category category)
        {
            SqlConnection conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Categories VALUES(@name) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@name", category.Name);
            conn.Open();
            category.Id = (int)(decimal)cmd.ExecuteScalar();
            conn.Close();
            conn.Dispose();
        }

        public void UpdateCategory(Category category)
        {
            SqlConnection conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Categories SET Name = @name WHERE id = @id";
            cmd.Parameters.AddWithValue("@name", category.Name);
            cmd.Parameters.AddWithValue("@id", category.Id);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }

        public void DeleteCategory(int id)
        {
            var conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE Categories WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }

        public IEnumerable<Product> GetProductsForCategory(int id)
        {
            var conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Products WHERE CategoryId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            var reader = cmd.ExecuteReader();
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    CategoryId = id,
                    Price = (decimal)reader["Price"],
                    Image = (string)reader["Image"]
                });
            }
            conn.Close();
            conn.Dispose();
            return products;
        }

        public void AddProduct(Product product)
        {
            var conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Products VALUES(@name, @desc, @price, @catId, @image)";
            cmd.Parameters.AddWithValue("@name", product.Name);
            cmd.Parameters.AddWithValue("@desc", product.Description);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@catId", product.CategoryId);
            cmd.Parameters.AddWithValue("@image", product.Image);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();

        }

        public void EditProduct(Product product)
        {
            var conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"Update Products SET name=@name, description=@desc, price=@price,
                                   Categoryid=@catId, image=@image where id=@prodId";
            cmd.Parameters.AddWithValue("@name", product.Name);
            cmd.Parameters.AddWithValue("@desc", product.Description);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@catId", product.CategoryId);
            cmd.Parameters.AddWithValue("@image", product.Image);
            cmd.Parameters.AddWithValue("@prodId", product.Id);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }

        public void DeleteProduct(int id)
        {
            var conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE Products WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }

        public Product GetProduct(int id)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 * FROM Products WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if(!reader.Read())
            {
                return null;
            }
            Product product = new Product
            {
                Name = (string)reader["Name"],
                Price = (decimal)reader["Price"],
                Id = (int)reader["Id"],
                CategoryId = (int)reader["CategoryId"],
                Description = (string)reader["Description"],
                Image = (string)reader["Image"]
            };
            conn.Close();
            conn.Dispose();
            return product;
        }

        public Category GetCategory(int id)
        {
            var conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 * FROM Categories WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            Category c = new Category
            {
                Name = (string)reader["Name"],
                Id = (int)reader["Id"],
                
            };
            conn.Close();
            conn.Dispose();
            return c;
        }

        public int AddShopper()
        {
            var conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO ShoppingCart VALUES (@datecreated) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@datecreated", DateTime.Now);
            conn.Open();
            int id = (int)(decimal)cmd.ExecuteScalar();
            conn.Close();
            conn.Dispose();
            return id;

        }

        public void AddItemToCart(CartItem cartItem)
        {
            var conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO ShoppingCartItems VALUES (@cartId, @productId, @quantity)";
            cmd.Parameters.AddWithValue("@cartId", cartItem.CartId);
            cmd.Parameters.AddWithValue("@productId", cartItem.ProductId);
            cmd.Parameters.AddWithValue("@quantity", cartItem.Quantity);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }

        public IEnumerable<Product> GetProductsInCart(int id)
        {
            var conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM ShoppingCartItems ci 
                                JOIN products p
                                ON p.Id = ci.ProductId
                                WHERE ci.ShoppingCartId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            var reader = cmd.ExecuteReader();
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    CategoryId = id,
                    Price = (decimal)reader["Price"],
                    Image = (string)reader["Image"],
                    Quantity = (int)reader["Quantity"]
                });
            }
            conn.Close();
            conn.Dispose();
            return products;
        }

        public void RemoveFromCart(CartItem cartItem)
        {
            var conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE ShoppingCartItems WHERE ProductId = @id AND  ShoppingCartId = @cartId";
            cmd.Parameters.AddWithValue("@id", cartItem.ProductId);
            cmd.Parameters.AddWithValue("@cartId", cartItem.CartId);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();

        }

        public void EditQuantity(CartItem cartItem)
        {
            var conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"Update ShoppingCartItems SET Quantity = @quantity 
                                WHERE ProductId = @id AND  ShoppingCartId = @cartId";
            cmd.Parameters.AddWithValue("@id", cartItem.ProductId);
            cmd.Parameters.AddWithValue("@cartId", cartItem.CartId);
            cmd.Parameters.AddWithValue("@quantity", cartItem.Quantity);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();

        }
    }
}
