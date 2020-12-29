using NWN.API;

namespace OMG.Data
{
    public class Character
    {
        public string PlayerName { get; set; }
        public string Name { get; set; }
        public string CDKey { get; set; }
        public int HP { get; set; }
        public bool IsDead { get; set; }
        public PersistentLocation PersistentLocation { get; set; }

        public void UpdateCharacter(NwPlayer nwPlayer)
        {
            HP = nwPlayer.HP;
            PersistentLocation = new PersistentLocation(nwPlayer.Location);
        }

        public void UpdateNwPlayer(NwPlayer nwPlayer)
        {
            nwPlayer.HP = HP;
            nwPlayer.Location = PersistentLocation;
        }

    }
}
