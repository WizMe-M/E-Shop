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
        public Seller(string Login, string Password) : base(Login, Password)
        {
            Functions.AddRange(new (string, Method)[]
            {
                ("Добавить магазин", AddShop),
                ("Оформить квитанцию", Checkout)
            });
            Position = "Продавец";
            AddShop();
            WorkPlace = Helper.ChooseShop().Name;
        }
        private void AddShop()
        {
            Console.WriteLine("Введите название магазина:");
            string name = Console.ReadLine().Trim();
            Helper.AddShopToBD(new Shop(name));            
        }
        private void Checkout()
        {
            while (true)
            {
                List<Shop> shops = Helper.DeserializeShops();
                int shopIndex = shops.FindIndex(s => s.Name == WorkPlace);
                Receipt registeredReceipt = shops[shopIndex].RegisterReceipt();
                if (registeredReceipt != null)
                {
                    shops[shopIndex].UnregisteredOrders.Remove(registeredReceipt);
                    //создаем текст квитанции
                    string message = $"\t#Квитанция магазина {shops[shopIndex].Name}#" +
                        $"\n--------------------------------------";
                    foreach (Product p in registeredReceipt.BuyProducts)                    
                        message += $"\n{p.Name} | {p.Category} | {p.Price} р. за один товар, " +
                            $"{registeredReceipt.PriceForProducts(p)} р. за {p.Count} товаров\n";                    
                    message += $"\n--------------------------------------" +
                        $"\tОбщая сумма заказа: {registeredReceipt.FullPrice} рублей";

                    Helper.SerializeShops(shops);

                    //здесь отправляем сообщение "на почту" (нужен метод)
                    Console.WriteLine(message);

                    Thread.Sleep(1000);
                    Console.WriteLine("Квитанция отправлена на почту покупателю");
                }
                else
                {
                    Console.WriteLine("Нельзя оформить заказ:");
                    Console.WriteLine("Некоторые товары из списка не соответствуют количеству на складе.");
                }
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
            }
        }
    }
}
