﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

#endregion

namespace ClipboardChangerPlant.UIConfiguration
{
  public class UIDialogConfirmator
  {
    #region Constructors and Destructors

    public UIDialogConfirmator(string confirmationSettingName, Func<IResultProvider> uiDialogConstructor)
    {
      this.UIDialogConstructor = uiDialogConstructor;
      //Moved to constructor because believe that it will be created before the initialization happen
      this.ConfirmationSetting = this.BuildConfirmationSetting(confirmationSettingName, confirmationSettingName);
    }

    #endregion

    #region Public Properties

    public ILordOfNotifications LordOfNotifications
    {
      get
      {
        return PopupDialogsManager.ActualManager.LordOfNotifications;
      }
    }

    #endregion

    #region Properties

    protected IBoolUserSetting ConfirmationSetting { get; set; }

    protected Func<IResultProvider> UIDialogConstructor { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual bool? ConfirmThroughUI()
    {
      if (this.ConfirmationSetting == null)
      {
        return null;
      }
      if (!this.ConfirmationSetting.Value)
      {
        return null;
      }
      return this.GetConfirmationFromUIDialog();
    }

    #endregion

    #region Methods

    protected virtual IBoolUserSetting BuildConfirmationSetting(string name, string title)
    {
      return UIConfigurationManager.ActualManager.SettingsSteward.DeclareBoolSetting(name, title, true);
    }

    protected virtual bool? GetConfirmationFromUIDialog()
    {
      IResultProvider dialog = this.UIDialogConstructor();
      if (dialog == null)
      {
        return null;
      }
      INotificationResultCourier resultCourier = this.LordOfNotifications.DisplayNotification(dialog);
      NotificationResult result = resultCourier.GetResultWithWait();
      if (result.Code == ResultCode.OK || result.Code == ResultCode.Yes)
      {
        return true;
      }
      if (result.Code == ResultCode.Close || result.Code == ResultCode.No)
      {
        return false;
      }
      if (result.Code == ResultCode.PermanentlyClose)
      {
        this.ConfirmationSetting.Value = false;
      }
      return null;
    }

    #endregion
  }
}