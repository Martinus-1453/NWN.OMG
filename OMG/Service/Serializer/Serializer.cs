using System;
using System.IO;
using NWN.API;
using OMG.Data;

namespace OMG.Service
{
    public abstract class Serializer<TEntity, TNwObject>
        where TEntity : Entity<TNwObject>
        where TNwObject : NwObject
    {
        protected Serializer()
        {
            // Create directories if not existent
            Directory.CreateDirectory(SerializerStrings.FolderPath);
            Directory.CreateDirectory(SerializerStrings.CharacterFolderPath);
            Directory.CreateDirectory(SerializerStrings.PlaceableFolderPath);
        }

        public abstract void Serialize(TEntity entity);
        public abstract TEntity Deserialize(TNwObject nwObject);
        public abstract TEntity Initialize(TNwObject nwObject);

        public virtual string GetFilePath()
        {
            throw new NotImplementedException();
        }

        protected virtual string GetFilePath(TNwObject nwObject)
        {
            throw new NotImplementedException();
        }

        protected virtual string GetFilePath(TEntity entity)
        {
            return $"{entity.FileFolderPath}\\{entity.ID}{SerializerStrings.FileFormat}";
        }
    }
}