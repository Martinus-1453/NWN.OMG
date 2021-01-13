using NWN.API;
using NWN.API.Events;
using NWN.Services;
using OMG.Util;

namespace OMG.Service.Discord
{
    [ServiceBinding(typeof(ServerStatus))]
    public class ServerStatus
    {
        public ServerStatus(NativeEventService nativeEventService)
        {
            nativeEventService.Subscribe<NwModule, ModuleEvents.OnModuleLoad>(NwModule.Instance, OnModuleLoad);
        }

        private async void OnModuleLoad(ModuleEvents.OnModuleLoad onModuleLoad)
        {
            await DiscordHooks.Status();
            await NwTask.SwitchToMainThread();
        }
    }
}