﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.UI.Configuration.EntryVM.Players;

#endregion

namespace TrayGarden.Services.Engine.UI.Intergration
{
  public class ConfigurationPlayerService : TypedConfigurationPlayer<bool>
  {
    #region Constructors and Destructors

    public ConfigurationPlayerService(IService serviceToManage)
      : base(serviceToManage.ServiceName, serviceToManage.CanBeDisabled, !serviceToManage.CanBeDisabled)
    {
      this.InfoSource = serviceToManage;
      this.InfoSource.IsEnabledChanged += x => this.OnValueChanged();
    }

    #endregion

    #region Public Properties

    public IService InfoSource { get; set; }

    public override bool RequiresApplicationReboot
    {
      get
      {
        return this.InfoSource.IsEnabled != this.InfoSource.IsActuallyEnabled;
      }
    }

    public override string SettingDescription
    {
      get
      {
        return this.InfoSource.ServiceDescription;
      }
      protected set
      {
        //Description should be changed. 
      }
    }

    public override bool Value
    {
      get
      {
        return this.InfoSource.IsEnabled;
      }
      set
      {
        this.InfoSource.IsEnabled = value;
        this.OnRequiresApplicationRebootChanged();
      }
    }

    #endregion

    #region Public Methods and Operators

    public override void Reset()
    {
      this.Value = this.InfoSource.IsActuallyEnabled;
      this.OnValueChanged();
    }

    #endregion
  }
}