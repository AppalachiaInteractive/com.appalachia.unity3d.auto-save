#if UNITY_EDITOR

#region

using UnityEditor;

#endregion

namespace Appalachia.Utility.Editor.AutoSave.Configuration
{
    internal class CachedInt : Cached<int?, int>
    {
        public CachedInt(string key, int defaultValue) : base(key, defaultValue)
        {
        }

        protected override int? Get(string key, int defaultValue)
        {
            var value = EditorPrefs.GetInt(key, defaultValue);
            return value;
        }

        protected override void Set(string key, int? value)
        {
            if (value.HasValue)
            {
                EditorPrefs.SetInt(key, value.Value);
            }
        }
    }
}

#endif
