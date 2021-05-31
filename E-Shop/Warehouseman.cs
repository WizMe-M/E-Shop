using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace E_Shop
{
    [Serializable]
    class Warehouseman : Account
    {
        public Warehouseman() : base()
        {
            Position = "Кладовщик";
            WorkPlace = "Не указано";
        }
        public Warehouseman(string Login, string Password) : base(Login, Password)
        {
            Position = "Кладовщик";
            WorkPlace = "Не указано";
        }
        void CreateStorage()
        {
            Console.WriteLine("Введите название для нового склада");
            string storageName = Console.ReadLine().Trim();
            List<Storage> storages = Helper.DeserializeStorage();
            if (storages.Count == 0 || storages.Find(s => s.Name == storageName) == null)
            {
                storages.Add(new Storage(storageName));
                Console.WriteLine($"Склад \"{storageName}\" создан");
                Helper.SerializeStorage(storages);
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Склад с таким именем уже существует!");
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
            }
        }
        void ShowProduct()
        {
            while (true)
            {
                List<Storage> storages = Helper.DeserializeStorage();
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

                    List<string> products = new List<string>();
                    foreach (Product product in storages[chooseStorage].Products)
                        products.Add($"Товар \"{product.Name}\" | Категория {product.Category}");
                    products.Add("Назад");
                    ConsoleMenu productMenu = new ConsoleMenu(products.ToArray());
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

                    Helper.SerializeStorage(storages);
                }
            }
        }
        void AddProductToStorage()
        {
            while (true)
            {
                List<Storage> storages = Helper.DeserializeStorage();
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
                    ConsoleMenu addOrBackMenu = new ConsoleMenu(new string[] { "Добавить товар", "Назад" });
                    if (addOrBackMenu.PrintMenu() == 1) break;
                    Console.Clear();
                    Console.WriteLine(storagesName[chooseStorage]);
                    storages[chooseStorage].Products.Add(new Product());
                    Helper.SerializeStorage(storages);
                }
            }
        }
        void MoveProduct()
        {
            List<Storage> storages = Helper.DeserializeStorage();
            if (storages.Count == 0)
            {
                Console.WriteLine("Не найдено cкладов.");
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
                return;
            }
            List<string> storageNames = new List<string>();
            foreach (Storage st in storages)
                storageNames.Add(st.Name);

            //выбираем склад-отправитель
            Console.Clear();
            Console.WriteLine("Выберите склад, с которого будете перемещать товар");
            Console.WriteLine("Нажмите любую кнопку...");
            Console.ReadKey();
            ConsoleMenu storageMenu = new ConsoleMenu(storageNames.ToArray());
            int index1 = storageMenu.PrintMenu();
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
            Console.WriteLine("Нажмите любую кнопку...");
            Console.ReadKey();
            List<string> products = new List<string>();
            foreach (Product product in storages[index1].Products)
                products.Add($"Товар \"{product.Name}\" | Категория {product.Category} | {product.Price} рублей за один товар | {product.Count} шт.");
            ConsoleMenu productMenu = new ConsoleMenu(products.ToArray());
            int chooseProduct = productMenu.PrintMenu();

            //выбираем склад-получатель
            Console.Clear();
            Console.WriteLine("Выберите склад, на который будете перемещать товар");
            Console.WriteLine("Нажмите любую кнопку...");
            Console.ReadKey();
            storageNames.Remove(storages[index1].Name);
            storageMenu = new ConsoleMenu(storageNames.ToArray());
            int index2 = storageMenu.PrintMenu();
            index2 = storages.FindIndex(s => s.Name == storageNames[index2]);

            //перемещаем товар
            Product movableProduct = storages[index1].Products[chooseProduct];
            storages[index1].Products.Remove(movableProduct);
            int prIndex = storages[index2].Products.FindIndex(p => p.Name == movableProduct.Name && p.Category == movableProduct.Category && p.Price == movableProduct.Price && p.ShelfLife == movableProduct.ShelfLife);
            if (prIndex != -1)
                storages[index2].Products[prIndex].Count += movableProduct.Count;
            else
                storages[index2].Products.Add(movableProduct);

            Helper.SerializeStorage(storages);
        }
        void EditProduct()
        {
            while (true)
            {
                List<Storage> storages = Helper.DeserializeStorage();
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

                    string[] productData =
                    {
                    "Название товара - " + storages[chooseStorage].Products[chooseProduct].Name,
                    "Категория - " + storages[chooseStorage].Products[chooseProduct].Category,
                    "Цена за единицу товара - " + storages[chooseStorage].Products[chooseProduct].Price.ToString(),
                    "Количество товара - " + storages[chooseStorage].Products[chooseProduct].Count.ToString(),
                    "Срок годности (до) - " + storages[chooseStorage].Products[chooseProduct].ShelfLife,
                    "Назад"
                    };
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
        void DeffectProduct()
        {
            while (true)
            {
                List<Storage> storages = Helper.DeserializeStorage();
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
                    Console.WriteLine("Товар удалён со склада!");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    if (storages[chooseStorage].Products.Count == 0) break;
                }
            }

        }
        public override int MainMenu()
        {
            string[] functions = {
                "Создать склад",
                "Просмотреть товар",
                "Добавить новый товар на склад",
                "Переместить товар с одного склада на другой",
                "Изменить данные о товаре на складе",
                "Забраковать товар (удалить товар со склада)",
                "Выйти из аккаунта",
                "Выйти из приложения" };
            ConsoleMenu adminMenu = new ConsoleMenu(functions);
            int chooseFunc = adminMenu.PrintMenu();
            Console.Clear();
            switch (chooseFunc)
            {
                case 0:
                    CreateStorage();
                    break;
                case 1:
                    ShowProduct();
                    break;
                case 2:
                    AddProductToStorage();
                    break;
                case 3:
                    MoveProduct();
                    break;
                case 4:
                    EditProduct();
                    break;
                case 5:
                    DeffectProduct();
                    break;
                default:
                    if (chooseFunc == functions.Length - 1)
                        return -1;
                    else return 1;
            }
            return 0;
        }
    }
}
