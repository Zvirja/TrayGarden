﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Plants;
using TrayGarden.Helpers;
using TrayGarden.Reception.Services;

namespace TrayGarden.Services.PlantServices.MyAdminConfig.Core
{
  [UsedImplicitly]
  public class MyAdminConfigService : PlantServiceBase<MyAdminConfigServicePlantBox>
  {
    public MyAdminConfigService()
      : base("My Admin Config", "MyAdminConfigService")
    {
      ServiceDescription = "Provide plants with configuration manager for their admin configurations (e.g. <moduleName>.dll.config)";
    }

    protected virtual void ProvidePlantWithConfig(IPlantEx plantEx)
    {
      var asExpected = plantEx.GetFirstWorkhorseOfType<IGiveMeMyAppConfig>();
      if (asExpected == null)
        return;
      string assemblyLocation = plantEx.Plant.GetType().Assembly.Location;
      System.Configuration.Configuration assemblyConfiguration = null;
      try
      {
        assemblyConfiguration = ConfigurationManager.OpenExeConfiguration(assemblyLocation);
      }
      catch (Exception ex)
      {
        Log.Warn("Unable to open admin config for {0}".FormatWith(assemblyLocation), this, ex);
      }
      if (assemblyConfiguration != null)
      {
        asExpected.StoreModuleConfiguration(assemblyConfiguration);
        plantEx.PutLuggage(LuggageName, new MyAdminConfigServicePlantBox());
      }
    }


    public override void InitializePlant(IPlantEx plantEx)
    {
      ProvidePlantWithConfig(plantEx);
    }

  }
}
