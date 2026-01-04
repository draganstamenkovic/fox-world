using System.Threading.Tasks;
using Configs;
using Message;
using Message.Messages;
using R3;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using Object = UnityEngine.Object;

namespace Gameplay.Level
{
    public class LevelManager : ILevelManager
    {
        [Inject] private readonly LevelConfig _levelConfig;
        [Inject] private readonly IMessageBroker _messageBroker;
        private AsyncOperationHandle<GameObject> _levelAssetReference;
        private BoxCollider2D _levelCollider;
        private GameObject _activeLevel;
        private int _activeLevelId;
        
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
            _levelAssetReference = levelAssetReference.LoadAssetAsync();
            var levelPrefab = await _levelAssetReference.Task;
            _activeLevel = Object.Instantiate(levelPrefab);
            _levelCollider = _activeLevel.GetComponent<BoxCollider2D>();
            levelAssetReference.ReleaseAsset();
        }

        public void LoadNextLevel()
        {
            
        }

        public void DestroyActiveLevel()
        {
            Object.Destroy(_activeLevel);
           // Addressables.Release(_levelAssetReference);
        }

        public BoxCollider2D GetLevelCollider()
        {
            return _levelCollider;
        }
    }
}