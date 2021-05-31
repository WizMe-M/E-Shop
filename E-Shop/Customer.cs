using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace E_Shop
{
    [Serializable]
    class Customer : Account
    {
        string email;
        public string Email
        {
            get { return email; }
            set
            {
                string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
                while (!Regex.IsMatch(value, pattern))
                {
                    Console.Clear();
                    Console.WriteLine("Введите корректный адрес электронной почты:");
                    value = Console.ReadLine().Trim();
                }
                email = value;
            }
        }
        public List<Product> ShopList { get; set; }
        public Customer() : base()
        {
            Position = "Покупатель";
            Console.Clear();
            Console.WriteLine("Введите электронный адрес, на который хотите получать чеки:");
            Email = Console.ReadLine().Trim();
        }
        public Customer(string Login, string Password) : base(Login, Password)
        {
            Position = "Покупатель";
            Console.Clear();
            Console.WriteLine("Введите электронный адрес, на который хотите получать чеки:");
            Email = Console.ReadLine().Trim();
        }
        void ShowSelf()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Данные пользователя {Email}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"ФИО пользователя: {LastName} {FirstName} {Patronomic}");
            Console.WriteLine($"Логин аккаунта:\t\t{Login}");
            Console.WriteLine($"Пароль аккаунта:\t{Password}");
            Console.WriteLine("Нажмите любую кнопку...");
            Console.ReadKey();
        }
        void AddProduct(Shop shop)
        {
            while (true)
            {
                List<Product> shopProducts = new List<Product>();

                shopProducts.AddRange(shop.AttachedStorage.Products);
                shopProducts.RemoveAll(p => p.Count == 0);
                if (shopProducts.Count == 0)
                {
                    Console.WriteLine("В выбранном магазине нет товаров...");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    return;
                }

                List<string> productNames = new List<string>();
                foreach (Product product in shopProducts)
                    productNames.Add($"Товар \"{product.Name}\" | Цена: {product.Price} рублей");
                productNames.Add("Назад");

                ConsoleMenu productMenu = new ConsoleMenu(productNames.ToArray());
                int chooseProduct = productMenu.PrintMenu();
                if (chooseProduct == productNames.Count - 1) break;

                ShopList.Add(shop.AttachedStorage.Products[chooseProduct]);
                ShopList[^1].Count = 1;
            }
        }
        void RemoveProduct()
        {
            while (true)
            {
                if (ShopList.Count == 0)
                {
                    Console.WriteLine("Корзина товаров пуста.");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    return;
                }
                List<string> productNames = new List<string>();
                foreach (Product product in ShopList)
                    productNames.Add($"Товар \"{product.Name}\" | Цена: {product.Price} рублей | Количество: {product.Count}");
                productNames.Add("Назад");

                ConsoleMenu productMenu = new ConsoleMenu(productNames.ToArray());
                int chooseProduct = productMenu.PrintMenu();
                if (chooseProduct == productNames.Count - 1) break;

                ShopList.RemoveAt(chooseProduct);
            }
        }
        void EditCount(Shop shop)
        {
            while (true)
            {
                if (ShopList.Count == 0)
                {
                    Console.WriteLine("Нет добавленных в корзину товаров...");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    return;
                }

                List<string> productNames = new List<string>();
                foreach (Product product in ShopList)
                    productNames.Add($"Товар \"{product.Name}\" | Цена: {product.Price} рублей | Количество: {product.Count}");
                productNames.Add("Назад");

                ConsoleMenu productMenu = new ConsoleMenu(productNames.ToArray());
                int chooseProduct = productMenu.PrintMenu();
                if (chooseProduct == productNames.Count - 1) break;

                //ищем на складе товар с такими же данными
                Product some = shop.AttachedStorage.Products.Find
                    (p => p.Name == ShopList[chooseProduct].Name
                    && p.Category == ShopList[chooseProduct].Category
                    && p.Price == ShopList[chooseProduct].Price
                    && p.ShelfLife == ShopList[chooseProduct].ShelfLife);
                ShopList[chooseProduct].Count = some.ChooseCountOfProducts();
            }
        }
        void FinalizeOrder(Shop shop)
        {
            if (ShopList.Count != 0)
            {
                Helper.SerializeReceipt(shop, this);
                ShopList.Clear();
                ShopList.TrimExcess();
                Console.WriteLine("Ваш заказ оформлен!");
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Ваша корзина пуста");
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
            }
        }
        void ShoppingBasket()
        {
            while (true)
            {
                List<Shop> shops = Helper.DeserializeShops();
                if (shops.Count == 0)
                {
                    Console.WriteLine("Не найдено магазинов");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    return;
                }

                List<string> shopNames = new List<string>();
                foreach (Shop shop in shops)
                    shopNames.Add(shop.Name);
                shopNames.Add("Назад");
                ConsoleMenu shopMenu = new ConsoleMenu(shopNames.ToArray());
                int chooseShop = shopMenu.PrintMenu();
                if (chooseShop == shopNames.Count - 1) break;

                string[] basket =
                {
                "Добавить товар в корзину",
                "Удалить товар из корзины",
                "Изменить количество товара",
                "Оформить заказ",
                "Выйти из меню"
                };
                ConsoleMenu basketMenu = new ConsoleMenu(basket);
                while (true)
                {
                    int choose = basketMenu.PrintMenu();
                    Console.Clear();
                    switch (choose)
                    {
                        case 0:
                            AddProduct(shops[chooseShop]);
                            break;
                        case 1:
                            RemoveProduct();
                            break;
                        case 2:
                            EditCount(shops[chooseShop]);
                            break;
                        case 3:
                            FinalizeOrder(shops[chooseShop]);
                            break;
                        default:
                            return;
                    }
                }

            }
        }

        public override int MainMenu()
        {
            string[] functions =
            {
                "Просмотреть информацию о себе",
                "Оформление заказа (корзина)",
                "Выйти из аккаунта",
                "Выйти из приложения"
            };
            ConsoleMenu mainMenu = new ConsoleMenu(functions);
            int chooseFunc = mainMenu.PrintMenu();
            Console.Clear();
            switch (chooseFunc)
            {
                case 0:
                    ShowSelf();
                    break;
                case 1:
                    ShoppingBasket();
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
