#region

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

#if UNITY_EDITOR

namespace Appalachia.AutoSave
{
    public static class AutoSaveIO
    {
        internal static string GetSaveDirectory()
        {
            var savePath = Application.dataPath + "/" + AutoSaver.Location;
            return savePath;
        }

        internal static string GetRelativeSaveDirectory()
        {
            var relativeSavePath = "Assets/" + AutoSaver.Location + "/";
            return relativeSavePath;
        }

        internal static void CreateSaveDirectory()
        {
            var saveDirectory = GetSaveDirectory();

            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
                AssetDatabase.Refresh();
            }
        }

        internal static void SaveScene()
        {
            CreateSaveDirectory();

            var scene = SceneManager.GetActiveScene();
            var relativeSavePath = GetRelativeSaveDirectory();
            var autosaveFileName = GetAutoSaveFileName();
            var sceneName = scene.name;
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                sceneName = "unsaved";
            }
            
            var filename = "{0}.{1}.unity".Format(sceneName, autosaveFileName);
            var savePath = "{0}{1}".Format(relativeSavePath, filename);

            EditorSceneManager.SaveScene(scene, savePath, true);
            var dif = AutoSaver.EditorTimer - AutoSaver.LastSave - AutoSaver.SaveInterval;
            AutoSaver.LastSave = (dif < AutoSaver.SaveInterval) && (dif > 0)
                ? AutoSaver.EditorTimer - dif
                : AutoSaver.EditorTimer;

            if (AutoSaver.Debug)
            {
                Debug.Log("Auto-Save Current Scene: " + savePath);
            }
        }

        internal static string GetAutoSaveFileName()
        {
            var filename = AutoSaver.FileName;
            var savePath = GetSaveDirectory();

            var files = Directory.GetFiles(savePath)
                                 .Select(f => f.Replace('\\', '/'))
                                 .Where(
                                      f => f.EndsWith(".unity") &&
                                           f.Substring(f.LastIndexOf('/') + 1).StartsWith(filename)
                                  )
                                 .ToArray();
            if (files.Length == 0)
            {
                return "{0}.00".Format(filename);
            }

            var times = files.Select(File.GetCreationTime).ToList();
            var max = times.Max();
            var ind = times.IndexOf(max);
            int count;
            files = files.Select(n => n.Remove(n.LastIndexOf('.'))).ToArray();
            if (int.TryParse(files[ind].Substring(files[ind].Length - 2), out count))
            {
                count = (count + 1) % AutoSaver.FilesCount;
                return "{0}.{1}".Format(filename, count.ToString("D2"));
            }

            return "{0}.00".Format(filename);
        }
    }
}

#endif
