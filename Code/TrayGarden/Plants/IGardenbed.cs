using System.Collections.Generic;
using JetBrains.Annotations;

namespace TrayGarden.Plants
{
  public interface IGardenbed
  {
    bool AutoDetectPlants { get; set; }
    void Initialize(List<object> plants);
    void InformPostInitStage();
    List<IPlantEx> GetAllPlants();
    List<IPlantEx> GetEnabledPlants();
  }
}