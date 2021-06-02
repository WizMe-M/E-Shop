using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    [Serializable]
    class Receipt
    {
        public string ShopName { get; set; }
        public string EMail { get; set; }
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
            BuyProducts = new List<Product>();
        }
        public Receipt(Customer c, Shop shop) : this()
        {
            EMail = c.Email;
            BuyProducts = c.ShopList;
            ShopName = shop.Name;
        }
        public int PriceForProducts(Product product)
        {
            int fullPrice = product.Price * product.Count;
            return fullPrice;
        }

    }
}
