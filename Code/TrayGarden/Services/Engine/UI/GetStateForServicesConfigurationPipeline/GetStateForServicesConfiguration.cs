﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.WindowWithReturn;

#endregion

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline
{
  internal static class GetStateForServicesConfiguration
  {
    #region Public Methods and Operators

    public static WindowStepState Run([NotNull] GetStateForServicesConfigurationPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      HatcherGuide<IPipelineManager>.Instance.InvokePipeline("getStateForServicesConfiguration", args);
      return args.Aborted ? null : args.Result as WindowStepState ?? args.StateConstructInfo.ResultState;
    }

    #endregion
  }
}