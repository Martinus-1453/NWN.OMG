using System.Collections.Generic;

namespace OMG.Data
{
    public static class Persistence
    {
        public static Dictionary<string, CharacterEntity> Characters { get; } = new Dictionary<string, CharacterEntity>();
    }
}