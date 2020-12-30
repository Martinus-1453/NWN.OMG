using NWN.API;
using NWN.API.Events;
using NWN.Services;

namespace OMG.Service
{
    // [ServiceBinding] indicates that this class will be created during server startup.
    [ServiceBinding(typeof(ModuleHello))]
    public class ModuleHello
    {
        // Called at startup. NWN.Managed resolves EventService for us.
        public ModuleHello(NativeEventService nativeEventService)
        {
            // Subscribe to the OnClientEnter event, and call our OnClientEnter function when someone connects.
            nativeEventService.Subscribe<NwModule, ModuleEvents.OnClientEnter>(NwModule.Instance, OnClientEnter);
        }

        private void OnClientEnter(ModuleEvents.OnClientEnter onClientEnter)
        {
            // Send a welcome message to the player who just connected
            onClientEnter.Player.SendServerMessage($"Hello {onClientEnter.Player.Name}! Welcome to the server!",
                Color.PINK);
        }
    }
}