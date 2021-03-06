﻿#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting
{
  public interface IMenuEntriesAppender
  {
    #region Public Methods and Operators

    void AppentMenuStripItem(string text, Icon icon, EventHandler clickHandler, IDynamicStateProvider dynamicStateProvider = null);

    #endregion
  }
}