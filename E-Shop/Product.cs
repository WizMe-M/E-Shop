using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace E_Shop
{
    [Serializable]
    class Product
    {
        [NonSerialized]
        public static string[] categories = { "Продукты", "Алкоголь", "Бытовая химия", "Техника", "Одежда", "Канцелярские товары" };

        string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                string pattern = "^(([A-Z](?:[a-zA-Z0-9 +\"\'.-])*)|(([А-Я](?:[а-яА-Я0-9 +\"\'.-])*)))$";
                while (!Regex.IsMatch(value, pattern))
                {
                    Console.Clear();
                    Console.WriteLine("Название продукта должно начинаться с заглавной буквы и " +
                        "состоять целиком либо из латиницы, либо из кириллицы." +
                        "\nТакже допустимы числа, некоторые спец. символы, точка, кавычки и пробелы");
                    Console.Write("Введите фамилию ещё раз: ");
                    value = Console.ReadLine().Trim();
                }
                name = value;
            }
        }
        public string Category { get; set; }
        double price;
        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                while(value < 0.0 || value >= 10.0e+5)
                {
                    Console.Clear();
                    Console.WriteLine("Цена продукта не может быть отрицательной или превышать миллион рублей!");
                    Console.Write("Введите цену продукта: ");
                    value = Console.ReadLine().SafeParseDouble();
                }
                price = value;
            }
        }
        int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                while (value < 0 || value >= 10e+5)
                {
                    Console.Clear();
                    Console.WriteLine("Количество продукта не может быть дефицитным или превышать миллион!");
                    Console.Write("Введите цену продукта: ");
                    value = Console.ReadLine().SafeParseInt();
                }
                count = value;
            }
        }
        
        DateTime shelfLife;
        public string ShelfLife
        {
            get
            {
                return shelfLife.ToShortDateString();
            }
            set
            {
                string pattern = @"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)
                (?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?
                (?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))
                $|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$";
                while (!Regex.IsMatch(value, pattern))
                    {
                        Console.Clear();
                        Console.WriteLine("Принимаются даты формата дд.мм.гггг или дд/мм/гггг или дд-мм-гггг, в том числе високосные года.");
                        Console.Write("Введите срок годности: ");
                        value = Console.ReadLine().Trim();
                    }
                shelfLife = DateTime.Parse(value);
            }
        }
        public Product()
        {
            Console.Clear();
            Console.WriteLine("Введите название товара:");
            Name = Console.ReadLine().Trim();

            ChangeCategory();
            
            Console.Clear();
            Console.WriteLine("Введите цену товара:");
            Price = int.Parse(Console.ReadLine().Trim());
            
            Console.Clear();
            Console.WriteLine("Введите количество товара:");
            Count = int.Parse(Console.ReadLine().Trim());
            
            Console.Clear();
            Console.WriteLine("Введите срок годности товара (до какого числа):");
            ShelfLife = Console.ReadLine().Trim();
        }
        //public Product(string Name, string Category, int Price, int Count, string ShelfLife) : this()
        //{
        //    this.Name = Name;
        //    this.Category = Category;
        //    this.Price = Price;
        //    this.Count = Count;
        //    this.ShelfLife = ShelfLife;
        //}
        public void ChangeCategory()
        {
            Console.Clear();
            Console.WriteLine("Выберите категорию товара:");
            Console.WriteLine("Нажмите любую кнопку, чтобы перейти к выбору...");
            Console.ReadKey();
            ConsoleMenu categoryMenu = new ConsoleMenu(categories);
            int chooseCategory = categoryMenu.PrintMenu();
            Category = categories[chooseCategory];
        }
        public int ChooseCountOfProducts()
        {
            int count = 1;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Товар {Name} | Цена: {Price} | Срок годности до: {ShelfLife}");
                Console.Write("Нажимайте на стрелки, чтобы изменить количество товара:\t"+count);
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        count++;
                        if (count > Count)
                        {
                            count = Count;
                            Console.WriteLine("Максимум товаров!");
                            Thread.Sleep(1000);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        count--;
                        if (count < 1)
                        {
                            count = 1;
                            Console.WriteLine("Минимум товаров!");
                            Thread.Sleep(1000);
                        }
                        break;
                    case ConsoleKey.Enter:
                        return count;
                }
            }
        }
    }
}
