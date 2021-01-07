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
            if (arguments.Length == 0) return;

            if (int.TryParse(arguments[0], out var amount))
            {
                cursorTargetService.EnterTargetMode(sender, selected =>
                {
                    if(selected.TargetObj is NwPlayer nwPlayer)
                    {
                        nwPlayer.GiveXp(amount);
                        sender.SendServerMessage($"You've given {nwPlayer.Name} {amount} XP", Color.GREEN);
                        nwPlayer.SendServerMessage($"You've been given {amount} XP by {sender.Name}", Color.GREEN);
                    }
                    else
                    {
                        sender.SendServerMessage($"Invalid object selected", Color.RED);
                    }
                }, NWN.API.Constants.ObjectTypes.Creature);
            }
            else
            {
                sender.SendServerMessage($"Invalid argument", Color.RED);
            }
        }
    }
}
