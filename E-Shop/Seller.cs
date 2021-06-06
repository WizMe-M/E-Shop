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
        public Seller() : base()
        {
            Position = "Продавец";
            Functions.AddRange(new (string, Method)[] {
                ("Добавить магазин", AddShop),
                ("Оформить квитанцию", Checkout)});
            WorkPlace = "Магазин";
        }
        //public Seller(string Login, string Password) : base(Login, Password)
        //{
        //    Position = "Продавец";
        //    Functions.AddRange(new (string, Method)[] {
        //        ("Добавить магазин", AddShop),
        //        ("Оформить квитанцию", Checkout)});
        //    WorkPlace = "Магазин";
        //}
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
                List<Receipt> receipts = Helper.DeserializeReceipt();
                List<int> idReceipts = new List<int>();
                List<string> receiptNames = new List<string>();
                foreach (Receipt r in receipts)
                    if (!r.isRegistered)
                    {
                        receiptNames.Add($"{r.EMail} | {r.CustomerFIO} | {r.ShopName} | {r.FullPrice} рублей");
                        idReceipts.Add(r.ID);
            }
                if (receiptNames.Count == 0)
                {
                    Console.WriteLine("Нет неоформленных квитанций.");
                    Console.WriteLine("Для оформления квитанций сначала нужно, чтобы покупатель оформил заявку.");
                    Thread.Sleep(1000);
                    return;
                }
                receiptNames.Add("Назад");

                ConsoleMenu receiptMenu = new ConsoleMenu(receiptNames.ToArray());
                int choose = receiptMenu.PrintMenu();
                if (choose == receiptNames.Count - 1) return;

                //проверка регистрации на всякий случай
                int index = receipts.FindIndex(r => !r.isRegistered && r.ID == idReceipts[choose]);

                if (receipts[index].RegisterReceipt())
                {
                    Helper.SerializeReceipt(receipts);
                    //создаем текст квитанции
                    string mes = CreateMessage(receipts[index]);
                    Mail mail = new Mail(receipts[index].EMail, mes);
                    mail.SendMessage();
                    Console.WriteLine("Квитанция отправлена на почту покупателю");
                }
                else Console.WriteLine("Нельзя оформить заказ");
                Thread.Sleep(1000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Thread.Sleep(2000);
            }
        }
        
        string CreateMessage(Receipt receipt)
        {
            string message = $"\t#Квитанция магазина {receipt.ShopName}#" + $"\n\tПокупатель: {receipt.CustomerFIO}" +
                        $"\n------------------------------------------------------------------------------------------------------------------";
            foreach (Product p in receipt.BuyProducts)
                message += $"\n{p.Name} | {p.Category} | {p.Price} р. за один товар | {receipt.PriceForProducts(p)} р. за {p.Count} товаров\n";
            message += $"\n------------------------------------------------------------------------------------------------------------------" +
                $"\n\tОбщая сумма заказа: {receipt.FullPrice} рублей";


            return message;
        }
        public override void OnDeserializing()
        {
            base.OnDeserializing();
            Functions.AddRange(new (string, Method)[] {
                ("Добавить магазин", AddShop),
                ("Оформить квитанцию", Checkout)});
        }

    }
}
