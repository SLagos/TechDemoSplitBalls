﻿using System;
using UnityEngine;
using Utils.PoolSystem;

namespace GameSystem.Spawn
{
    [RequireComponent(typeof(Collider))]
    public class SpawnTrigger:MonoBehaviour
    {
        [SerializeField]
        private Spawner _spawner;

        /// <summary>
        /// Due configuration the matrix collisions layers has been modified so the spawner just collide with balls layer
        /// So there is no need to check for collisions safecheks
        /// </summary>
        /// <param name="other">The ball entering the trigger</param>
        private void OnTriggerEnter(Collider other)
        {
            PoolManager.Instance.Dispose(other.transform.parent.gameObject,EPools.Ball);
            Level.Instance.RemoveBall(other.transform.parent.gameObject);
            _spawner.Spawn();
        }
    }
}