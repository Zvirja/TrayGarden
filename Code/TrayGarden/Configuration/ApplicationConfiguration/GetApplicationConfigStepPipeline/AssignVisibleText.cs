﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline
{
  [UsedImplicitly]
  public class AssignVisibleText
  {
    #region Constructors and Destructors

    public AssignVisibleText()
    {
      this.GlobalTitle = "Tray Garden -- Application configuration";
      this.Header = "Application configuration";
      this.ShortName = "app config";
      this.ConfigurationDescription = "Here you may configure global application settings. Some settings may require reboot to be applied";
    }

    #endregion

    #region Public Properties

    public string ConfigurationDescription { get; set; }

    public string GlobalTitle { get; set; }

    public string Header { get; set; }

    public string ShortName { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(GetApplicationConfigStepArgs args)
    {
      args.ConfigurationConstructInfo.ConfigurationDescription = this.ConfigurationDescription;
      var stepInfo = args.StepConstructInfo;
      stepInfo.GlobalTitle = this.GlobalTitle;
      stepInfo.Header = this.Header;
      stepInfo.ShortName = this.ShortName;
    }

    #endregion
  }
}