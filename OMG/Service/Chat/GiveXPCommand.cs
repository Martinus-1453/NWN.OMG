using NWN.API;
using NWN.Services;
using OMG.Interface;
using System;

namespace OMG.Service.Chat
{
    [ServiceBinding(typeof(IChatCommand))]
    class GiveXPCommand : IChatCommand
    {
        public string Command { get; } = "/givexp";
        private readonly CursorTargetService cursorTargetService;

        public GiveXPCommand(CursorTargetService cursorTargetService)
        {
            this.cursorTargetService = cursorTargetService;
        }

        public void ExecuteCommand(NwPlayer sender, string arguments)
        {
            if (Int32.TryParse(arguments[..1], out int amount))
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
