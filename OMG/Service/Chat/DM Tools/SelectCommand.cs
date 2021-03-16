using System.Collections.Generic;
using NWN.API;
using NWN.API.Constants;
using NWN.Services;
using OMG.Interface;
using OMG.Util;

namespace OMG.Service.Chat.DM_Tools
{
    [ServiceBinding(typeof(IChatCommand))]
    public class SelectCommand : IChatCommand
    {
        private readonly CursorTargetService cursorTargetService;
        public static Dictionary<string, List<NwGameObject>> SelectionDictionary { get; } =
            new Dictionary<string, List<NwGameObject>>();

        public SelectCommand(CursorTargetService cursorTargetService)
        {
            this.cursorTargetService = cursorTargetService;
        }
        public string Command { get; } = "/select";
        public bool IsDMOnly { get; } = true;
        public void ExecuteCommand(NwPlayer sender, string[] arguments)
        {
            SelectionDictionary.TryAdd(sender.CDKey, new List<NwGameObject>());

            cursorTargetService.EnterTargetMode(sender, selected =>
            {
                if (selected.TargetObj is null || selected.TargetObj is NwPlayer ||
                    !(selected.TargetObj is NwCreature nwCreature))
                {
                    return;
                }

                SelectionDictionary.TryGetValue(sender.CDKey, out var selectionList);

                if (selectionList == null)
                {
                    return;
                }

                foreach (var nwGameObject in selectionList)
                {
                    if (nwGameObject.UUID == nwCreature.UUID)
                    {
                        return;
                    }
                }

                selectionList.Add(nwCreature);
            }, ObjectTypes.Creature);
        }
    }
}