﻿using System.Collections.Generic;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Stuff
{
    public delegate void UserSettingValuesChanged(List<IUserSettingChange> changes);
}
