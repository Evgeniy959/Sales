using System;
using CLI;
using SalesLib;
using MySql.Data.MySqlClient;

namespace SalesApp
{
    class Program
    {
        static void Main()
        {
            Show.Menu();
            var select = Console.ReadLine();
            switch (select)
            {
                case "1": // 1. Оформление заказа
                    InitOrder();
                    break;
                case "2": // 2. Добавление данных о покупке // Добавил для домашнего задания
                    InsertOrder();
                    break;
                case "3": // 3. Экспорт списка продуктов
                    ExportProducts();
                    break;
                case "4": // 4. Импорт списка продуктов
                    ImportProducts();
                    break;
                case "5": // 5. Экспорт списка закзов // Добавил для домашнего задания
                    ExportOrders();
                    break;
                case "6": // 6. Экспорт списка пользователей с указанием их дисконтной программы  // Добавил для домашнего задания
                    ExportBuyer();
                    break;
                case "7": // 7. Импорт списка пользователей с указанием их дисконтной программы  // Добавил для домашнего задания
                    ImportBuyer();
                    break;
                case "8": // 8. Импорт списка пользователей с указанием их дисконтной программы  // Добавил для домашнего задания (второй вариант правильный)
                    ImportBuyer1();
                    break;
            }
        }

        static void InitOrder()
        {
            var db = new DataBase();
            var products = db.GetProducts();
            var buyers = db.GetBuyers();

            Buyer buyer;

            foreach (var item in buyers)
            {
                Show.PrintLn($"{item.Id} - {item.Name}");
            }
            Show.PrintLn("Выберите номер покупателя");
            var buyer_id = uint.Parse(Console.ReadLine());
            if (buyer_id == 0)
            {
                buyer = new Buyer();
            }
            else
            {
                buyer = buyers[(int)(buyer_id - 1)];
            }

            Show.PrintLn($"{buyer.Name} - {buyer.Discount}");

            foreach (var product in products)
            {
                Show.PrintLn($"{product.Id}: {product.Name}, {product.Price}");
            }

            Show.Print("Введите номер продукта: ");
            var product_id = uint.Parse(Console.ReadLine());
            Show.Print("Введите количество: ");
            var count_user = uint.Parse(Console.ReadLine());

            var count_stock = db.GetProductCount(product_id);

            if (count_user > count_stock)
            {
                Show.Error("Столько нет товара на складе");
                return;
            }

            var price = products[(int)(product_id - 1)].Price;
            var total_price = count_user * (price - price * buyer.Discount / 100);
            Show.PrintLn($"Вам необходимо заплатить - {total_price}");
        }

/*Домашнее задание.
Добавить в программу (https://github.com/itstep-shambala/Sales.git) возможность добавления данных о покупке после ввода 
всей необходимой информации. Т.е.нужно от пользователя получить данные для всех полей таблицы tab_orders и написать 
запрос на добавление в неё строки.*/
                 
        static void InsertOrder()
        {
            var db = new DataBase();
            var ord = new Order();
            Show.PrintLn("Введите данные о заказе: ");
            Show.Print("Номер покупателя  ");
            ord.BuyerId = uint.Parse(Console.ReadLine());
            Show.Print("Номер продавца  ");
            ord.SellerId = uint.Parse(Console.ReadLine());
            Show.Print("Дата  ");
            ord.Date = Console.ReadLine();            
            ord.Date = DateTime.Now.ToString("yyyyMMddHHmmss");
            Show.Print("Номер продукта  ");
            ord.ProductId = uint.Parse(Console.ReadLine());
            Show.Print("Цена  ");
            ord.Amount = uint.Parse(Console.ReadLine());
            Show.Print("Итоговая стоимость  ");
            ord.TotalPrice = uint.Parse(Console.ReadLine());
            db.AddOrder(ord);
            var orders = db.GetOrders();
            foreach (var order in orders)
            {
                Show.PrintLn($"{order.Id}: {order.BuyerId}, {order.Date} {order.Amount}, {order.TotalPrice}");
            }
//--------------------------------------------------------------------------------------------
        }
        static void ExportProducts()
        {
            var db = new DataBase();
            db.ExportProductsToCSV("products.csv");
        }

        static void ImportProducts()
        {
            var db = new DataBase();
            db.ImportProductsFromCSV("products.csv");
        }
        //Домашнее задание.
        // Добавить возможность экспорта заказов, экспорта и импорта списка
        // пользователей с указанием их дисконтной программы.
        static void ExportOrders()
        {
            var db = new DataBase();
            db.ExportOrdersToCSV("opders.csv");
        }
        static void ExportBuyer()
        {
            var db = new DataBase();
            db.ExportBuyersTypeDiscountToCSV("buyers.csv");
        }
        static void ImportBuyer()
        {
            var db = new DataBase();
            db.ImportBuyersFromCSV("buyers.csv");
        }
        static void ImportBuyer1()
        {
            var db = new DataBase();
            db.ImportBuyers1FromCSV("buyers1.csv");
        }
    }
}