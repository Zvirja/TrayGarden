﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.WindowWithReturn;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline
{
  [UsedImplicitly]
  public class CreateStep
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(UNConfigurationStepArgs args)
    {
      var constructInfo = args.StateConstructInfo;
      constructInfo.ResultState = new WindowStepState(
        constructInfo.GlobalTitle,
        constructInfo.Header,
        constructInfo.ShortName,
        args.ConfigurationConstructInfo.ResultControlVM,
        constructInfo.SuperAction,
        constructInfo.StateSpecificHelpActions);
    }

    #endregion
  }
}