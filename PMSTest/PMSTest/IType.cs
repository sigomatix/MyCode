﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PMSTest
{
    public interface IType
    {
        IMethodInfo[] GetMethods();
    }
}
