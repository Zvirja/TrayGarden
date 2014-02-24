﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners
{
    public class NewListDirectAssigner : ListAssigner
    {
        protected override System.Collections.IList GetListObject(System.Xml.XmlNode contentNode, object instance, System.Type instanceType)
        {
            return instance as IList;
        }
    }
}