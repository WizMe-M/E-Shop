using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace E_Shop
{
    [Serializable]
    class Shop
    {
        string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                string pattern = @"^(([A-Z](?:[a-zA-Z0-9 \.\-]+))|([А-Я](?:[а-яА-Я0-9 \.\-]+)))$";
                while (!Regex.IsMatch(value, pattern))
                {
                    Console.Clear();
                    Console.WriteLine("Название склада должно начинаться с заглавной буквы и " +
                        "состоять либо из кириллицы, либо латиницы. " +
                        "\nРазрешены: числа, точка, тире и пробел.");
                    Console.Write("Введите название склада: ");
                    value = Console.ReadLine();
                }
                name = value;
            }
        }
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

        public Shop(string Name)
        {
            this.Name = Name;
            ChooseAttachedStorage();
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
