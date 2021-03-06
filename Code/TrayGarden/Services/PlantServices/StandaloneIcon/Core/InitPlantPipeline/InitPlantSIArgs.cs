﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;

#endregion

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
  public class InitPlantSIArgs : PipelineArgs
  {
    #region Constructors and Destructors

    public InitPlantSIArgs(
      [NotNull] IPlantEx plantEx,
      [NotNull] string luggageName,
      EventHandler closeComponentClick,
      EventHandler exitGardenClick)
    {
      Assert.ArgumentNotNull(plantEx, "plantEx");
      Assert.ArgumentNotNullOrEmpty(luggageName, "luggageName");
      this.PlantEx = plantEx;
      this.LuggageName = luggageName;
      this.CloseComponentClick = closeComponentClick;
      this.ExitGardenClick = exitGardenClick;
    }

    #endregion

    #region Public Properties

    public EventHandler CloseComponentClick { get; set; }

    public EventHandler ExitGardenClick { get; set; }

    public string LuggageName { get; set; }

    public IPlantEx PlantEx { get; protected set; }

    public StandaloneIconPlantBox SIBox { get; set; }

    #endregion
  }
}