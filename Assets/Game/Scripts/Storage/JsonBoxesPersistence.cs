using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Scripts.Storage
{
    public class JsonBoxesPersistence : IBoxesPersistence
    {
        [Serializable]
        private class BoxesSaveData
        {
            public List<BoxSaveParameters> Boxes = new();
        }

        public List<BoxSaveParameters> Load(string saveName)
        {
            var path = GetFilePath(saveName);
            if (!File.Exists(path))
            {
                return new List<BoxSaveParameters>();
            }

            var json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<BoxSaveParameters>();
            }

            var container = JsonUtility.FromJson<BoxesSaveData>(json);
            return container?.Boxes ?? new List<BoxSaveParameters>();
        }

        public void Save(string saveName, List<BoxSaveParameters> data, bool overwrite)
        {
            var path = GetFilePath(saveName);
            if (overwrite && File.Exists(path))
            {
                File.Delete(path);
            }

            var container = new BoxesSaveData { Boxes = data };
            var json = JsonUtility.ToJson(container);
            File.WriteAllText(path, json);
        }

        private static string GetFilePath(string saveName)
        {
            var fileName = saveName.EndsWith(".json") ? saveName : saveName + ".json";
            return Application.persistentDataPath + "/" + fileName;
        }
    }
}


