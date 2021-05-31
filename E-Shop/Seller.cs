using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    class Seller : Account
    {
        void Checkout()
        {
            while (true)
            {
                List<Shop> shops = Helper.DeserializeShops();
                List<Receipt> receipts = Helper.DeserializeReceipt();
                List<string> receiptNames = new List<string>();
                foreach (Receipt r in receipts)
                    receiptNames.Add($"ID {r.ID } | {r.CustomerFILogin} | {r.FullPrice} рублей");
                receiptNames.Add("Назад");
                ConsoleMenu receiptMenu = new ConsoleMenu(receiptNames.ToArray());
                int choose = receiptMenu.PrintMenu();
                int shopIndex = shops.FindIndex(s => s.Name == receipts[choose].ShopName);

                //вычитаем со склада
                foreach (Product product in receipts[choose].BuyProducts)
                {
                    int i = shops[shopIndex].AttachedStorage.Products.FindIndex
                        (p => p.Name == product.Name
                        && p.Category == product.Category
                        && p.Price == product.Price
                        && p.ShelfLife == product.ShelfLife);
                    if (i != -1)
                        shops[shopIndex].AttachedStorage.Products[i].Count -= product.Count;
                }

                string message = $"--------------------------------------";
                foreach (Product p in receipts[choose].BuyProducts)
                {
                    message += $"\n{p.Name} | {p.Category} | {p.Price} за один товар, " +
                        $"{receipts[choose].PriceForProducts(p)} за {p.Count} товаров\n";
                }
                message += $" Общая сумма заказа: {receipts[choose].FullPrice}" +
                    $"\n--------------------------------------";
                Console.WriteLine(message);
            }
        }
        public override int MainMenu()
        {
            string[] functions = {
                "Оформить квитанции",
                "Выйти из аккаунта",
                "Выйти из приложения" };
            ConsoleMenu adminMenu = new ConsoleMenu(functions);
            int chooseFunc = adminMenu.PrintMenu();
            Console.Clear();
            switch (chooseFunc)
            {
                case 0:
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
