using NWN.Services;
using NWN.API.Events;
using OMG.Interface;
using System.Collections.Generic;
using NWN.API;
using System.Linq;

namespace OMG.Service.Chat
{
    [ServiceBinding(typeof(ChatHandler))]
    public class ChatHandler
    {
        private readonly List<IChatCommand> chatCommands;

        public ChatHandler(NativeEventService eventService, IEnumerable<IChatCommand> commands)
        {
            // Store all define chat commands.
            chatCommands = commands.ToList();

            // Subscribe to the OnPlayerChat module event
            eventService.Subscribe<NwModule, ModuleEvents.OnPlayerChat>(NwModule.Instance, OnChatMessage);
        }

        public string ProcessChatMessage(ModuleEvents.OnPlayerChat eventInfo)
        {
            var message = eventInfo.Message;

            // Check if message starts with out-of-character sequence "//"
            if (message.StartsWith("//"))
            {
                // Message is out-of-character
                // Make it orange
                message = StringExtensions.ColorString(message, new Color(255, 165, 0));
            }
            else
            {
                // Message is in-character
                var foundEmoteIndices = new List<int>();
                // Check if message contains emote symbol
                if (message.Contains('*'))
                {
                    // Find emote symbol '*' occurences
                    for (int i = 0; i < message.Length; i++)
                    {
                        if (message[i] == '*') foundEmoteIndices.Add(i);
                    }
                    // ^This definetely can be replaced with Regex.Split()
                    // ¯\_(ツ)_/¯
                }

                // Emote is always a pair of '*'
                // Discard odd last '*' by dividing Count by 2
                for (int i = 0; i < foundEmoteIndices.Count / 2; i++)
                {
                    var emote = message.Substring(foundEmoteIndices[i * 2], foundEmoteIndices[(i + 1) * 2] - foundEmoteIndices[i * 2]);
                    // Color emote to cyan
                    message = message.Replace(emote, StringExtensions.ColorString(emote, new Color(0, 116, 214)));
                }
            }
            return message;
        }

        public void OnChatMessage(ModuleEvents.OnPlayerChat eventInfo)
        {
            // Trim message to remove excess spaces at the begining and the end
            eventInfo.Message = eventInfo.Message.Trim();
            // Get the message from the event
            var message = eventInfo.Message.Split(' ');

            // Check if message has command syntax
            if (message.Length == 0) return;
            if (message[0].Length < 2) return;
            // Check if message is not a command and needs to be processed by ProcessChatMessage method
            if (!message[0].StartsWith('/') || message[0].StartsWith("//"))
            {
                // Disable Shout and Party for players
                if (eventInfo.Volume == NWN.API.Constants.TalkVolume.Shout
                     || eventInfo.Volume == NWN.API.Constants.TalkVolume.Party)
                {
                    if (!eventInfo.Sender.IsDM)
                    {
                        eventInfo.Message = null;
                        return;
                    }
                }
                // A good place to send chat message to discord or smth
                SendToWebhook(eventInfo);
                // Handle message further
                eventInfo.Message = ProcessChatMessage(eventInfo);
                return;
            }

            // It's a valid command at this point and we don't want player to send the message in the chat
            eventInfo.Message = null;

            // Find first message from created ones. Command must be at the begining of the event message. 
            // The rest of the message is probably arguments or empty
            var foundCommand = chatCommands.First(command => command.Command.ToLower().Equals(message[0].ToLower()));

            // This checks might look weird but it ensures to give access to DM commands only to DMs
            // Also checks whether command is not null you dummy :)
            if (foundCommand != null || eventInfo.Sender.IsDM || !foundCommand.IsDMOnly)
            {
                foundCommand.ExecuteCommand(eventInfo.Sender, message[1..]);
            }
            else
            {
                // (╯°□°）╯︵ ┻━┻
                eventInfo.Sender.SendServerMessage($"Command: {message[0]} could not be found");
            }
        }

        public void SendToWebhook(ModuleEvents.OnPlayerChat eventInfo)
        {
            // TODO: Implement webhook
        }
    }
}
