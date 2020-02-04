using System.Collections.Generic;

namespace SaveManager.IO
{
    public interface SaveManagerIO
    {
        void Save(Dictionary<string, object> dictionary, string saveGameName);
        Dictionary<string, object> Load(string saveGameName);
    }
}