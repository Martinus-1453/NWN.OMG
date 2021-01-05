using NWN.API;
using OMG.Interface;

namespace OMG.Service.Chat
{
    class PlayerListCommand : IChatCommand
    {
        public string Command { get; } = "/playerlist";

        public void ExecuteCommand(NwPlayer sender, string arguments)
        {
            string playerList = string.Empty;
            foreach(var player in NwModule.Instance.Players)
            {
                playerList += $"{player.PlayerName}, ";
            }
            sender.SendServerMessage($"Online players: {playerList[(playerList.Length - 1)..]}");
        }
    }
}
