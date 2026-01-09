using System.Threading.Tasks;
using Configs;
using Message;
using Message.Messages;
using R3;
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
            await _levelAssetReference.Task;
            
            if (_levelAssetReference.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed to load level prefab");
                return;
            }
            _activeLevel = Object.Instantiate(_levelAssetReference.Result);
            _levelCollider = _activeLevel.GetComponent<BoxCollider2D>();
        }

        public void DestroyActiveLevel()
        {
            if (_activeLevel != null)
            {
                Object.Destroy(_activeLevel);
                _activeLevel = null;
            }

            if (_levelAssetReference.IsValid())
            {
                Addressables.Release(_levelAssetReference);
            }
        }

        public BoxCollider2D GetLevelCollider()
        {
            return _levelCollider;
        }
    }
}