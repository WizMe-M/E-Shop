﻿using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    class Warehouseman : Account
    {
        public override string Role { get; } = nameof(Warehouseman);

        public override string[] Functions => throw new NotImplementedException();

        public override Helper.Method[] MyFunctions => throw new NotImplementedException();
    }
}
