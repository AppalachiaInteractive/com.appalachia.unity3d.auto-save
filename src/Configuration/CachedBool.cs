#if UNITY_EDITOR

#region

using UnityEditor;

#endregion

namespace Appalachia.Utility.Editor.AutoSave.Configuration
{
    internal class CachedBool : Cached<bool?, bool>
    {
        public CachedBool(string key, bool defaultValue) : base(key, defaultValue)
        {
        }

        protected override bool? Get(string key, bool defaultValue)
        {
            var value = EditorPrefs.GetInt(key, defaultValue ? 1 : 0);
            return value == 1;
        }

        protected override void Set(string key, bool? value)
        {
            EditorPrefs.SetInt(key, value ?? false ? 1 : 0);
        }
    }
}

#endif
