using System.IO;
using Data.Load;
using UnityEngine;

namespace Data
{
    public class DataManager
    {
        public static string DataPath => Path.Combine(Application.persistentDataPath + "/gamedata.json");
    }
}