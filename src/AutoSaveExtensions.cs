#if UNITY_EDITOR

namespace Appalachia.AutoSave
{
    internal static class AutoSaveExtensions
    {
        public static string Format(this string formatString, params object[] args)
        {
            return string.Format(formatString, args);
        }
    }
}

#endif
