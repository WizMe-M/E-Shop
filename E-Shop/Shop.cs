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
        Shop()
        {
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
    }
}
