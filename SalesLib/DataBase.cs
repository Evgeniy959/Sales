using System;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;

namespace SalesLib
{
    public class DataBase
    {
        private MySqlConnection db;
        private MySqlCommand command;

        public DataBase()
        {
            var connectionString = ConnectionString.Init(@"db_connect.ini");
            db = new MySqlConnection(connectionString);
            command = new MySqlCommand { Connection = db };
        }

        public void Open() => db.Open();

        public void Close() => db.Close();
        public List<Product> GetProducts()
        {
            Open();
            var list = new List<Product>();

            var sql = "SELECT id, name, price FROM tab_products;";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return null;

            while (res.Read())
            {
                var id = res.GetUInt32("id");
                var name = res.GetString("name");
                var price = res.GetUInt32("price");
                list.Add(new Product { Id = id, Name = name, Price = price });
            }

            Close();
            return list;
        }

        public uint GetProductCount(uint id)
        {
            Open();

            var sql = @$"SELECT count
                        FROM tab_products_stock
                        JOIN tab_products 
                            ON tab_products_stock.product_id = tab_products.id
                        WHERE product_id = {id};";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return 0;

            res.Read();
            var count = res.GetUInt32("count");

            Close();

            return count;
        }

        public List<Buyer> GetBuyers()
        {
            var list = new List<Buyer>();

            Open();

            var sql = @"SELECT tab_buyers.id, first_name, last_name, discount
                        FROM tab_buyers
                        JOIN tab_people 
                            ON tab_buyers.people_id = tab_people.id
                        JOIN tab_discounts 
                            ON tab_buyers.discount_id = tab_discounts.id;";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return null;

            while (res.Read())
            {
                var id = res.GetUInt32("id");
                var name = $"{res.GetString("first_name")} {res.GetString("last_name")}";
                var discount = res.GetUInt32("discount");

                list.Add(new Buyer { Id = id, Name = name, Discount = discount });
            }

            Close();

            return list;
        }
        public List<Buyer> GetBuyersTypeDiscount() // Добавил для домашнего задания
        {
            var list = new List<Buyer>();

            Open();

            var sql = @"SELECT tab_buyers.id, first_name, last_name, type, discount
                        FROM tab_buyers
                        JOIN tab_people 
                            ON tab_buyers.people_id = tab_people.id
                        JOIN tab_discounts 
                            ON tab_buyers.discount_id = tab_discounts.id;";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return null;

            while (res.Read())
            {
                var id = res.GetUInt32("id");
                var name = $"{res.GetString("first_name")} {res.GetString("last_name")}";
                var type = res.GetString("type");
                var discount = res.GetUInt32("discount");

                list.Add(new Buyer { Id = id, Name = name, Type = type, Discount = discount });
            }

            Close();

            return list;
        }
        public List<Order> GetOrders() // Добавил для домашнего задания
        {
            Open();
            var list = new List<Order>();

            var sql = "SELECT id, buyer_id, seller_id, date, product_id, amount, total_price FROM tab_orders;";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return null;

            while (res.Read())
            {
                var id = res.GetUInt32("id");
                var buyer_id = res.GetUInt32("buyer_id");
                var seller_id = res.GetUInt32("seller_id");
                var date = res.GetString("date");
                var product_id = res.GetUInt32("product_id");
                var amount = res.GetUInt32("amount");
                var total_price = res.GetUInt32("total_price");
                list.Add(new Order { Id = id, BuyerId = buyer_id, SellerId = seller_id, Date = date, ProductId = product_id, Amount = amount, TotalPrice = total_price });
            }
            Close();
            return list;
        }

        public void AddOrder(Order ord) // Добавил для домашнего задания
        {
            Open();
            var sql = $@"INSERT INTO tab_orders (buyer_id, seller_id, date, product_id, amount, total_price)
                      VALUE ({ord.BuyerId}, {ord.SellerId}, {ord.Date}, {ord.ProductId}, {ord.Amount}, {ord.TotalPrice});";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Close();
        }

        public void ExportProductsToCSV(string path)
        {
            var products = GetProducts();

            using var file = new StreamWriter(path, append: false);
            foreach (var product in products)
            {
                file.WriteLine($"{product.Id}|{product.Name}|{product.Price}");
            }
        }
        public void ExportOrdersToCSV(string path) // Добавил для домашнего задания
        {
            var orders = GetOrders();

            using var file = new StreamWriter(path, append: false);
            foreach (var order in orders)
            {
                file.WriteLine($"{order.Id}|{order.BuyerId}|{order.SellerId}|{order.Date}|{order.ProductId}|{order.Amount}|{order.TotalPrice}");
            }
        }
        public void ExportBuyersTypeDiscountToCSV(string path) // Добавил для домашнего задания
        {
            var buyers = GetBuyersTypeDiscount();

            using var file = new StreamWriter(path, append: false);
            foreach (var buyer in buyers)
            {
                file.WriteLine($"{buyer.Id}|{buyer.Name}|{buyer.Type}|{buyer.Discount}");
            }
        }
        public void ImportProductsFromCSV(string path)
        {
            var products_csv = new List<Product>();

            using var file = new StreamReader(path);

            var line = string.Empty;
            while ((line = file.ReadLine()) != null)
            {
                var temp = line.Split('|');
                var product = new Product
                {
                    Id = uint.Parse(temp[0]),
                    Name = temp[1],
                    Price = uint.Parse(temp[2])
                };
                products_csv.Add(product);
            }

            //TODO Переписать проверку на уникальность
            /*var products_db = GetProducts();
            var products = new List<Product>();
            uint i = 1; 
            foreach (var product_db in products_db)
            {
                foreach (var product_csv in products_csv)  
                {
                    if (product_db.Name == product_csv.Name) continue; 

                    products.Add(new Product {Id = i, Name = product_csv.Name, Price = product_csv.Price});
                    i++;
                }
            }*/

            Open();
            foreach (var product in products_csv)
            {
                var sql = $"INSERT INTO tab_products (name, price) VALUES ('{product.Name}', {product.Price});";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            Close();
        }

        // Первый вариант импорта (неправильный) в специально созданную новую таблицу
        public void ImportBuyersFromCSV(string path) // Добавил для домашнего задания
        {
            var buyers_csv = new List<Buyer>();

            using var file = new StreamReader(path);

            var line = string.Empty;
            while ((line = file.ReadLine()) != null)
            {
                var temp = line.Split('|');
                var buyer = new Buyer
                {
                    Id = uint.Parse(temp[0]),
                    Name = temp[1],
                    Type = temp[2],
                    Discount = uint.Parse(temp[3])
                };
                buyers_csv.Add(buyer);
            }
            Open();
            foreach (var buyer in buyers_csv)
            {
                var sql = $"INSERT INTO tab_users (name, type, discount) VALUES ('{buyer.Name}', '{buyer.Type}', {buyer.Discount});";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            Close();
        }

        // Второй вариант импорта (правильный) в уже существующие таблицы
        public void ImportBuyers1FromCSV(string path) // Добавил для домашнего задания
        {
            var buyers_csv = new List<Buyer>();

            using var file = new StreamReader(path);

            var line = string.Empty;
            while ((line = file.ReadLine()) != null)
            {
                var temp = line.Split('|');
                var buyer = new Buyer
                {
                    Id = uint.Parse(temp[0]),
                    First_Name = temp[1],
                    Last_Name = temp[2],
                    Phone = temp[3],
                    Type = temp[4],
                    Discount = uint.Parse(temp[5])
                };
                buyers_csv.Add(buyer);
            }
            Open();
            foreach (var buyer in buyers_csv)
            {
                var sql = $"INSERT INTO tab_people (first_name, last_name, phone) VALUES ('{buyer.First_Name}','{buyer.Last_Name}', '{buyer.Phone}');";
                var sql1 = $"INSERT INTO tab_discounts (type, discount) VALUES ('{buyer.Type}', {buyer.Discount});";
                var sql2 = $"INSERT INTO tab_buyers (people_id, discount_id) VALUES ((SELECT id FROM tab_people WHERE id = (SELECT MAX(id) FROM tab_people)), " +
                           $"(SELECT id FROM tab_discounts WHERE id = (SELECT MAX(id) FROM tab_discounts)));";
                command.CommandText = sql;
                command.ExecuteNonQuery();
                command.CommandText = sql1;
                command.ExecuteNonQuery();
                command.CommandText = sql2;
                command.ExecuteNonQuery();
            }
            Close();
        }
    }
}