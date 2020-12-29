using System.Linq;
using System.Numerics;
using NLog;
using NWN.API;

namespace OMG.Data
{
    public class PersistentLocation
    {
        public string AreaResRef { get; }

        public Vector3 Position { get; }

        public float Orientation { get; }

        public PersistentLocation(Location location)
        {
            if (location != null)
            {
                Position = location.Position;
                AreaResRef = location.Area.ResRef;
                Orientation = location.Rotation;
            }
        }

        public static implicit operator Location(PersistentLocation persistentLocation)
        {
            return Location.Create(NwModule.Instance.Areas.First(area => area.ResRef == persistentLocation.AreaResRef),
                persistentLocation.Position, persistentLocation.Orientation);
        }
    }
}