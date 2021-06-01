using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace E_Shop
{
    //Продавец
    [Serializable]
    class Seller : Account
    {
        Seller()
        {
            Position = "Продавец";
            WorkPlace = "Магазин";
        }
        public Seller(string Login, string Password) : base(Login, Password)
        {
            Position = "Продавец";
            //сделать выбор магазина? тогда нужно будет привязать квитанции к определенному магазину, уф
            AddShop();
            WorkPlace = "Магазин";
        }
        void AddShop()
        {
            List<Shop> shops = Helper.DeserializeShops();
            Console.WriteLine("Введите название магазина:");
            string name = Console.ReadLine().Trim();
            shops.Add(new Shop(name));
            Helper.SerializeShops(shops);
        }
        void Checkout()
        {
            while (true)
            {
                List<Shop> shops = Helper.DeserializeShops();
                List<Receipt> receipts = Helper.DeserializeReceipt();
                if (receipts.Count == 0)
                {
                    Console.WriteLine("Нет неоформленных квитанций");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    return;
                }
                List<string> receiptNames = new List<string>();
                foreach (Receipt r in receipts)
                    receiptNames.Add($"ID {r.ID } | {r.CustomerFILogin} | {r.FullPrice} рублей");
                receiptNames.Add("Назад");
                ConsoleMenu receiptMenu = new ConsoleMenu(receiptNames.ToArray());
                int choose = receiptMenu.PrintMenu();
                if (choose == receiptNames.Count - 1) return;

                int shopIndex = shops.FindIndex(s => s.Name == receipts[choose].ShopName);

                //вычитаем со склада N товаров
                List<Storage> storages = Helper.DeserializeStorage();
                int ind = storages.FindIndex(st => st.Name == shops[shopIndex].AttachedStorage.Name);
                storages.RemoveAt(ind);
                foreach (Product product in receipts[choose].BuyProducts)
                {
                    //ищем товар на складе, совпадающий по всем параметрам (кроме количества, разумеется)
                    int i = shops[shopIndex].AttachedStorage.Products.FindIndex
                        (p => p.Name == product.Name
                        && p.Category == product.Category
                        && p.Price == product.Price
                        && p.ShelfLife == product.ShelfLife);
                    if (i != -1)
                        shops[shopIndex].AttachedStorage.Products[i].Count -= product.Count;
                }
                storages.Insert(ind, shops[shopIndex].AttachedStorage);

                //создаем текст квитанции
                string message = $"\tКвитанция #{receipts[choose].ID}" +
                    $"\n--------------------------------------";
                foreach (Product p in receipts[choose].BuyProducts)
                {
                    message += $"\n{p.Name} | {p.Category} | {p.Price} р. за один товар, " +
                        $"{receipts[choose].PriceForProducts(p)} р. за {p.Count} товаров\n";
                }
                message += $"\n--------------------------------------" +
                    $"\tОбщая сумма заказа: {receipts[choose].FullPrice} рублей";

                //удаляем заказ из списка
                receipts.RemoveAt(choose);
                Helper.SerializeReceipt(receipts);
                Helper.SerializeStorage(storages);

                //здесь отправляем сообщение "на почту"
                Console.WriteLine(message);
                Thread.Sleep(1000);
                Console.WriteLine("Квитанция отправлена на почту покупателю");
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
            }
        }
        public override int MainMenu()
        {
            string[] functions = {
                "Добавить магазин",
                "Оформить квитанцию",
                "Выйти из аккаунта",
                "Выйти из приложения" };
            ConsoleMenu adminMenu = new ConsoleMenu(functions);
            int chooseFunc = adminMenu.PrintMenu();
            Console.Clear();
            switch (chooseFunc)
            {
                case 0:
                    AddShop();
                    break;
                case 1:
                    Checkout();
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
