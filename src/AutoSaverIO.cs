#region

using System.IO;
using System.Linq;
using Appalachia.Utility.Editor.AutoSave.Configuration;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

#if UNITY_EDITOR

namespace Appalachia.Utility.Editor.AutoSave
{
    internal static class AutoSaverIO
    {
        internal static void CreateSaveDirectory()
        {
            var saveDirectory = AutoSaverConfiguration.GetSaveDirectory();

            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
                AssetDatabase.Refresh();
            }
        }

        internal static void SaveScene(Scene scene, string savePath)
        {
            CreateSaveDirectory();

            EditorSceneManager.SaveScene(scene, savePath, true);

            if (AutoSaverConfiguration.Debug)
            {
                Debug.Log("Auto-Save Current Scene: " + savePath);
            }
        }

        internal static string[] GetAutoSaveFiles()
        {
            var savePath = AutoSaverConfiguration.GetSaveDirectory();

            var files = Directory.GetFiles(savePath)
                                 .Select(f => f.Replace('\\', '/'))
                                 .Where(f => f.EndsWith(".unity"))
                                 .ToArray();

            return files;
        }
    }
}

#endif
