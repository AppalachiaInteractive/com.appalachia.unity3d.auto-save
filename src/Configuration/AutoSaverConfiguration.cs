#if UNITY_EDITOR

#region

using UnityEditor;
using UnityEngine.Device;

#endregion

namespace Appalachia.Utility.Editor.AutoSave.Configuration
{
    public static class AutoSaverConfiguration
    {
        private const string PREFIX = "Appalachia/Internal/AutoSave";

        private static readonly CachedFloat _saveInterval =
            new("{0}/save-interval".Format(PREFIX), 5);

        private static readonly CachedBool _debug = new("{0}/debug".Format(PREFIX), false);
        private static readonly CachedBool _enable = new("{0}/enable-save".Format(PREFIX), true);
        private static readonly CachedInt _filesCount = new("{0}/files-count".Format(PREFIX), 10);
        private static readonly CachedFloat _lastSave = new("{0}/last-save".Format(PREFIX), 0);

        private static readonly CachedString _fileName =
            new("{0}/file-name".Format(PREFIX), "AutoSave");

        private static readonly CachedString _location =
            new("{0}/location".Format(PREFIX), "_autosave");

        internal static string DateTimeFormat = "yyyy-MM-dd_HH-mm-ss";

        internal static float SaveInterval
        {
            get => (float) _saveInterval * 60;
            set => _saveInterval.Current = value / 60;
        }

        internal static bool Debug
        {
            get => _debug.Current ?? false;
            set => _debug.Current = value;
        }

        internal static bool Enable
        {
            get => _enable.Current ?? false;
            set => _enable.Current = value;
        }

        internal static int FilesCount
        {
            get => _filesCount.Current ?? 0;
            set => _filesCount.Current = value;
        }

        internal static string FileName
        {
            get => _fileName.Current;
            set => _fileName.Current = value;
        }

        internal static float LastSave
        {
            get => (float) _lastSave;
            set => _lastSave.Current = value;
        }

        internal static float EditorTimer => (float) (EditorApplication.timeSinceStartup % 1000000);

        internal static string Location
        {
            get => _location.Current;
            set => _location.Current = value;
        }

        internal static string GetSaveDirectory()
        {
            var savePath = Application.dataPath + "/" + Location;
            return savePath;
        }

        internal static string GetRelativeSaveDirectory()
        {
            var relativeSavePath = "Assets/" + Location + "/";
            return relativeSavePath;
        }
    }
}

#endif
