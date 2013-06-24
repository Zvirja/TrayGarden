﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Services.Engine;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Pipelines.Startup
{
    public class PlantServicesInformDisplay
    {
        public void Process(StartupArgs args)
        {
            HatcherGuide<IServicesSteward>.Instance.InformDisplayStage();
        }
    }
}