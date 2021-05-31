using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace E_Shop
{
    [Serializable]
    class Product
    {
        [NonSerialized]
        public static string[] categories = { "Продукты", "Алкоголь", "Бытовая химия", "Техника", "Одежда", "Канцелярские товары" };

        public string Name { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        //DateTime:
        DateTime shelfLife;
        public string ShelfLife { get; set; }
        public Product()
        {
            Console.Clear();
            Console.WriteLine("Введите название товара:");
            Name = Console.ReadLine().Trim();
            //выбираем категорию товара
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
        public Product(string Name, string Category, int Price, int Count, string ShelfLife)
        {
            this.Name = Name;
            this.Category = Category;
            this.Price = Price;
            this.Count = Count;
            this.ShelfLife = ShelfLife;
        }
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
