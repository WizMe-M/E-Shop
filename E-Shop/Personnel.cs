using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace E_Shop
{
    class Personnel : Account
    {
        public override string Role { get; } = typeof(Personnel).Name;
        public Personnel()
        {
            
        }

    }
}
