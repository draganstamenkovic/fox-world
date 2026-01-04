using System.Threading.Tasks;
using Configs;
using Message;
using Message.Messages;
using R3;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using Object = UnityEngine.Object;

namespace Gameplay.Level
{
    public class LevelManager : ILevelManager
    {
        [Inject] private readonly LevelConfig _levelConfig;
        [Inject] private readonly IMessageBroker _messageBroker;
        private int _activeLevelId;
        private BoxCollider2D _levelCollider;
        public void Initialize()
        {
            _messageBroker.Receive<SelectedLevelMessage>().Subscribe(message =>
            {
                _activeLevelId = message.LevelIndex;
            });
        }

        public async Task LoadLevel()
        {
            var levelAssetReference = _levelConfig.GetLevelData(_activeLevelId);
            if (levelAssetReference == null)
            {
                Debug.LogError("No level data found for level");
                return;
            }
            
            var handle = levelAssetReference.LoadAssetAsync();
            var levelPrefab = await handle.Task;
            var level = Object.Instantiate(levelPrefab);
            _levelCollider = level.GetComponent<BoxCollider2D>();
        }

        public void LoadNextLevel()
        {
            
        }

        public void DestroyActiveLevel()
        {
            
        }

        public BoxCollider2D GetLevelCollider()
        {
            return _levelCollider;
        }
    }
}