using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace E_Shop
{
    [Serializable]
    class Storage
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
                while(!Regex.IsMatch(value, pattern))
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
        List<Product> products = new List<Product>();
        public List<Product> Products
        {
            get { return products; }
            set
            {
                products = value;
                Storage_OnChangeProducts();
            }
        }
        Storage()
        {
            Products = new List<Product>();
        }
        public Storage(string Name) : this()
        {
            this.Name = Name;
        }

        private void Storage_OnChangeProducts()
        {
            List<Shop> shops = Helper.DeserializeShops();
            for (int i = 0; i < shops.Count; i++)
                if (shops[i].AttachedStorage.Name == Name)
                    shops[i].AttachedStorage = this;
            Helper.SerializeShops(shops);
        }
        public void AddOrIncrementProduct(Product product)
        {
            int index = Products.FindIndex(p =>
                p.Name == product.Name
                && p.Category == product.Category
                && p.Price == product.Price
                && p.ShelfLife == product.ShelfLife);
            if (index == -1)
                Products.Add(product);
            else Products[index].Count += product.Count;
            Products = Products;
        }
    }
}
