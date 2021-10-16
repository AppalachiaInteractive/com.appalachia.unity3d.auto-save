#if UNITY_EDITOR

using UnityEditor;

namespace Appalachia.Utility.Editor.AutoSave.Configuration
{
    internal class CachedString : Cached<string, string>
    {
        public CachedString(string key, string defaultValue) : base(key, defaultValue)
        {
        }

        protected override string Get(string key, string defaultValue)
        {
            var value = EditorPrefs.GetString(key, defaultValue);
            return value;
        }

        protected override void Set(string key, string value)
        {
            EditorPrefs.SetString(key, value);
        }
    }
}
#endif
