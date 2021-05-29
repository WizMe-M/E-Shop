using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace E_Shop
{
    //Кадровик
    class Personnel : Account
    {
        public override string Role { get; } = nameof(Personnel);

        public override int MainFunction()
        {
            return 0;
        }
    }
}
