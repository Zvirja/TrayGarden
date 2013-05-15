﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
    public static class InitPlantSIPipeline
    {
        public static void Run(IPlant plant, string luggageName, EventHandler closeComponentClick, EventHandler exitGardenClick)
        {
            var args = new InitPlantSIArgs(plant, luggageName, closeComponentClick, exitGardenClick);
            HatcherGuide<IPipelineManager>.Instance.InvokePipeline("standaloneIconServiceInitPlant",args);
        }
    }
}
