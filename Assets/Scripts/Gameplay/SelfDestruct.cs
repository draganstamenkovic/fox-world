using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float _selfDestructTime = 3f;
    private float _time;
    private void Update()
    {
        if (!gameObject.activeSelf) return;
        
        _time += Time.deltaTime;
        if (_time > _selfDestructTime)
        {
            Destroy(gameObject);
        }
    }
}
