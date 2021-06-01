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
        Storage()
        {
            Products = new List<Product>();

        }
        public Storage(string Name) : this()
        {
            this.Name = Name;
        }
    }
}
