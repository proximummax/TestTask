using System.Collections.Generic;

namespace Game.Scripts.Storage
{
    public interface IBoxesPersistence
    {
        List<BoxSaveParameters> Load(string saveName);
        void Save(string saveName, List<BoxSaveParameters> data, bool overwrite);
    }
}


