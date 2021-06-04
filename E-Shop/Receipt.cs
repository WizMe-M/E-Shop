using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace E_Shop
{
    [Serializable]
    class Receipt
    {
        public int ID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool isRegistered { get; set; }
        public string ShopName { get; set; }
        public string EMail { get; set; }
        public string CustomerFIO { get; set; }
        public List<Product> BuyProducts { get; set; }
        public int FullPrice 
        {
            get
            {
                int price = 0;
                foreach (Product product in BuyProducts)
                    price += PriceForProducts(product);
                return price;
            }
        }
        Receipt()
        {
            ID = Helper.DeserializeReceipt().Count + 1;
            isRegistered = false;
            BuyProducts = new List<Product>();
        }
        public Receipt(Customer c, Shop shop) : this()
        {
            EMail = c.Email;
            CustomerFIO = $"{c.FirstName} {c.LastName} {c.Patronomic}";
            BuyProducts = c.ShopList;
            ShopName = shop.Name;
        }
        public int PriceForProducts(Product product)
        {
            int fullPrice = product.Price * product.Count;
            return fullPrice;
        }
        public bool RegisterReceipt()
        {
            List<Shop> shops = Helper.DeserializeShops();
            int shopIndex = shops.FindIndex(s => s.Name == ShopName);

            //вычитаем из магазина (склада, привязанного к магазину) количество товаров из списка
            foreach (Product product in BuyProducts)
            {
                //ищем товар на складе, совпадающий по всем параметрам (кроме количества, разумеется)
                int i = shops[shopIndex].AttachedStorage.Products.FindIndex
                    (p => p.Name == product.Name
                    && p.Category == product.Category
                    && p.Price == product.Price
                    && p.ShelfLife == product.ShelfLife);

                if (i != -1)
                    if (shops[shopIndex].AttachedStorage.Products[i].Count > product.Count)
                    {
                        shops[shopIndex].AttachedStorage.Products[i].Count -= product.Count;
                        shops[shopIndex].AttachedStorage = shops[shopIndex].AttachedStorage;
                    }
                    else return false;
            }

            isRegistered = true;
            RegistrationDate = DateTime.Now;

            return true;
        }

    }
}
