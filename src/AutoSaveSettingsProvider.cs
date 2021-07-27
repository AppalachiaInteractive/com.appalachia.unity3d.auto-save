#region

using System.IO;
using UnityEditor;
using UnityEngine;

#endregion

#if UNITY_EDITOR

namespace Appalachia.AutoSave
{
    [InitializeOnLoad]
    public static class AutoSaveSettingsProvider
    {
        [SettingsProvider]
        private static SettingsProvider MyNewPrefCode0()
        {
            var p = new MyPrefSettingsProvider("Preferences/Auto-Save") {keywords = new[] {"AutoSave"}};
            return p;
        }

        // ReSharper disable once UnusedParameter.Local
        private static void OnPreferencesGUI(string searchContext)
        {
            EditorGUILayout.LabelField("Assets/" + AutoSaver.Location + " - Auto-Save Location");
            var r = EditorGUILayout.GetControlRect(GUILayout.Height(30));
            GUI.Box(r, "");
            r.x += 7;
            r.y += 7;
            AutoSaver.Enable = EditorGUI.ToggleLeft(r, "Enable", AutoSaver.Enable);
            GUI.enabled = AutoSaver.Enable;

            AutoSaver.FilesCount = Mathf.Clamp(
                EditorGUILayout.IntField("Maximum Files Version", AutoSaver.FilesCount),
                1,
                99
            );
            AutoSaver.SaveInterval = Mathf.Clamp(
                                         EditorGUILayout.IntField(
                                             "Save Every (Minutes)",
                                             (int) (AutoSaver.SaveInterval / 60)
                                         ),
                                         1,
                                         60
                                     ) *
                                     60;

            var location = EditorGUILayout.TextField("Location", AutoSaver.Location).Replace('\\', '/');
            if (location.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                location = AutoSaver.Location;
            }

            var fileName = EditorGUILayout.TextField("FileName", AutoSaver.FileName).Replace('\\', '/');
            if (fileName.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                fileName = AutoSaver.FileName;
            }

            AutoSaver.Debug = EditorGUILayout.Toggle("Log", AutoSaver.Debug);

            if (GUI.changed)
            {
                AutoSaver.Location = location;
                AutoSaver.FileName = fileName;
                AutoSaver.LastSave = AutoSaver.EditorTimer;
            }

            GUI.enabled = true;
        }

        private class MyPrefSettingsProvider : SettingsProvider
        {
            public MyPrefSettingsProvider(string path, SettingsScope scopes = SettingsScope.User) : base(path, scopes)
            {
            }

            public override void OnGUI(string searchContext)
            {
                OnPreferencesGUI(searchContext);
            }
        }
    }
}
#endif
