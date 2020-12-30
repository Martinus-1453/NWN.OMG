using System.Linq;
using System.Numerics;
using NWN.API;
using static OMG.Service.LogService;

namespace OMG.Data
{
    public class PersistentLocation
    {
        public PersistentLocation(Location location)
        {
            if (location == null)
            {
                Logger.Warn($"PersistentLocation: ctor of {this} is null");
                return;
            }

            Position = location.Position;
            AreaResRef = location.Area.ResRef;
            Orientation = location.Rotation;
        }

        public string AreaResRef { get; }
        public Vector3 Position { get; }
        public float Orientation { get; }

        public static implicit operator Location(PersistentLocation persistentLocation)
        {
            return Location.Create(NwModule.Instance.Areas.First(area => area.ResRef == persistentLocation.AreaResRef),
                persistentLocation.Position, persistentLocation.Orientation);
        }
    }
}