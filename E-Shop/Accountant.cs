using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    class Accountant : Account
    {
        public override string Role { get; } = nameof(Accountant);

        public override string[] Functions => throw new NotImplementedException();

        public override Helper.Method[] MyFunctions => throw new NotImplementedException();
    }
}
