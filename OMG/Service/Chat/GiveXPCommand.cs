using NWN.API;
using NWN.API.Constants;
using NWN.Services;
using OMG.Interface;
using OMG.Util;

namespace OMG.Service.Chat
{
    [ServiceBinding(typeof(IChatCommand))]
    internal class GiveXPCommand : IChatCommand
    {
        private readonly CursorTargetService cursorTargetService;

        public GiveXPCommand(CursorTargetService cursorTargetService)
        {
            this.cursorTargetService = cursorTargetService;
        }

        public string Command { get; } = "/givexp";
        public bool IsDMOnly { get; } = true;

        public void ExecuteCommand(NwPlayer sender, string[] arguments)
        {
            if (arguments.Length > 0 && int.TryParse(arguments[0], out var amount))
            {
                cursorTargetService.EnterTargetMode(sender, selected =>
                {
                    if (selected.TargetObj is not null && selected.TargetObj is NwPlayer nwPlayer)
                    {
                        nwPlayer.GiveXp(amount);

                        // TODO: FIX THIS CAUSE THIS CHECK DOES NOT WORK
                        if (nwPlayer.UUID == sender.UUID)
                        {
                            sender.SendServerMessage($"You've given {nwPlayer.Name} {amount} XP", Colors.Green);
                        }

                        nwPlayer.SendServerMessage($"You've been given {amount} XP by {sender.Name}", Colors.Green);
                    }
                    else
                    {
                        sender.SendServerMessage("Invalid object selected", Colors.Red);
                    }
                }, ObjectTypes.Creature);
            }
            else
            {
                sender.SendServerMessage("Invalid argument", Colors.Red);
            }
        }
    }
}