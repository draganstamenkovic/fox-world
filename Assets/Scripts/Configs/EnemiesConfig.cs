using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemiesConfig", menuName = "Configs/EnemiesConfig")]
    public class EnemiesConfig : ScriptableObject
    {
        public List<EnemyData> Enemies;

        [Header("Grid settings")] 
        public float GridCellSize = 1f;
        public int CellHeight = 10;
        public int CellWidth = 10;
        public float GridPositionOffset = 0.5f;
        public EnemyData GetEnemy(string id)
        {
            var enemy = Enemies.Find(e => e.Id == id);
            if (enemy != null) return enemy;
            
            Debug.LogError("No enemy with that id");
            return null;
        }
    }

    [Serializable]
    public class EnemyData
    {
        public string Id;
        public int Health;
        public GameObject Prefab;
    }
}