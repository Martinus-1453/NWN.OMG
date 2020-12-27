using System;
using System.IO;
using Newtonsoft.Json;
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
            nativeEventService.Subscribe<NwModule, ModuleEvents.OnClientEnter>(NwModule.Instance, OnClientEnter);
            nativeEventService.Subscribe<NwModule,ModuleEvents.OnClientLeave>(NwModule.Instance, OnClientLeave);
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

        private void SaveCharacter(Character character)
        {
            // Create directory if not existent
            if (!Directory.Exists(DatabaseStrings.DatabaseFolderPath))
            {
                Directory.CreateDirectory(DatabaseStrings.DatabaseFolderPath);

                if (!Directory.Exists(DatabaseStrings.DatabaseCharacterFolderPath))
                {
                    Directory.CreateDirectory(DatabaseStrings.DatabaseCharacterFolderPath);
                }
            }
            // Save character json
            File.WriteAllText(GetCharacterFilePath(character), JsonConvert.SerializeObject(character));
        }

        private Character LoadCharacter(NwPlayer nwPlayer)
        {
            string filePath = GetCharacterFilePath(nwPlayer);

            if (!File.Exists(filePath))
            {
                var newCharacter = new Character
                {
                    PlayerName = nwPlayer.PlayerName,
                    Name = nwPlayer.Name,
                    CDKey = nwPlayer.CDKey,
                    HealthPoints = nwPlayer.HP,
                    IsDead = nwPlayer.IsDead,
                    PersistentLocation = new PersistentLocation(NwModule.Instance.StartingLocation)
                };

                SaveCharacter(newCharacter);

                return newCharacter;
            }

            var character = JsonConvert.DeserializeObject<Character>(File.ReadAllText(filePath));

            if (character != null)
            {
                character.UpdateCharacter(nwPlayer);

                // CHECK PLAYER NAME HERE
                if (nwPlayer.PlayerName != character.PlayerName || nwPlayer.CDKey != character.CDKey)
                {
                    //KICK PLAYER
                }

                //CHECK IF DEAD
                if (character.IsDead)
                {
                    //KILL CHARACTER OR SMTH
                }

                return character;
            }

            return null;
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
