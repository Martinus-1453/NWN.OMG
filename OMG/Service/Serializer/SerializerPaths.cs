using System.IO;

namespace OMG.Service
{
    internal static class SerializerPaths
    {
        static SerializerPaths()
        {
            Directory.CreateDirectory(FolderPath);
            Directory.CreateDirectory(CharacterFolderPath);
            Directory.CreateDirectory(PlaceableFolderPath);
        }

        private static string FolderPath { get; } = "db_omg";
        internal static string CharacterFolderPath { get; } = Path.Join(FolderPath, "players");
        internal static string PlaceableFolderPath { get; } = Path.Join(FolderPath, "placeables");
        internal static string ServerFolderPath { get; } = Path.Join(FolderPath, "server");
        internal static string FileFormat { get; } = ".json";
    }
}