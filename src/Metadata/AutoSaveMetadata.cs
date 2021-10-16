using System;
using System.Collections.Generic;
using System.IO;
using Appalachia.Utility.Editor.AutoSave.Configuration;
using UnityEngine.SceneManagement;

namespace Appalachia.Utility.Editor.AutoSave.Metadata
{
    [Serializable]
    internal class AutoSaveMetadata
    {
        public string sceneName;
        public DateTime saveTime;
        public string filePath;
        public string fileName;

        public AutoSaveMetadata()
        {
            sceneName = "unsaved";
            saveTime = DateTime.Now;
        }

        public AutoSaveMetadata(Scene scene)
        {
            sceneName = scene.name;

            if (string.IsNullOrWhiteSpace(sceneName))
            {
                sceneName = "unsaved";
            }

            saveTime = DateTime.Now;
        }

        public string GetSaveFileName(string identifier)
        {
            var formattedTime = saveTime.ToString(AutoSaverConfiguration.DateTimeFormat);
            return $"{sceneName}.{identifier}.{formattedTime}";
        }

        public static List<AutoSaveMetadata> FromFiles(string[] files)
        {
            var results = new List<AutoSaveMetadata>();

            foreach (var file in files)
            {
                var cleanFileName = Path.GetFileNameWithoutExtension(file);
                var fileInfo = new FileInfo(file);

                var splits = cleanFileName.Split('.');

                if (splits.Length is < 3 or > 5)
                {
                    continue;
                }

                var save = new AutoSaveMetadata {filePath = file, fileName = cleanFileName};

                if (splits.Length == 3)
                {
                    save.sceneName = splits[0];
                    save.saveTime = default;
                }
                else if (splits.Length == 4)
                {
                    save.sceneName = splits[0];

                    if (!DateTime.TryParse(splits[2], out save.saveTime))
                    {
                        save.saveTime = default;
                    }
                }
                else if (splits.Length == 5)
                {
                    save.sceneName = splits[0];

                    if (!DateTime.TryParse($"{splits[2]} {splits[3]}", out save.saveTime))
                    {
                        save.saveTime = default;
                    }
                }

                if (save.saveTime == default)
                {
                    save.saveTime = fileInfo.CreationTime;
                }

                results.Add(save);
            }

            return results;
        }
    }
}
