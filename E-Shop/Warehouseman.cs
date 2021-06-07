using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace E_Shop
{
    //Кладовщик
    [Serializable]
    class Warehouseman : Account
    {
        public Warehouseman() : base()
        {
            Position = "Кладовщик";
            WorkPlace = "Не указано";
            Functions.AddRange(new (string, Method)[] {
                ("Создать склад", CreateStorage),
                ("Просмотреть товар", ShowProduct),
                ("Добавить новый товар на склад", AddProductToStorage),
                ("Переместить товар с одного склада на другой", MoveProduct),
                ("Изменить данные о товаре на складе", EditProduct),
                ("Забраковать товар (удалить товар со склада)", DeffectProduct)});
        }
        //public Warehouseman(string Login, string Password) : base(Login, Password)
        //{
        //    Position = "Кладовщик";
        //    WorkPlace = "Не указано";
        //    Functions.AddRange(new (string, Method)[] {
        //        ("Создать склад", CreateStorage),
        //        ("Просмотреть товар", ShowProduct),
        //        ("Добавить новый товар на склад", AddProductToStorage),
        //        ("Переместить товар с одного склада на другой", MoveProduct),
        //        ("Изменить данные о товаре на складе", EditProduct),
        //        ("Забраковать товар (удалить товар со склада)", DeffectProduct)});
        //}
        private void CreateStorage()
        {
            Console.WriteLine("Введите название для нового склада");
            string storageName = Console.ReadLine().Trim();
            List<Storage> storages = Helper.DeserializeStorage();
            if (storages.Count == 0 || storages.Find(s => s.Name == storageName) == null)
            {
                Helper.AddStorageToBD(new Storage(storageName));
                Console.WriteLine($"Склад \"{storageName}\" создан");
            }
            else Console.WriteLine("Склад с таким именем уже существует!");
            Console.WriteLine("Нажмите любую кнопку...");
            Console.ReadKey();
        }
        private void ShowProduct()
        {
            List<Storage> storages = Helper.DeserializeStorage();
            while (true)
            {
                if (storages.Count == 0)
                {
                    Console.WriteLine("Не найдено cкладов.");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    return;
                }
                List<string> storagesName = new List<string>();
                for (int i = 0; i < storages.Count; i++)
                    storagesName.Add($"Склад №{i + 1} - {storages[i].Name}");
                storagesName.Add("Назад");

                ConsoleMenu storageMenu = new ConsoleMenu(storagesName.ToArray());
                int chooseStorage = storageMenu.PrintMenu();

                if (chooseStorage == storagesName.Count - 1) break;


                List<string> products = new List<string>();
                foreach (Product product in storages[chooseStorage].Products)
                    products.Add($"Товар \"{product.Name}\" | Категория {product.Category}");
                products.Add("Назад");
                ConsoleMenu productMenu = new ConsoleMenu(products.ToArray());
                while (true)
                {
                    if (storages[chooseStorage].Products.Count == 0)
                    {
                        Console.WriteLine("На этом складе нет товаров");
                        Console.WriteLine("Нажмите любую кнопку...");
                        Console.ReadKey();
                        break;
                    }

                    int chooseProduct = productMenu.PrintMenu();
                    if (chooseProduct == products.Count - 1) break;

                    Console.WriteLine($"Данные о товаре №{chooseProduct + 1}");
                    Console.WriteLine($"Название: {storages[chooseStorage].Products[chooseProduct].Name}");
                    Console.WriteLine($"Категория: {storages[chooseStorage].Products[chooseProduct].Category}");
                    Console.WriteLine($"Цена за единицу товара: {storages[chooseStorage].Products[chooseProduct].Price}");
                    Console.WriteLine($"Срок годности до: {storages[chooseStorage].Products[chooseProduct].ShelfLife}");
                    Console.WriteLine($"Количество товара на складе: {storages[chooseStorage].Products[chooseProduct].Count}");
                    Console.WriteLine("Нажмите любую кнопку, чтобы продолжить...");
                    Console.ReadKey();
                }
            }
        }
        private void AddProductToStorage()
        {
            List<Storage> storages = Helper.DeserializeStorage();
            while (true)
            {
                if (storages.Count == 0)
                {
                    Console.WriteLine("Не найдено cкладов.");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    return;
                }
                List<string> storagesName = new List<string>();
                for (int i = 0; i < storages.Count; i++)
                    storagesName.Add($"Склад №{i + 1} - {storages[i].Name}");
                storagesName.Add("Назад");

                ConsoleMenu storageMenu = new ConsoleMenu(storagesName.ToArray());
                int chooseStorage = storageMenu.PrintMenu();

                if (chooseStorage == storagesName.Count - 1) break;

                ConsoleMenu AddOrBackMenu = new ConsoleMenu(new string[] { "Добавить товар", "Назад" });
                while (true)
                {
                    if (AddOrBackMenu.PrintMenu() == 1) break;
                    storages[chooseStorage].AddOrIncrementProduct(new Product());
                    Helper.SerializeStorage(storages);
                }
            }
        }
        private void MoveProduct()
        {
            List<Storage> storages = Helper.DeserializeStorage();
            if (storages.Count < 2)
            {
                Console.WriteLine("Недостаточно или нет складов.");
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
                return;
            }

            List<string> storageNames = new List<string>();
            foreach (Storage st in storages)
                storageNames.Add(st.Name);
            storageNames.Add("Выйти в главное меню");

            //выбираем склад-отправитель
            Console.Clear();
            Console.WriteLine("Выберите склад, с которого будете перемещать товар");
            Thread.Sleep(500);
            ConsoleMenu storageMenu = new ConsoleMenu(storageNames.ToArray());
            int index1 = storageMenu.PrintMenu();
            if (index1 == storageNames.Count - 1) return;
            if (storages[index1].Products.Count == 0)
            {
                Console.WriteLine("На этом складе нет товаров");
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
                return;
            }

            //выбираем товар
            Console.Clear();
            Console.WriteLine("Выберите товар, который хотите переместить");
            Thread.Sleep(500);
            List<string> products = new List<string>();
            foreach (Product product in storages[index1].Products)
                products.Add($"Товар \"{product.Name}\" | Категория {product.Category} | {product.Price} рублей за один товар | {product.Count} шт.");
            products.Add("Выйти в главное меню");
            ConsoleMenu productMenu = new ConsoleMenu(products.ToArray());
            int chooseProduct = productMenu.PrintMenu();
            if (chooseProduct == products.Count - 1) return;

            //выбираем склад-получатель
            Console.Clear();
            Console.WriteLine("Выберите склад, на который будете перемещать товар");
            Thread.Sleep(500);
            storageNames.Remove(storages[index1].Name);
            storageNames.TrimExcess();
            storageMenu = new ConsoleMenu(storageNames.ToArray());
            int index2 = storageMenu.PrintMenu();
            if (index2 == storageNames.Count - 1) return;
            index2 = storages.FindIndex(s => s.Name == storageNames[index2]);

            //перемещаем товар
            Product movableProduct = storages[index1].Products[chooseProduct];
            storages[index1].Products.Remove(movableProduct);
            storages[index2].AddOrIncrementProduct(movableProduct);
            Helper.SerializeStorage(storages);
        }
        private void EditProduct()
        {
            List<Storage> storages = Helper.DeserializeStorage();
            while (true)
            {
                if (storages.Count == 0)
                {
                    Console.WriteLine("Не найдено cкладов.");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    return;
                }
                List<string> storagesName = new List<string>();
                for (int i = 0; i < storages.Count; i++)
                    storagesName.Add($"Склад №{i + 1} - {storages[i].Name}");
                storagesName.Add("Назад");

                ConsoleMenu storageMenu = new ConsoleMenu(storagesName.ToArray());
                int chooseStorage = storageMenu.PrintMenu();

                if (chooseStorage == storagesName.Count - 1) break;

                while (true)
                {
                    if (storages[chooseStorage].Products.Count == 0)
                    {
                        Console.WriteLine("На этом складе нет товаров");
                        Console.WriteLine("Нажмите любую кнопку...");
                        Console.ReadKey();
                        break;
                    }

                    List<string> productNames = new List<string>();
                    foreach (Product product in storages[chooseStorage].Products)
                        productNames.Add($"Товар {product.Name} | Категория {product.Category}");
                    productNames.Add("Назад");
                    ConsoleMenu productMenu = new ConsoleMenu(productNames.ToArray());
                    int chooseProduct = productMenu.PrintMenu();

                    if (chooseProduct == productNames.Count - 1)
                        break;

                    string[] productData = {
                    "Название товара - " + storages[chooseStorage].Products[chooseProduct].Name,
                    "Категория - " + storages[chooseStorage].Products[chooseProduct].Category,
                    "Цена за единицу товара - " + storages[chooseStorage].Products[chooseProduct].Price.ToString(),
                    "Количество товара - " + storages[chooseStorage].Products[chooseProduct].Count.ToString(),
                    "Срок годности (до) - " + storages[chooseStorage].Products[chooseProduct].ShelfLife,
                    "Назад"};
                    ConsoleMenu dataMenu = new ConsoleMenu(productData);
                    int chooseData = dataMenu.PrintMenu();
                    if (chooseData == productData.Length - 1)
                        break;

                    if (chooseData == 1)
                        storages[chooseStorage].Products[chooseProduct].ChangeCategory();
                    else
                    {
                        Console.WriteLine("Введите новые данные:");
                        string changedData = Console.ReadLine().Trim();
                        switch (chooseData)
                        {
                            case 0:
                                storages[chooseStorage].Products[chooseProduct].Name = changedData;
                                break;
                            case 2:
                                storages[chooseStorage].Products[chooseProduct].Price = int.Parse(changedData);
                                break;
                            case 3:
                                storages[chooseStorage].Products[chooseProduct].Count = int.Parse(changedData);
                                break;
                            case 4:
                                storages[chooseStorage].Products[chooseProduct].ShelfLife = changedData;
                                break;
                        }
                    }
                    Helper.SerializeStorage(storages);
                }
            }
        }
        private void DeffectProduct()
        {
            List<Storage> storages = Helper.DeserializeStorage();
            while (true)
            {
                if (storages.Count == 0)
                {
                    Console.WriteLine("Не найдено cкладов.");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    return;
                }
                List<string> storagesName = new List<string>();
                for (int i = 0; i < storages.Count; i++)
                    storagesName.Add($"Склад №{i + 1} - {storages[i].Name} ");
                storagesName.Add("Назад");

                ConsoleMenu storageMenu = new ConsoleMenu(storagesName.ToArray());
                int chooseStorage = storageMenu.PrintMenu();
                if (chooseStorage == storagesName.Count - 1) break;

                while (true)
                {
                    if (storages[chooseStorage].Products.Count == 0)
                    {
                        Console.WriteLine("На этом складе нет товаров");
                        Console.WriteLine("Нажмите любую кнопку...");
                        Console.ReadKey();
                        break;
                    }

                    List<string> productNames = new List<string>();
                    foreach (Product product in storages[chooseStorage].Products)
                        productNames.Add($"Товар {product.Name} | Категория {product.Category} | Срок годности {product.ShelfLife}");
                    productNames.Add("Назад");

                    ConsoleMenu productMenu = new ConsoleMenu(productNames.ToArray());
                    int chooseProduct = productMenu.PrintMenu();
                    if (chooseProduct == productNames.Count - 1)
                        break;

                    storages[chooseStorage].Products.RemoveAt(chooseProduct);
                    Helper.SerializeStorage(storages);
                    if (storages[chooseStorage].Products.Count == 0) break;
                }
            }

        }
        public override void OnDeserializing()
        {
            base.OnDeserializing();
            Functions.AddRange(new (string, Method)[] {
                ("Создать склад", CreateStorage),
                ("Просмотреть товар", ShowProduct),
                ("Добавить новый товар на склад", AddProductToStorage),
                ("Переместить товар с одного склада на другой", MoveProduct),
                ("Изменить данные о товаре на складе", EditProduct),
                ("Забраковать товар (удалить товар со склада)", DeffectProduct)});
        }
    }
}
