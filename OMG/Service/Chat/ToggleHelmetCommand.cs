using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWN.API;
using NWN.API.Constants;
using OMG.Interface;

namespace OMG.Service.Chat
{
    class ToggleHelmetCommand : IChatCommand
    {
        public string Command { get; } = "/helmet";
        public bool IsDMOnly { get; } = false;
        public void ExecuteCommand(NwPlayer sender, string[] arguments)
        {
            var helm = sender.GetItemInSlot(InventorySlot.Head);
            if(helm.IsValid)
            {
                helm.HiddenWhenEquipped = (helm.HiddenWhenEquipped + 1) % 2;
            }
        }
    }
}
