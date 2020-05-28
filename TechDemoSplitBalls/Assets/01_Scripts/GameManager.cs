using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Utils.PoolSystem;

namespace GameSystem
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        private Animator _uiController;

        [SerializeField] private TextMeshProUGUI _scoreLabel;
        private int _score = 0;

        private void Start()
        {
            SceneManager.LoadScene("MainGame");
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= SceneLoaded;
            StartCoroutine(WaitForLevelReady());
        }

        private IEnumerator WaitForLevelReady()
        {
            yield return new WaitWhile(()=> Level.Instance == null); //Waiting for the level instanciation
            yield return new WaitUntil(() => Level.Instance.Ready); // waitinf for level inizializarion
            _uiController.SetTrigger("Out"); // hiding tally
            Level.Instance.StartLevel();
        }
        public void AddScore(int amount = 0)
        {
            if (amount > 0)
                _score += amount;
            else
            {
                ++_score;
            }
        }

        public void EndGame()
        {
            if (_score > 0)
                Victory();
            else
                Lose();
        }

        private void Victory()
        {
            _uiController.ResetTrigger("Out");
            _uiController.SetTrigger("In");
            _uiController.SetBool("Win",true);
            _scoreLabel.text = _score.ToString();
            InputManager.OnDrag += Restart;
        }
        private void Lose()
        {
            _uiController.ResetTrigger("Out");
            _uiController.SetTrigger("In");
            _uiController.SetBool("Win",false);
            InputManager.OnDrag += Restart;
        }

        private void Restart()
        {
            InputManager.OnDrag -= Restart;
            PoolManager.Instance.Clear();
            SceneManager.LoadScene("MainGame");
            SceneManager.sceneLoaded += SceneLoaded;
            _score = 0;
            InputManager.Instance.Restart();
        }
    }
}

       