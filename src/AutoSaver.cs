#if UNITY_EDITOR

#region

using UnityEditor;

#endregion

namespace Appalachia.AutoSave
{
    public static class AutoSaver
    {
        private const string PREFIX = "Appalachia/Internal/AutoSave";
        private static readonly CachedFloat _saveInterval = new CachedFloat("{0}/save-interval".Format(PREFIX), 5);
        private static readonly CachedBool _debug = new CachedBool("{0}/debug".Format(PREFIX),        false);
        private static readonly CachedBool _enable = new CachedBool("{0}/enable-save".Format(PREFIX), true);
        private static readonly CachedInt _filesCount = new CachedInt("{0}/files-count".Format(PREFIX), 10);
        private static readonly CachedFloat _lastSave = new CachedFloat("{0}/last-save".Format(PREFIX), 0);
        private static readonly CachedString _fileName = new CachedString("{0}/file-name".Format(PREFIX), "AutoSave");
        private static readonly CachedString _location = new CachedString("{0}/location".Format(PREFIX),  "_autosave");

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
    }
}

#endif
