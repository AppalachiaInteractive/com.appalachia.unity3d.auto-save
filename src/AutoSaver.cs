using System.IO;
using Appalachia.Utility.Editor.AutoSave.Configuration;
using Appalachia.Utility.Editor.AutoSave.Metadata;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Appalachia.Utility.Editor.AutoSave
{
    internal static class AutoSaver
    {
        public static void Execute()
        {
            SaveCurrentScene();
            OrganizeExistingSaves();
        }

        private static void SaveCurrentScene()
        {
            var scene = SceneManager.GetActiveScene();
            var newSave = new AutoSaveMetadata(scene);

            var relativeSavePath = AutoSaverConfiguration.GetRelativeSaveDirectory();

            var saveFilePath = GetAutoSaveFilePath(newSave, relativeSavePath);

            AutoSaverIO.SaveScene(scene, saveFilePath);

            AutoSaverConfiguration.LastSave = AutoSaverConfiguration.EditorTimer;
        }

        private static void OrganizeExistingSaves()
        {
            var allSaveFiles = AutoSaverIO.GetAutoSaveFiles();
            var saveCollection = new AutoSaveMetadataCollection(allSaveFiles);
            var saveNameIdentifier = AutoSaverConfiguration.FileName;

            foreach (var scene in saveCollection.scenes.Keys)
            {
                var saves = saveCollection.scenes[scene];

                saves.SortSaves();

                while (saves.saves.Count > AutoSaverConfiguration.FilesCount)
                {
                    var first = saves.saves[0];
                    saves.saves.RemoveAt(0);

                    var relativeBase = AutoSaverConfiguration.GetRelativeSaveDirectory();
                    var relativePath = Path.Combine(relativeBase, first.fileName) + ".unity";

                    AssetDatabase.DeleteAsset(relativePath);
                }

                saves.UpdateNames(saveNameIdentifier);
            }
        }

        public static string GetAutoSaveFilePath(AutoSaveMetadata metadata, string relativeSavePath)
        {
            var saveNameIdentifier = AutoSaverConfiguration.FileName;
            var autosaveFileName = metadata.GetSaveFileName(saveNameIdentifier);

            var filename = $"{autosaveFileName}.unity";
            var finalOutputPath = "{0}{1}".Format(relativeSavePath, filename);

            return finalOutputPath;
        }
    }
}
