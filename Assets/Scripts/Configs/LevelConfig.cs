using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public List<LevelData> levels;

        public AssetReferenceGameObject GetLevelData(int levelId) => 
            levels.Find(level => level.id == levelId).level;
    }
    
    [Serializable]
    public class LevelData
    {
        public int id;
        public AssetReferenceGameObject level;
    }
}