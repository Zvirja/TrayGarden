﻿#region

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Resources;
using TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;
using TrayGarden.Services.Engine.UI.Intergration;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.Configuration.EntryVM.ExtentedEntry;
using TrayGarden.UI.Configuration.EntryVM.Players;
using TrayGarden.UI.WindowWithReturn;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Integration
{
  [UsedImplicitly]
  public class PlantServiceConfigurator
  {
    #region Constructors and Destructors

    public PlantServiceConfigurator()
    {
      this.Description = "Configure service";
    }

    #endregion

    #region Public Properties

    public string Description { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(GetStateForServicesConfigurationPipelineArgs args)
    {
      //Find setting, related to UserConfiguration service
      var entryRelatedToService =
        args.ConfigConstructInfo.ConfigurationEntries.FirstOrDefault(
          x => ((ConfigurationPlayerService)x.RealPlayer).InfoSource is UserNotificationsService);
      Assert.IsNotNull(entryRelatedToService, "Entry cannot be unresloved");
      this.FillPlayerWithConfigAction(entryRelatedToService.RealPlayer);
    }

    #endregion

    #region Methods

    protected virtual void FillPlayerWithConfigAction(IConfigurationPlayer realPlayer)
    {
      realPlayer.AdditionalActions.Add(this.GetConfigurationAction());
    }

    protected virtual IConfigurationEntryAction GetConfigurationAction()
    {
      var configureIcon = HatcherGuide<IResourcesManager>.Instance.GetIconResource("configureV1", null);
      Assert.IsNotNull(configureIcon, "Resolved image cannot be null");
      var imageSource = ImageHelper.GetBitmapImageFromBitmapThreadSafe(configureIcon.ToBitmap(), ImageFormat.Png);
      return new SimpleConfigurationEntryAction(imageSource, this.ShowConfigurationWindow, true, null, this.Description);
    }

    protected virtual void ShowConfigurationWindow(object obj)
    {
      WindowStepState windowStepState = UNConfigurationStepPipeline.Run();
      WindowWithBackVM.GoAheadWithBackIfPossible(windowStepState);
    }

    #endregion
  }
}