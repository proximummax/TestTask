using System.Collections.Generic;

namespace Game.Scripts.Storage
{
    public interface IBoxesPersistence
    {
        IReadOnlyList<BoxSaveParameters> Load(string saveName);
        void Save(string saveName, IReadOnlyList<BoxSaveParameters> data);
    }
}
