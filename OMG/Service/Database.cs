using System.IO;

namespace OMG.Service
{
    internal static class DatabaseStrings
    {
        internal static readonly string FolderPath = "db_omg";
        internal static readonly string CharacterFolderPath = $"{FolderPath}\\players";
        internal static readonly string FileFormat = ".json";
    }

    public static class Database
    {
        public static void CreateFolders()
        {
            // Create directory if not existent
            if (!Directory.Exists(DatabaseStrings.FolderPath))
            {
                Directory.CreateDirectory(DatabaseStrings.FolderPath);

                if (!Directory.Exists(DatabaseStrings.CharacterFolderPath))
                    Directory.CreateDirectory(DatabaseStrings.CharacterFolderPath);
            }
        }
    }
}