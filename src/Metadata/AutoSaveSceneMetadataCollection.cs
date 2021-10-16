using System;
using System.Collections.Generic;
using System.IO;
using Appalachia.Utility.Editor.AutoSave.Configuration;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Utility.Editor.AutoSave.Metadata
{
    [Serializable]
    internal class AutoSaveSceneMetadataCollection
    {
        private static readonly Comparison<AutoSaveMetadata> _sortKey = (a, b) =>
            a.saveTime.CompareTo(b.saveTime);

        public string sceneName;

        public List<AutoSaveMetadata> saves;

        public void SortSaves()
        {
            saves.Sort(_sortKey);
        }

        public void UpdateNames(string identifier)
        {
            foreach (var save in saves)
            {
                var relativeBase = AutoSaverConfiguration.GetRelativeSaveDirectory();
                var relativePath = Path.Combine(relativeBase, save.fileName) + ".unity";
                var newFileName = save.GetSaveFileName(identifier) + ".unity";

                var oldFileName = Path.GetFileName(relativePath);

                if (oldFileName == newFileName)
                {
                    continue;
                }

                var renameResult = AssetDatabase.RenameAsset(relativePath, newFileName);

                if (!string.IsNullOrEmpty(renameResult))
                {
                    Debug.LogError(renameResult);
                }
            }
        }
    }
}
