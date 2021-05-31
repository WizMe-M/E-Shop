using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    [Serializable]
    class Receipt
    {
        public int ID { get; } = 0;
        public string ShopName { get; set; }
        public string CustomerFILogin { get; set; }
        public Customer Customer { get; set; }
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
        public Receipt()
        {
            ID = NextID();
            BuyProducts = new List<Product>();
        }
        public Receipt(Customer customer, Shop shop) : this()
        {
            Customer = customer;
            BuyProducts = customer.ShopList;
            ShopName = shop.Name;
        }
        int NextID()
        {
            List<Receipt> r = Helper.DeserializeReceipt();
            if (r.Count == 0)
                return 0;
            return r[^1].ID + 1;
        }
        public int PriceForProducts(Product product)
        {
            int fullPrice = product.Price * product.Count;
            return fullPrice;
        }

    }
}
