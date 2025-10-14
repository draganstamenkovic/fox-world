using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Transform _cannonTransform;
        
        public Rigidbody2D Rigidbody => _rigidbody;
        public Transform CannonTransform => _cannonTransform;
    }
}