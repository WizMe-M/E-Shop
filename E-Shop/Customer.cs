using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    class Customer : Account
    {
        public override string Role { get; } = nameof(Customer);
    }
}
