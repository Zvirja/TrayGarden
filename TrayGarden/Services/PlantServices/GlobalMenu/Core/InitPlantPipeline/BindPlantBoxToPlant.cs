﻿using JetBrains.Annotations;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class BindPlantBoxToPlant
    {
        [UsedImplicitly]
        public virtual void Process(InitPlantGMArgs args)
        {
            GlobalMenuPlantBox globalMenuPlantBox = args.GMBox;
            globalMenuPlantBox.RelatedPlant = args.Plant;
            globalMenuPlantBox.RelatedPlant.PutLuggage(args.LuggageName,globalMenuPlantBox);
        }
    }
}
