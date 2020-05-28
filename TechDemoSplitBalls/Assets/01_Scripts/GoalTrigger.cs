using System;
using UnityEngine;
using Utils.PoolSystem;

namespace GameSystem
{
    public class GoalTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            PoolManager.Instance.Dispose(other.transform.parent.gameObject,EPools.Ball);
            Level.Instance.RemoveBall(other.transform.parent.gameObject);
            GameManager.Instance.AddScore();
        }
    }
}