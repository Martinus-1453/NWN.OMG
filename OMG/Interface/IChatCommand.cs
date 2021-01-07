using NWN.API;

namespace OMG.Interface
{
    public interface IChatCommand
    {
        public string Command { get; }
        public bool IsDMOnly { get; }
        void ExecuteCommand(NwPlayer sender, string[] arguments);
    }
}