using System.IO;

namespace OMG.Service
{
    internal static class DatabaseStrings
    {
        internal static readonly string DatabaseFolderPath = "db_omg";
        internal static readonly string DatabaseCharacterFolderPath = $"{DatabaseFolderPath}\\players";
        internal static readonly string DatabaseFileFormat = ".json";
    }

    public static class Database
    {
        public static void CreateFolders()
        {
            // Create directory if not existent
            if (!Directory.Exists(DatabaseStrings.DatabaseFolderPath))
            {
                Directory.CreateDirectory(DatabaseStrings.DatabaseFolderPath);

                if (!Directory.Exists(DatabaseStrings.DatabaseCharacterFolderPath))
                {
                    Directory.CreateDirectory(DatabaseStrings.DatabaseCharacterFolderPath);
                }
            }
        }
    }
}