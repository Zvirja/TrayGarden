﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Plants;
using TrayGarden.Services.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline
{
  [UsedImplicitly]
  public abstract class ServicePresenterBase<TServiceType>
    where TServiceType : IService
  {
    #region Constructors and Destructors

    public ServicePresenterBase()
    {
      this.ServiceName = typeof(TServiceType).Name;
      this.ServiceDescription = "<this service doesn't provide description>";
    }

    #endregion

    #region Public Properties

    public string ServiceDescription { get; set; }

    public string ServiceName { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(ResolveSinglePlantVMPipelineArgs args)
    {
      var serviceInstance =
        (TServiceType)(HatcherGuide<IServicesSteward>.Instance.Services.FirstOrDefault(x => x.GetType() == typeof(TServiceType)));
      if (serviceInstance == null)
      {
        Log.Warn("Service of type '{0}' wasn't found".FormatWith(typeof(TServiceType)), this);
        return;
      }
      if (!serviceInstance.IsActuallyEnabled)
      {
        return;
      }
      bool isAvailableForPlant = serviceInstance.IsAvailableForPlant(args.PlantEx);
      if (!isAvailableForPlant)
      {
        return;
      }
      ServiceForPlantVMBase serviceForPlantVMBase = this.GetServiceVM(serviceInstance, args.PlantEx);
      if (serviceForPlantVMBase != null)
      {
        args.PlantVM.ServicesVM.Add(serviceForPlantVMBase);
      }
    }

    #endregion

    #region Methods

    protected abstract ServiceForPlantVMBase GetServiceVM(TServiceType serviceInstance, IPlantEx plantEx);

    #endregion
  }
}