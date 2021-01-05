using NWN.API;

namespace OMG.Data
{
    public abstract class Entity<T> where T : NwObject
    {
        protected Entity(T nwObjectInstance)
        {
            if (NwObjectInstance == null) return;
            NwObjectInstance = nwObjectInstance;
        }

        public abstract string ID { get; }
        public abstract string FileFolderPath { get; }
        public T NwObjectInstance { get; set; }

        public abstract void UpdateEntity();
        public abstract void UpdateNwObject();
    }
}