using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemiesConfig", menuName = "Configs/EnemiesConfig")]
    public class EnemiesConfig : ScriptableObject
    {
        [Header("Vulture")]
        public float vultureMoveDuration;
        public List<EnemyData> Enemies;

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