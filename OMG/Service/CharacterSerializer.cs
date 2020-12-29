using System.IO;
using Newtonsoft.Json;
using NLog;
using NWN.API;
using NWN.API.Events;
using NWN.Services;
using OMG.Data;

namespace OMG.Service
{
    [ServiceBinding(typeof(CharacterSerializer))]
    class CharacterSerializer
    {
        public CharacterSerializer(NativeEventService nativeEventService)
        {
            Database.CreateFolders();
            nativeEventService.Subscribe<NwModule, ModuleEvents.OnClientEnter>(NwModule.Instance, OnClientEnter);
            nativeEventService.Subscribe<NwModule, ModuleEvents.OnClientLeave>(NwModule.Instance, OnClientLeave);
            foreach (var instanceArea in NwModule.Instance.Areas)
            {
                nativeEventService.Subscribe<NwArea, AreaEvents.OnEnter>(instanceArea, OnEnter);
            }
        }

        private void OnClientEnter(ModuleEvents.OnClientEnter onClientEnter)
        {
            if (!Persistence.Characters.ContainsKey(onClientEnter.Player.CDKey))
            {
                Persistence.Characters.Add(onClientEnter.Player.CDKey,
                    LoadCharacter(onClientEnter.Player));
            }
        }

        private void OnClientLeave(ModuleEvents.OnClientLeave onClientLeave)
        {
            var character = Persistence.Characters[onClientLeave.Player.CDKey];
            character.UpdateCharacter(onClientLeave.Player);
            SaveCharacter(character);
            Persistence.Characters.Remove(onClientLeave.Player.CDKey);
        }

        private void OnEnter(AreaEvents.OnEnter onEnter)
        {
            if (onEnter.EnteringObject is NwPlayer nwPlayer)
            {
                var character = Persistence.Characters[nwPlayer.CDKey];
                character.UpdateCharacter(nwPlayer);
                SaveCharacter(character);
            }
        }

        private void SaveCharacter(Character character)
        {
            // Save character json
            File.WriteAllText(GetCharacterFilePath(character), JsonConvert.SerializeObject(character));
        }

        private Character LoadCharacter(NwPlayer nwPlayer)
        {
            string filePath = GetCharacterFilePath(nwPlayer);

            if (!File.Exists(filePath))
            {
                // Make a new file for a new character
                var newCharacter = new Character
                {
                    PlayerName = nwPlayer.PlayerName,
                    Name = nwPlayer.Name,
                    CDKey = nwPlayer.CDKey,
                    HP = nwPlayer.HP,
                    IsDead = nwPlayer.IsDead,
                    PersistentLocation = new PersistentLocation(NwModule.Instance.StartingLocation)
                };
                // Save character .json
                SaveCharacter(newCharacter);

                return newCharacter;
            }

            // Deserialize character .json
            var character = JsonConvert.DeserializeObject<Character>(File.ReadAllText(filePath));

            if (character == null) return null;

            LogManager.GetCurrentClassLogger().Info($"{character.PersistentLocation.Position} {character.PersistentLocation.AreaResRef} {character.PersistentLocation.Orientation}");
            character.UpdateNwPlayer(nwPlayer);

            // Check if player name checks out
            if (nwPlayer.PlayerName != character.PlayerName)
            {
                // kick player
                nwPlayer.BootPlayer("Wrong player name");
            } // Check if player has a valid cdkey
            else if (nwPlayer.CDKey != character.CDKey)
            {
                // kick player
                nwPlayer.BootPlayer("Invalid CD-key");
            }
            // Check if player was dead
            if (character.IsDead)
            {
                //TODO: KILL CHARACTER OR SMTH
            }

            return character;
        }

        private string GetCharacterFilePath(NwPlayer nwPlayer)
        {
            return $"{DatabaseStrings.DatabaseCharacterFolderPath}\\{nwPlayer.Name}{DatabaseStrings.DatabaseFileFormat}";
        }

        private string GetCharacterFilePath(Character character)
        {
            return $"{DatabaseStrings.DatabaseCharacterFolderPath}\\{character.Name}{DatabaseStrings.DatabaseFileFormat}";
        }
    }
}
