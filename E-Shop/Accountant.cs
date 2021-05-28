using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    class Accountant : Account
    {
        public override string Role { get; } = nameof(Accountant);
    }
}
