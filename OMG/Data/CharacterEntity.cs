using NWN.API;
using OMG.Service;

namespace OMG.Data
{
    public class CharacterEntity : Entity<NwPlayer>
    {
        public CharacterEntity(NwPlayer nwObjectInstance) : base(nwObjectInstance)
        {
        }

        public string PlayerName { get; set; }
        public string Name { get; set; }
        public string CDKey { get; set; }
        public int HP { get; set; }
        public bool IsDead { get; set; }
        public PersistentLocation PersistentLocation { get; set; }

        public override string ID => Name;

        public override string FileFolderPath { get; } =
            SerializerPaths.CharacterFolderPath;

        public override void UpdateEntity()
        {
            if (NwObjectInstance == null)
            {
                return;
            }

            HP = NwObjectInstance.HP;
            PersistentLocation = new PersistentLocation(NwObjectInstance.Location);
        }

        public override void UpdateNwObject()
        {
            if (NwObjectInstance == null)
            {
                return;
            }

            NwObjectInstance.HP = HP;
            if (PersistentLocation != null)
            {
                NwObjectInstance.Location = PersistentLocation;
            }
        }
    }
}