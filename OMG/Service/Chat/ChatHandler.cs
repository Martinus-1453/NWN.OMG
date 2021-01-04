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

        public void OnChatMessage(ModuleEvents.OnPlayerChat eventInfo)
        {
            // Get the message from the event.
            string message = eventInfo.Message;

            // Find first message from created ones. Command must be at the begining of the event message. 
            // The rest of the message is probably arguments or empty
            var foundCommand = chatCommands.First(command => command.Command.StartsWith(message));
            foundCommand?.ExecuteCommand(eventInfo.Sender, message[foundCommand.Command.Length..]);
        }
    }
}
