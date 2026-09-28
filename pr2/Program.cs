using System;
using System.Numerics;
using System.Xml.Linq;

namespace pr2
{

    public enum Category
    {
        Техника,
        Мебель,
        Еда
    }

    class Product
    {
        private static int NextId = 1;
        public int Id { get; }
        public string Name { get; set; }
        public decimal Price { get; set;  }
        public int Amount { get; set; }
        public Category Category {  get; set; }   
        
        public Product(string name, decimal price, int amount, Category category) 
        {
            Id = NextId;
            NextId++;
            Name = name;
            Price = price;
            Amount = amount;
            Category = category;
        }

        public void Print() 
        {
            Console.WriteLine("Код: " + Id);
            Console.WriteLine("Название: " + Name);
            Console.WriteLine("Цена: " + Price);
            Console.WriteLine("Остаток: " + Amount);
            Console.WriteLine("Категория: " + Category);
            Console.WriteLine("-----------------------");
        }

        
    }

    class Program
    {
        static List<Product> products = new List<Product>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Start_Products();
            RunMenu();
        }

        static void Start_Products()
        {
            products.Add(new Product("Греча", 49.9m, 100, Category.Еда));
            products.Add(new Product("Шкаф", 15999.99m, 12, Category.Мебель));
            products.Add(new Product("Стиральная машина", 24999.99m, 7, Category.Техника));
            products.Add(new Product("Iphone 18 Pro Max Swag", 150000.00m, 5, Category.Техника));
            products.Add(new Product("Мармеладки", 72.34m, 1000, Category.Еда));
        }

        static void RunMenu()
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("Учёт товаров в магазине");
                Console.WriteLine("1 - Показать все товары");
                Console.WriteLine("2 - Добавить товар");
                Console.WriteLine("3 - Удалить товар");
                Console.WriteLine("4 - Заказать поставку товара");
                Console.WriteLine("5 - Продать товар");
                Console.WriteLine("6 - Поиск товара");
                Console.WriteLine("0 - Выход");
                Console.Write("Выберите команду: ");

                string user_input = Console.ReadLine();

                switch (user_input)
                {
                    case "1":
                        PrintAllProduct();
                        break;
                    case "2":
                        AddProduct();
                        break;
                    case "3":
                        DelProduct();
                        break;
                    case "4":
                        Postavka();
                        break;
                    case "5":
                        SellProduct();
                        break;
                    case "6":
                        SearchProduct();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("😘");
                        break;
                    default:
                        Console.WriteLine("Выберите команду ТОЛЬКО из списка!");
                        break;
                }
            }
        }

        static void PrintAllProduct() 
        {
            Console.WriteLine();
            foreach (Product p in products) { p.Print(); }
        }

        static string CheckString()
        {
            string input;

            while (true)
            {
                Console.Write("Введите название: ");
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) { Console.WriteLine("Ввод некорректен"); }
                else { return input; }
            }
        }

        static decimal CheckDecimal()
        {
            decimal value;

            while (true)
            {
                Console.Write("Введите цену: ");
                string input = Console.ReadLine();
                if (decimal.TryParse(input, out value) && value > 0) { return value; }
                else { Console.WriteLine("Ввод некорректен!"); }
            }
        }

        static int CheckInt(string text)
        {
            int value;

            while (true)
            {
                Console.Write(text);
                string input = Console.ReadLine();

                if (int.TryParse(input, out value) && value > 0) { return value; }
                else { Console.WriteLine("Ввод некорректен!"); }
            }
        }

        static Category CheckEnum()
        {
            while (true)
            {
                Console.WriteLine("Выберите категорию:");
                Console.WriteLine("1 - Техника");
                Console.WriteLine("2 - Мебель");
                Console.WriteLine("3 - Еда");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        return Category.Техника;
                    case "2":
                        return Category.Мебель;
                    case "3":
                        return Category.Еда;
                    default:
                        Console.WriteLine("Некорректный ввод!");
                        break;
                }
            }
        }

        static void AddProduct() 
        {            
            string name = CheckString();
            decimal price = CheckDecimal();
            int amount = CheckInt("Введите количество: ");
            Category category = CheckEnum();

            products.Add(new Product(name, price, amount, category));
            Console.WriteLine($"Товар успешно добавлен!");
        }

        static void DelProduct() 
        {
            while (true)
            {
                int index = CheckInt("Введите код: ");

                Product del = null;

                foreach (Product p in products)
                {
                    if (p.Id == index)
                    {
                        del = p;
                    }
                }

                if (del != null)
                {
                    Console.WriteLine($"Товар {del.Name} успешно удален!");
                    products.Remove(del);
                    break;
                }
                else
                {
                    Console.WriteLine("Код товара не найден!");
                }                
            }            
        }

        static void Postavka() 
        {
            while (true)
            { 
                int code = CheckInt("Выберите код для поставки: ");                

                Product findproduct = null;

                foreach (Product product in products)
                {
                    if (product.Id == code)
                    {
                        findproduct = product;
                    }
                }
                if (findproduct != null)
                {
                    int kolvo = CheckInt("Выберите количество для поставки: ");

                    findproduct.Amount += kolvo;
                    Console.WriteLine("Товар поставлен!");
                    break;
                }
                else 
                { 
                    Console.WriteLine("Товар с таким кодом не найден"); 
                }
            }            
        }

        static void SellProduct() 
        {
            while (true)
            {
                int code = CheckInt("Введите код товара для продажи: ");            

                Product product = null;

                foreach (Product p in products)
                {
                    if (p.Id == code)
                    {
                        product = p;
                    }
                }
            
                if (product != null)
                {
                    int kolvo = CheckInt("Введите количество товара для продажи: ");

                    if (product.Amount >= kolvo)
                    {
                        product.Amount -= kolvo;
                        Console.WriteLine($"Товар успешно продан за {product.Price * kolvo}!" );
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Товара недостаточно!");                        
                    }
                }
                else
                {
                    Console.WriteLine("Товара с таким кодом не найдено!");                    
                }
            }
            
        }

        static void SearchProduct() 
        {                
        }      
    }
}
