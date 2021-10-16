#region

using System.IO;
using UnityEditor;
using UnityEngine;

#endregion

#if UNITY_EDITOR

namespace Appalachia.Utility.Editor.AutoSave.Configuration
{
    [InitializeOnLoad]
    internal static class AutoSaveSettingsProvider
    {
        [SettingsProvider]
        private static SettingsProvider MyNewPrefCode0()
        {
            var p = new MyPrefSettingsProvider("Preferences/Appalachia/Auto-Save")
            {
                keywords = new[] {"AutoSave"}
            };
            return p;
        }

        // ReSharper disable once UnusedParameter.Local
        private static void OnPreferencesGUI(string searchContext)
        {
            EditorGUILayout.LabelField(
                "Assets/" + AutoSaverConfiguration.Location + " - Auto-Save Location"
            );
            var r = EditorGUILayout.GetControlRect(GUILayout.Height(30));
            GUI.Box(r, "");
            r.x += 7;
            r.y += 7;
            AutoSaverConfiguration.Enable = EditorGUI.ToggleLeft(
                r,
                "Enable",
                AutoSaverConfiguration.Enable
            );
            GUI.enabled = AutoSaverConfiguration.Enable;

            AutoSaverConfiguration.FilesCount = Mathf.Clamp(
                EditorGUILayout.IntField(
                    "Maximum Files Version",
                    AutoSaverConfiguration.FilesCount
                ),
                1,
                99
            );
            AutoSaverConfiguration.SaveInterval = Mathf.Clamp(
                                                      EditorGUILayout.IntField(
                                                          "Save Every (Minutes)",
                                                          (int) (AutoSaverConfiguration
                                                             .SaveInterval /
                                                          60)
                                                      ),
                                                      1,
                                                      60
                                                  ) *
                                                  60;

            var location = EditorGUILayout.TextField("Location", AutoSaverConfiguration.Location)
                                          .Replace('\\', '/');
            if (location.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                location = AutoSaverConfiguration.Location;
            }

            var fileName = EditorGUILayout.TextField("FileName", AutoSaverConfiguration.FileName)
                                          .Replace('\\', '/');
            if (fileName.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                fileName = AutoSaverConfiguration.FileName;
            }

            AutoSaverConfiguration.Debug =
                EditorGUILayout.Toggle("Log", AutoSaverConfiguration.Debug);

            if (GUI.changed)
            {
                AutoSaverConfiguration.Location = location;
                AutoSaverConfiguration.FileName = fileName;
                AutoSaverConfiguration.LastSave = AutoSaverConfiguration.EditorTimer;
            }

            GUI.enabled = true;
        }

        private class MyPrefSettingsProvider : SettingsProvider
        {
            public MyPrefSettingsProvider(
                string path,
                SettingsScope scopes = SettingsScope.User) : base(path, scopes)
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
