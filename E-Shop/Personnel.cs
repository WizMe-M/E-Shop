using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace E_Shop
{
    class Personnel : Account
    {
        public override string Role { get; } = nameof(Personnel);

        public override string[] Functions => throw new NotImplementedException();

        public override Helper.Method[] MyFunctions => throw new NotImplementedException();
    }
}
