#if UNITY_EDITOR

#region

using Appalachia.Utility.Editor.AutoSave.Configuration;
using UnityEditor;
using UnityEngine;

#endregion

namespace Appalachia.Utility.Editor.AutoSave
{
    [InitializeOnLoad]
    internal static class EditorApplicationUpdateHandler
    {
        private static float? _launchTime;
        private static float _editorTimer;

        static EditorApplicationUpdateHandler()
        {
            EditorApplication.update -= OnEditorApplicationUpdate;
            EditorApplication.update += OnEditorApplicationUpdate;
        }

        public static void OnEditorApplicationUpdate()
        {
            if (!AutoSaverConfiguration.Enable)
            {
                return;
            }

            if (Application.isPlaying)
            {
                if (_launchTime == null)
                {
                    _launchTime = AutoSaverConfiguration.EditorTimer;
                }

                return;
            }

            if (_launchTime != null)
            {
                AutoSaverConfiguration.LastSave +=
                    AutoSaverConfiguration.EditorTimer - _launchTime.Value;
                _launchTime = null;
            }

            if (Mathf.Abs(_editorTimer - AutoSaverConfiguration.EditorTimer) < 4)
            {
                return;
            }

            _editorTimer = AutoSaverConfiguration.EditorTimer;

            var saveAge = AutoSaverConfiguration.LastSave - AutoSaverConfiguration.EditorTimer;

            if (Mathf.Abs(saveAge) >= AutoSaverConfiguration.SaveInterval)
            {
                AutoSaver.Execute();
                EditorApplication.update -= OnEditorApplicationUpdate;
                EditorApplication.update += OnEditorApplicationUpdate;
            }
        }
    }
}

#endif
