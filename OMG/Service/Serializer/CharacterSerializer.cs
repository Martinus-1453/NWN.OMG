using System;
using System.IO;
using Newtonsoft.Json;
using NWN.API;
using NWN.API.Events;
using NWN.Services;
using OMG.Data;
using static OMG.Service.Log;

namespace OMG.Service
{
    [ServiceBinding(typeof(CharacterSerializer))]
    internal class CharacterSerializer : Serializer<CharacterEntity, NwPlayer>
    {
        public CharacterSerializer(NativeEventService nativeEventService)
        {
            nativeEventService.Subscribe<NwModule, ModuleEvents.OnClientEnter>(NwModule.Instance, OnClientEnter);
            nativeEventService.Subscribe<NwModule, ModuleEvents.OnClientLeave>(NwModule.Instance, OnClientLeave);
            foreach (var instanceArea in NwModule.Instance.Areas)
            {
                nativeEventService.Subscribe<NwArea, AreaEvents.OnEnter>(instanceArea, OnEnter);
            }
        }

        private void OnClientEnter(ModuleEvents.OnClientEnter onClientEnter)
        {
            // Add Character to collection with sanity check
            if (!Persistence.Characters.ContainsKey(onClientEnter.Player.CDKey))
            {
                Persistence.Characters.Add(onClientEnter.Player.CDKey,
                    Deserialize(onClientEnter.Player));
            }
        }

        private void OnClientLeave(ModuleEvents.OnClientLeave onClientLeave)
        {
            var clientKey = onClientLeave.Player.CDKey;
            var character = Persistence.Characters[clientKey];
            character.UpdateEntity();
            Serialize(character);
            Persistence.Characters.Remove(clientKey);
        }

        private void OnEnter(AreaEvents.OnEnter onEnter)
        {
            if (onEnter.EnteringObject is NwPlayer nwPlayer && Persistence.Characters.ContainsKey(nwPlayer.CDKey))
            {
                var character = Persistence.Characters[nwPlayer.CDKey];
                character.UpdateEntity();
                Serialize(character);
            }
        }

        public override void Serialize(CharacterEntity entity)
        {
            // Serialize characterEntity json
            // TODO: Handle exception if something goes wrong
            File.WriteAllText(GetFilePath(entity), JsonConvert.SerializeObject(entity));
        }

        public override CharacterEntity Deserialize(NwPlayer nwObject)
        {
            var filePath = GetFilePath(nwObject);

            // Check whether we have persistence file
            if (File.Exists(filePath))
            {
                CharacterEntity character;
                // Try if Deserialization goes well
                try
                {
                    // Deserialize character json
                    character = JsonConvert.DeserializeObject<CharacterEntity>(File.ReadAllText(filePath));
                }
                catch (Exception e)
                {
                    // Well f**k
                    // We have to kick player cause json failed to load properly
                    // TODO: Handle it better
                    nwObject.BootPlayer("Persistence JSON Load Error");
                    Logger.Error(e, $"Deserialization error for player {nwObject.PlayerName}:{nwObject.Name}");
                    return null;
                }


                // Check if player name checks out
                if (nwObject.PlayerName != character.PlayerName)
                {
                    nwObject.BootPlayer("Wrong player name");
                    Logger.Info(
                        $"{character.PlayerName} was kicked cause of incorrect player name associated with {character.Name}!");
                    return null;
                }

                // Check if player has a valid cdkey
                if (nwObject.CDKey != character.CDKey)
                {
                    nwObject.BootPlayer("Invalid CD-key");
                    Logger.Info(
                        $"{character.PlayerName} was kicked cause of incorrect cdkey associated with {character.Name}!");
                    return null;
                }

                // Assign and update NwObjectInstance corresponding to Entity
                character.NwObjectInstance = nwObject;
                character.UpdateNwObject();

                // Check if player was dead
                if (character.IsDead)
                {
                    //TODO: KILL CHARACTER OR SMTH
                }

                return character;
            }

            // Character is new => Make a new file for the characterEntity
            return Initialize(nwObject);
        }

        public override CharacterEntity Initialize(NwPlayer nwObject)
        {
            var newCharacter = new CharacterEntity(nwObject)
            {
                PlayerName = nwObject.PlayerName,
                Name = nwObject.Name,
                CDKey = nwObject.CDKey,
                HP = nwObject.HP,
                IsDead = nwObject.IsDead,
                PersistentLocation = new PersistentLocation(NwModule.Instance.StartingLocation)
            };

            // Serialize characterEntity json
            Serialize(newCharacter);

            return newCharacter;
        }

        protected override string GetFilePath(NwPlayer nwObject)
        {
            return $"{SerializerStrings.CharacterFolderPath}{nwObject.Name}{SerializerStrings.FileFormat}";
        }
    }
}