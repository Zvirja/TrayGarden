﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific
{
  public interface IStringUserSetting : IUserSettingBase
  {
    string StringValue { get; set; }
  }
}