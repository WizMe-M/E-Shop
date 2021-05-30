using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    [Serializable]
    class Storage
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        public string AttachedShop { get; set; }
        public Storage(string Name)
        {
            this.Name = Name;
            Products = new List<Product>();
            AttachedShop = "Магазин";
        }        
    }
}
