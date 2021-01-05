using NWN.API;
using OMG.Data;

namespace OMG.Service
{
    public abstract class Serializer<TEntity, TNwObject>
        where TEntity : Entity<TNwObject>
        where TNwObject : NwObject
    {
        protected Serializer() => Database.CreateFolders();
        public abstract void Serialize(TEntity entity);
        public abstract TEntity Deserialize(TNwObject nwObject);
        public abstract TEntity Initialize(TNwObject nwObject);
        public virtual string GetFilePath() => throw new System.NotImplementedException();
        protected virtual string GetFilePath(TNwObject nwObject) => throw new System.NotImplementedException();
        protected virtual string GetFilePath(TEntity entity) => $"{entity.FileFolderPath}\\{entity.ID}{DatabaseStrings.FileFormat}";
    }
}
