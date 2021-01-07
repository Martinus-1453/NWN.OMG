using NWN.API;
using NWN.Services;
using OMG.Interface;

namespace OMG.Service.Chat
{
    [ServiceBinding(typeof(IChatCommand))]
    internal class PlayerListCommand : IChatCommand
    {
        public string Command { get; } = "/playerlist";
        public bool IsDMOnly { get; } = false;

        public void ExecuteCommand(NwPlayer sender, string[] arguments)
        {
            var playerList = string.Empty;
            foreach (var player in NwModule.Instance.Players) playerList += $"{player.PlayerName}, ";
            sender.SendServerMessage($"Online players: {playerList[^1..]}", Color.ROSE);
        }
    }
}