﻿using System.Xml;
using System.Collections.Generic;

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers
{
    public class BoolParcer : IParcer
    {
        public virtual object ParceNodeValue(XmlNode nodeValue)
        {
            string value = nodeValue.InnerText;
            bool intValue;
            if (bool.TryParse(value, out intValue))
                return intValue;
            return null;
        }
    }
}