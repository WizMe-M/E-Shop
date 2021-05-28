using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    class Seller : Account
    {
        public override string Role { get; } = nameof(Seller);
    }
}
