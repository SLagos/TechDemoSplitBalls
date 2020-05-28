using GameSystem.Spawn;
using UnityEngine;
using Utils.PoolSystem;

namespace GameSystem
{
    [RequireComponent(typeof(Collider))]
    public class LockGate : MonoBehaviour
    {
        [SerializeField]
        private int _amountToUnlock;
        [SerializeField]
        private Spawner _spawner;

        [SerializeField] 
        private float _bonusValue;

        [SerializeField]
        private MeshRenderer _render;

        private int _currentAmount = 0;
        private bool _active = true;
        
        private void OnTriggerEnter(Collider other)
        {
            if (_active)
            {
                PoolManager.Instance.Dispose(other.transform.parent.gameObject,EPools.Ball);
                Level.Instance.RemoveBall(other.transform.parent.gameObject);
                ++_currentAmount;
                if (_currentAmount >= _amountToUnlock)
                {
                    _active = false;
                    _render.enabled = false;
                    _spawner.Spawn(Mathf.FloorToInt(_amountToUnlock*_bonusValue));
                }
            }
            
        }

    }
}