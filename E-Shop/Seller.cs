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
            Position = "Продавец";
            Functions.AddRange(new (string, Method)[] {
                ("Добавить магазин", AddShop),
                ("Оформить квитанцию", Checkout)});
            AddShop();
            Console.Clear();
            Console.WriteLine("Выберите магазин, в котором будете работать:");
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
            try
            {
                List<Shop> shops = Helper.DeserializeShops();
                int shopIndex = shops.FindIndex(s => s.Name == WorkPlace);
                Receipt registeredReceipt = shops[shopIndex].RegisterReceipt();
                if (registeredReceipt != null)
                {


                    //создаем текст квитанции
                    string message = $"\t#Квитанция магазина {shops[shopIndex].Name}#" +
                        $"\n------------------------------------------------------------------------------------------------------------------";
                    foreach (Product p in registeredReceipt.BuyProducts)
                        message += $"\n{p.Name} | {p.Category} | {p.Price} р. за один товар, " +
                            $"{registeredReceipt.PriceForProducts(p)} р. за {p.Count} товаров\n";
                    message += $"\n------------------------------------------------------------------------------------------------------------------" +
                        $"\n\tОбщая сумма заказа: {registeredReceipt.FullPrice} рублей";

                    
                    
                    Mail mail = new Mail(registeredReceipt.EMail, message);
                    mail.SendMessage();


                    shops[shopIndex].UnregisteredOrders.Remove(registeredReceipt);
                    Helper.SerializeShops(shops);
                    Thread.Sleep(1000);
                    Console.WriteLine("Квитанция отправлена на почту покупателю");
                }
                else Console.WriteLine("Нельзя оформить заказ");
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Thread.Sleep(1500);
            }
        }
        public override void OnDeserializing()
        {
            Functions = new List<(string, Method)>();
            Functions.AddRange(new (string, Method)[] {
                ("Добавить магазин", AddShop),
                ("Оформить квитанцию", Checkout)});
        }

    }
}
