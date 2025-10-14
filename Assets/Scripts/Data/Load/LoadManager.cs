using System.IO;

namespace Data.Load
{
    public class LoadManager : ILoadManager
    {
        public void Initialize()
        {
            Load();
            FillData();
        }

        public void Load()
        {
            if (!File.Exists(DataManager.DataPath))
            {
               // _tempGameData = new SerializableGameData();
            }
            else
            {
                var json = File.ReadAllText(DataManager.DataPath);
               // _tempGameData = JsonUtility.FromJson<SerializableGameData>(json);
            }
        }

        private void FillData()
        {
            //TODO: Add logic
        }
    }
}