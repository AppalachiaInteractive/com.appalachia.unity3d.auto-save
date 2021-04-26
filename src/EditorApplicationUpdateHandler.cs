#if UNITY_EDITOR

#region

using UnityEditor;
using UnityEngine;

#endregion

namespace Appalachia.AutoSave
{
    [InitializeOnLoad]
    public static class EditorApplicationUpdateHandler
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
            if (!AutoSaver.Enable)
            {
                return;
            }

            if (Application.isPlaying)
            {
                if (_launchTime == null)
                {
                    _launchTime = AutoSaver.EditorTimer;
                }

                return;
            }

            if (_launchTime != null)
            {
                AutoSaver.LastSave += AutoSaver.EditorTimer - _launchTime.Value;
                _launchTime = null;
            }

            if (Mathf.Abs(_editorTimer - AutoSaver.EditorTimer) < 4)
            {
                return;
            }

            _editorTimer = AutoSaver.EditorTimer;

            if (Mathf.Abs(AutoSaver.LastSave - AutoSaver.EditorTimer) >= (AutoSaver.SaveInterval * 2))
            {
                AutoSaver.LastSave = AutoSaver.EditorTimer;
            }

            if (Mathf.Abs(AutoSaver.LastSave - AutoSaver.EditorTimer) >= AutoSaver.SaveInterval)
            {
                AutoSaveIO.SaveScene();
                EditorApplication.update -= OnEditorApplicationUpdate;
                EditorApplication.update += OnEditorApplicationUpdate;
            }
        }
    }
}

#endif
