using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    class Shop
    {
        public string Name { get; set; }
        public Storage AttachedStorage { get; set; }
        Shop()
        {
            ChooseAttachedStorage();
        }
        public Shop(string Name) : this()
        {
            this.Name = Name;
        }
        void ChooseAttachedStorage()
        {
            Console.Clear();
            Console.WriteLine("Выберите склад, к которому будет привязан магазин:");
            Console.WriteLine("Нажмите любую кнопку, чтобы перейти к выбору...");
            Console.ReadKey();
            List<Storage> storages = Helper.DeserializeStorage();
            List<string> storageNames = new List<string>();
            foreach (Storage s in storages)
                storageNames.Add(s.Name);
            ConsoleMenu categoryMenu = new ConsoleMenu(storageNames.ToArray());
            int chooseStorage = categoryMenu.PrintMenu();
            AttachedStorage = storages[chooseStorage];
        }
    }
}
