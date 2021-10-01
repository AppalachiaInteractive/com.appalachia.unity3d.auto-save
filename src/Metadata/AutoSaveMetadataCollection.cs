using System;
using System.Collections.Generic;

namespace Appalachia.AutoSave
{
    [Serializable]
    internal class AutoSaveMetadataCollection
    {
        public AutoSaveMetadataCollection (string[] files)
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
        
        public Dictionary<string, AutoSaveSceneMetadataCollection> scenes;
    }
}