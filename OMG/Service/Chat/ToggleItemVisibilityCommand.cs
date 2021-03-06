﻿using NWN.API;
using NWN.API.Constants;
using NWN.Services;
using OMG.Interface;
using OMG.Util;

namespace OMG.Service.Chat
{
    [ServiceBinding(typeof(IChatCommand))]
    internal class ToggleItemVisibilityCommand : IChatCommand
    {
        public string Command { get; } = "/toggle";
        public bool IsDMOnly { get; } = false;

        public void ExecuteCommand(NwPlayer sender, string[] arguments)
        {
            if (arguments.Length < 1)
            {
                return;
            }

            switch (arguments[0].ToLower())
            {
                case "helm":
                case "helmet":
                {
                    ToggleItemVisibility(sender.GetItemInSlot(InventorySlot.Head));
                    break;
                }
                // Hide cloak
                case "cloak":
                case "cape":
                {
                    ToggleItemVisibility(sender.GetItemInSlot(InventorySlot.Cloak));
                    break;
                }
                default:
                {
                    sender.SendServerMessage("Command needs an argument", Colors.Red);
                    break;
                }
            }
        }

        private void ToggleItemVisibility(NwItem nwItem)
        {
            if (nwItem is not null && nwItem.IsValid)
            {
                nwItem.HiddenWhenEquipped = ++nwItem.HiddenWhenEquipped % 2;
            }
        }
    }
}