﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  public interface IUserSettingMetadataMaster<T> : ITypedUserSettingMetadata<T>
  {
    #region Public Methods and Operators

    void Initialize(string name, string title, T defaultValue, string description, object additionalParams, IUserSettingHallmark hallmark);

    #endregion
  }
}