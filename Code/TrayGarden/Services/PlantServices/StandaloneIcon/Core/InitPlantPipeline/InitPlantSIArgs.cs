﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
  public class InitPlantSIArgs : PipelineArgs
  {
    public string LuggageName { get; set; }
    public EventHandler CloseComponentClick { get; set; }
    public EventHandler ExitGardenClick { get; set; }
    public IPlantEx PlantEx { get; protected set; }
    public StandaloneIconPlantBox SIBox { get; set; }

    public InitPlantSIArgs([NotNull] IPlantEx plantEx, [NotNull] string luggageName, EventHandler closeComponentClick, EventHandler exitGardenClick)
    {
      Assert.ArgumentNotNull(plantEx, "plantEx");
      Assert.ArgumentNotNullOrEmpty(luggageName, "luggageName");
      PlantEx = plantEx;
      LuggageName = luggageName;
      CloseComponentClick = closeComponentClick;
      ExitGardenClick = exitGardenClick;
    }
  }
}