using System.Collections.Generic;
using System.Diagnostics;
using GEONIS.gear.AddIn;
using GEONIS.gear.AddIn.Binding;
using SampleMeasuringAddIn.Services;

namespace SampleMeasuringAddIn
{
    public class SampleMeasuringAddIn : DefaultAddInInformation
    {
        protected override IList<GBinding> GetCustomBindings()
        {
            // Allows easy debugging during the development process
#if DEBUG
            Debugger.Launch();
#endif
            // create and return bindings for interfaces and implementations you would like to manage using the GEONIS gear Kernel.
            // you don't need to add commands, maptools, services or widgets as the GEONIS gear Kernel infrastructure autodiscovers these.
            return new []
            {
                GBind.From<SampleMeasuringDrawingService>().ToSelf().InSingletonScope()   
            };
        }
    }
}