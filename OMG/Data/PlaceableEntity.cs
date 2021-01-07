using System;
using NWN.API;

namespace OMG.Data
{
    public class PlaceableEntity : Entity<NwPlaceable>
    {
        public PlaceableEntity(NwPlaceable nwObjectInstance) : base(nwObjectInstance)
        {
        }

        public string Tag { get; set; }
        public string AreaResRef { get; set; }
        public string AreaTag { get; set; }

        public override string ID { get; }
        public override string FileFolderPath { get; }

        public override void UpdateEntity()
        {
            throw new NotImplementedException();
        }

        public override void UpdateNwObject()
        {
            throw new NotImplementedException();
        }
    }
}