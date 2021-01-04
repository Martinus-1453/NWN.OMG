using NWN.API;

namespace OMG.Interface
{
    public interface IChatCommand
    {
        public string Command { get; }
        void ExecuteCommand(NwPlayer sender, string arguments);
    }
}
