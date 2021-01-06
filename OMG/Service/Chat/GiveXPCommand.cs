using NWN.API;
using NWN.Services;
using OMG.Interface;

namespace OMG.Service.Chat
{
    [ServiceBinding(typeof(IChatCommand))]
    class GiveXPCommand : IChatCommand
    {
        public string Command { get; } = "/givexp";
        public bool IsDMOnly { get; } = true;

        private readonly CursorTargetService cursorTargetService;

        public GiveXPCommand(CursorTargetService cursorTargetService)
        {
            this.cursorTargetService = cursorTargetService;
        }

        public void ExecuteCommand(NwPlayer sender, string[] arguments)
        {
            if (!sender.IsDM && IsDMOnly) return;
            if (int.TryParse(arguments[0], out var amount))
            {
                cursorTargetService.EnterTargetMode(sender, selected =>
                {
                    if(selected.TargetObj is NwPlayer nwPlayer)
                    {
                        nwPlayer.GiveXp(amount);
                    }
                }, NWN.API.Constants.ObjectTypes.Creature);
            }
        }
    }
}
