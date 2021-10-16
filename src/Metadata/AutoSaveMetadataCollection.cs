using System;
using System.Collections.Generic;

namespace Appalachia.Utility.Editor.AutoSave.Metadata
{
    [Serializable]
    internal class AutoSaveMetadataCollection
    {
        public Dictionary<string, AutoSaveSceneMetadataCollection> scenes;

        public AutoSaveMetadataCollection(string[] files)
        {
            var saves = AutoSaveMetadata.FromFiles(files);

            if (scenes == null)
            {
                scenes = new Dictionary<string, AutoSaveSceneMetadataCollection>();
            }
            else
            {
                scenes.Clear();
            }

            foreach (var save in saves)
            {
                if (!scenes.ContainsKey(save.sceneName))
                {
                    scenes.Add(save.sceneName, new AutoSaveSceneMetadataCollection());
                }

                var sceneMetadata = scenes[save.sceneName];

                if (sceneMetadata.saves == null)
                {
                    sceneMetadata.saves = new List<AutoSaveMetadata>();
                }

                sceneMetadata.saves.Add(save);
            }
        }
    }
}
