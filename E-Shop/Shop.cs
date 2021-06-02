using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    [Serializable]
    class Shop
    {
        public string Name { get; set; }

        Storage storage;
        public Storage AttachedStorage
        {
            get { return storage; }
            set
            {
                storage = value;
                Shop_OnChangeStorage();
            }
        }
        public List<Receipt> UnregisteredOrders { get; set; } = new List<Receipt>();
        Shop()
        {
            UnregisteredOrders = new List<Receipt>();
            ChooseAttachedStorage();
        }
        public Shop(string Name) : this()
        {
            this.Name = Name;
        }
        void Shop_OnChangeStorage()
        {
            List<Storage> storages = Helper.DeserializeStorage();
            int index = storages.FindIndex(st => st.Name == AttachedStorage.Name);
            storages.RemoveAt(index);
            storages.Insert(index, AttachedStorage);
            Helper.SerializeStorage(storages);
        }
        void ChooseAttachedStorage()
        {
            List<Storage> storages = Helper.DeserializeStorage();
            if (storages.Count == 0)
            {
                Console.WriteLine("Нет складов в базе данных! Создайте новый склад.");
                Console.WriteLine("Введите название склада:");
                string name = Console.ReadLine();
                storages.Add(new Storage(name));
                Helper.SerializeStorage(storages);
            }

            Console.Clear();
            Console.WriteLine("Выберите склад, к которому будет привязан магазин:");
            Console.WriteLine("Нажмите любую кнопку, чтобы перейти к выбору...");
            Console.ReadKey();

            List<string> storageNames = new List<string>();
            foreach (Storage s in storages)
                storageNames.Add(s.Name);
            ConsoleMenu categoryMenu = new ConsoleMenu(storageNames.ToArray());
            int chooseStorage = categoryMenu.PrintMenu();
            AttachedStorage = storages[chooseStorage];
        }
        public void AddReceipt(Customer customer)
        {
            UnregisteredOrders.Add(new Receipt(customer, this));
        }
        public Receipt RegisterReceipt()
        {
            if (UnregisteredOrders.Count == 0)
            {
                Console.WriteLine("Нет неоформленных квитанций для данного магазина");
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
                return null;
            }

            List<string> receiptNames = new List<string>();
            foreach (Receipt r in UnregisteredOrders)
                receiptNames.Add($" {r.EMail} | {Name} | {r.FullPrice} рублей");
            receiptNames.Add("Назад");

            ConsoleMenu receiptMenu = new ConsoleMenu(receiptNames.ToArray());
            int choose = receiptMenu.PrintMenu();
            if (choose == receiptNames.Count - 1) return null;

            //вычитаем из магазина N товаров из списка
            foreach (Product product in UnregisteredOrders[choose].BuyProducts)
            {
                //ищем товар на складе, совпадающий по всем параметрам (кроме количества, разумеется)
                int i = AttachedStorage.Products.FindIndex
                    (p => p.Name == product.Name
                    && p.Category == product.Category
                    && p.Price == product.Price
                    && p.ShelfLife == product.ShelfLife);

                if (i != -1)
                    if (AttachedStorage.Products[i].Count > product.Count)
                    {
                        AttachedStorage.Products[i].Count -= product.Count;
                        AttachedStorage = AttachedStorage;
                    }
                    else return null;
            }
            return UnregisteredOrders[choose];
        }
    }
}
