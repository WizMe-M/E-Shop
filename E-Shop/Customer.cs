using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

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
        public List<Product> ShopList { get; set; } = new List<Product>();
        //экземпляр магазина для работы с корзиной
        Shop ThisShop;
        public Customer(string Login, string Password) : base(Login, Password)
        {
            Position = "Покупатель";
            Functions.AddRange(new (string, Method)[] {
                ("Просмотреть информацию о себе", ShowSelf),
                ("Добавить товар в корзину", AddProduct),
                ("Изменить количество товара в корзине", EditCount),
                ("Убрать товар из корзины", RemoveProduct),
                ("Оформить заказ", FinalizeOrder)});
            Console.Clear();
            Console.WriteLine("Введите электронный адрес, на который хотите получать чеки:");
            Email = Console.ReadLine().Trim();
        }
        private void ShowSelf()
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
        void AddProduct()
        {
            while (true)
            {
                if (ThisShop == null)
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
                    if (chooseShop == shopNames.Count - 1) return;
                    ThisShop = shops[chooseShop];
                }

                //получаем продукты из выбранного магазина
                List<Product> shopProducts = new List<Product>();
                shopProducts.AddRange(ThisShop.AttachedStorage.Products);
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

                if (!ShopList.Contains(ThisShop.AttachedStorage.Products[chooseProduct]))
                {
                    ShopList.Add(ThisShop.AttachedStorage.Products[chooseProduct]);
                    ShopList[^1].Count = 1;
                    Console.WriteLine("Товар добавлен в корзину");
                }
                else Console.WriteLine("Такой товар уже есть в вашей корзине!");
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
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
                if (ShopList.Count == 0) ThisShop = null;
            }
        }
        void EditCount()
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
                Product some = ThisShop.AttachedStorage.Products.Find
                    (p => p.Name == ShopList[chooseProduct].Name
                    && p.Category == ShopList[chooseProduct].Category
                    && p.Price == ShopList[chooseProduct].Price
                    && p.ShelfLife == ShopList[chooseProduct].ShelfLife);

                int count;
                do
                {
                    Console.Clear();
                    Console.Write("Введите количество товара: ");
                    count = Console.ReadLine().SafeParse();
                }
                while (count <= 0);
                ShopList[chooseProduct].Count = count;
                //ShopList[chooseProduct].Count = some.ChooseCountOfProducts();
            }
        }
        void FinalizeOrder()
        {
            if (ShopList.Count != 0)
            {
                Helper.AddReceiptToBD(this, ThisShop);
                ShopList.Clear();
                ThisShop = null;
                Console.WriteLine("Ваш заказ отправлен на валидацию!");
            }
            else Console.WriteLine("Ваша корзина пуста");
            Thread.Sleep(1000);
        }

        public override void OnDeserializing()
        {
            base.OnDeserializing();
            Functions.AddRange(new (string, Method)[] {
                ("Просмотреть информацию о себе", ShowSelf),
                ("Добавить товар в корзину", AddProduct),
                ("Изменить количество товара в корзине", EditCount),
                ("Убрать товар из корзины", RemoveProduct),
                ("Оформить заказ", FinalizeOrder)});
        }

    }
}
