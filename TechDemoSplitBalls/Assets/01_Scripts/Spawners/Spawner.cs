using System.Collections;
using System.Collections.Generic;
using GameSystem.Settings;
using UnityEngine;
using Utils.PoolSystem;

namespace GameSystem.Spawn
{
    /// <summary>
    /// This class is in charge of spawn things in the game
    /// </summary>
    public class Spawner : MonoBehaviour
    {

        [SerializeField]
        private SettingsScriptableObject _settings;
        /// <summary>
        /// The pool that is going to be used to spawn
        /// </summary>
        [SerializeField] 
        private EPools _objectToSpawn;

        /// <summary>
        /// The amount of item that are going to be spanw by activation
        /// </summary>
        [SerializeField] 
        private int _amountToSpawn;

        /// <summary>
        /// Where the object spawned is going to appear
        /// </summary>
        [SerializeField]
        private Transform _spawnPoint;

        private Coroutine _spawnRoutine;

        private int _objectsToSpawn;

        public void Spawn(int amount = 0)
        {
            if (amount > 0)
                _amountToSpawn = amount;
            if (_amountToSpawn < _settings.SpawnSafeTreshHold)
            {
                ImmediateSpawnObject();
            }
            else
            {
                if(_spawnRoutine == null)
                    _spawnRoutine = StartCoroutine(SafeSpawnObject());
                else
                {
                    _objectsToSpawn += _amountToSpawn;
                }
            }
        }
        /// <summary>
        /// Method to call when the amount of items to spawn are less
        /// thant the threshold to avoid main thread lock
        /// </summary>
        private void ImmediateSpawnObject()
        {
            for (int i = 0; i < _amountToSpawn; i++)
            {
                Level.Instance.AddBall(PoolManager.Instance.Instantiate(_objectToSpawn, _spawnPoint.transform.position,Level.Instance.transform));
            }
        }
        /// <summary>
        /// Mehtod to call when the amount of item to spawn are more
        /// than the treshhold to avoid main thread lock
        /// </summary>
        /// <returns>Coroutine in charge of spawn objects</returns>

        private IEnumerator SafeSpawnObject()
        {
            int iterations = 0;
            _objectsToSpawn = _amountToSpawn;
            //This is to avoid having multiple routines, we just handle one and if is called again
            // we add that amount to the routine running, otherwise we create a new one
            while (_objectsToSpawn > 0) 
            {
                Level.Instance.AddBall(PoolManager.Instance.Instantiate(_objectToSpawn, _spawnPoint.transform.position,Level.Instance.transform));
                --_objectsToSpawn;
                ++iterations;
                yield return new WaitForSeconds(_settings.SpawnRate);
            }
            yield return null;
            _spawnRoutine = null;
        }
    }
}


