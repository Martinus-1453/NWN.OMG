using System.Linq;
using System.Numerics;
using NWN.API;

namespace OMG.Data
{
    public class PersistentLocation
    {
        public string AreaResRef { get; set; }

        public Vector3 Position { get; set; }

        public float Orientation { get; set; }

        public PersistentLocation(Location location)
        {
            Position = location.Position;
            AreaResRef = location.Area.ResRef;
            Orientation = location.Rotation;
        }

        public static implicit operator Location(PersistentLocation persistentLocation)
        {
            return Location.Create(NwModule.Instance.Areas.First(area => area.ResRef == persistentLocation.AreaResRef),
                persistentLocation.Position, persistentLocation.Orientation);
        }
    }
}