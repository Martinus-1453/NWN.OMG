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

        private void OnModuleLoad(ModuleEvents.OnModuleLoad onModuleLoad)
        {
            // TODO: Fix it somehow to prevent server crash
            DiscordHooks.StatusHook.SendMessage("Server is up and running!");
        }
    }
}