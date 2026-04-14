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

        public IReadOnlyList<BoxSaveParameters> Load(string saveName)
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

        public void Save(string saveName, IReadOnlyList<BoxSaveParameters> data)
        {
            var path = GetFilePath(saveName);
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? Application.persistentDataPath);
            var container = new BoxesSaveData { Boxes = new List<BoxSaveParameters>(data) };
            var json = JsonUtility.ToJson(container);
            File.WriteAllText(path, json);
        }

        private static string GetFilePath(string saveName)
        {
            var fileName = saveName.EndsWith(".json") ? saveName : saveName + ".json";
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}
