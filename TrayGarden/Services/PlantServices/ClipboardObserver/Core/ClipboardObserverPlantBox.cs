﻿using System.Threading.Tasks;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.ClipboardObserver.Smorgasbord;

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
    public class ClipboardObserverPlantBox:ServicePlantBoxBase
    {
        public IAskClipboardEvents EventsHungry { get; set; }
        
        public virtual void InformNewClipboardValue(string newClipboardValue)
        {
            if(IsEnabled)
                Task.Factory.StartNew(() => EventsHungry.OnClipboardTextChanged(newClipboardValue));
        }
    }
}
