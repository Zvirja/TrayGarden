﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Configuration
{
    public interface IPrototype
    {
        object CreateNewInializedInstance();
    }
}