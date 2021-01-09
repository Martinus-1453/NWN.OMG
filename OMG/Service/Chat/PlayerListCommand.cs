using System.Text;
using NWN.API;
using NWN.Services;
using OMG.Interface;
using OMG.Util;

namespace OMG.Service.Chat
{
    [ServiceBinding(typeof(IChatCommand))]
    internal class PlayerListCommand : IChatCommand
    {
        public string Command { get; } = "/playerlist";
        public bool IsDMOnly { get; } = false;

        public void ExecuteCommand(NwPlayer sender, string[] arguments)
        {
            var playerList = new StringBuilder();
            foreach (var player in NwModule.Instance.Players)
            {
                playerList.Append($"{player.PlayerName}, ");
            }

            sender.SendServerMessage($"Online players: {playerList.ToString()[..^2]}", Colors.Salmon);
        }
    }
}