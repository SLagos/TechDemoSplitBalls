using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Utils;
using Utils.PoolSystem;

namespace GameSystem
{
    public class Level : Singleton<Level>
    {
        [SerializeField]
        private Transform _startPoint;

        [SerializeField]
        private Transform _cameraHolder;
        
        private List<GameObject> _balls = new List<GameObject>();

        private Vector3 _rotation;
        private bool _ready = false;
        public bool Ready =>_ready;
        private Coroutine _endCondition;

        private void Update()
        {
            _rotation.y = Mathf.Lerp(-25,25,(InputManager.Instance.DragValue+1)/2);
            _cameraHolder.eulerAngles = _rotation;
        }

        public void Start()
        {
            PoolManager.Instance.InitializePool(EPools.Ball);
            _rotation = _cameraHolder.eulerAngles;
            _ready = true;
        }

        public void AddBall(GameObject ball)
        {
            _balls.Add(ball);
        }

        public void RemoveBall(GameObject ball)
        {
            _balls.Remove(ball);
            if (_balls.Count == 0)
            {
                if (_endCondition != null)
                {
                    StopCoroutine(_endCondition);
                }
                _endCondition =StartCoroutine(CheckLoseCondition());
            }
        }

        private IEnumerator CheckLoseCondition()
        {
            yield return new WaitForSeconds(3f);
            if (_balls.Count == 0)
            {
                GameManager.Instance.EndGame();
            }
        }

        public void StartLevel()
        {
            AddBall(PoolManager.Instance.Instantiate(EPools.Ball, _startPoint.position, this.transform));
        }
        
    }
}

