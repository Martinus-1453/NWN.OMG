using System.Collections.Generic;
using System.Linq;
using NWN.API;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Services;
using OMG.Interface;

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

        public void OnChatMessage(ModuleEvents.OnPlayerChat eventInfo)
        {
            // Trim message to remove excess spaces at the beginning and the end
            eventInfo.Message = eventInfo.Message.Trim();
            // Get the message from the event
            var message = eventInfo.Message.Split(' ');

            // Check if message has command syntax: at least two characters
            if (message.Length == 0 || message[0].Length < 2)
            {
                return;
            }

            // Check if message is not a command and needs to be processed by ProcessChatMessage method
            if (!message[0].StartsWith('/') || message[0].StartsWith("//"))
            {
                // Disable Shout and Party for players
                if (eventInfo.Volume == TalkVolume.Shout
                    || eventInfo.Volume == TalkVolume.Party)
                {
                    if (!eventInfo.Sender.IsDM)
                    {
                        eventInfo.Message = null;
                        return;
                    }
                }

                // A good place to send chat message to discord or smth
                SendToWebHook(eventInfo);
                // Handle message further
                eventInfo.Message = ChatProcessor.ProcessChatMessage(eventInfo.Message);
                return;
            }

            // It's a valid syntax-wise command at this point and we don't want player to send the message in the chat
            eventInfo.Message = null;

            // Find first message from created ones. Command must be at the beginning of the event message. 
            // The rest of the message is probably arguments or empty
            var foundCommand = chatCommands.FirstOrDefault(command => command.Command.ToLower().Equals(message[0].ToLower()));

            // This checks might look weird but it ensures to give access to DM commands only to DMs
            // Also checks whether command is not null you dummy :)
            if (foundCommand != null && (eventInfo.Sender.IsDM || !foundCommand.IsDMOnly))
            {
                foundCommand.ExecuteCommand(eventInfo.Sender, message[1..]);
            }
            else if (message[0].ToLower() == "/help")
            {
                //TODO: Handle help command here
            }
            else
            {
                // (╯°□°）╯︵ ┻━┻
                eventInfo.Sender.SendServerMessage($"Command: {message[0]} could not be found. Type /help for command list");
            }
        }

        public void SendToWebHook(ModuleEvents.OnPlayerChat eventInfo)
        {
            // TODO: Implement webhook
        }
    }
}