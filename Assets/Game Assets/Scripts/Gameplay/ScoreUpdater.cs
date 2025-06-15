using System;
using FiberCase.Event;
using UnityEngine;
using UnityEngine.UI;

namespace FiberCase.Gameplay
{
    public class ScoreUpdater : MonoBehaviour
    {
        [SerializeField] private float _scoreMultiplier;
        [SerializeField] private float _scoreGoal;

        [SerializeField] private Image _scoreBar;
        
        private float _currentScore;

        private void OnEnable()
        {
            EventBus.Subscribe<CoinsPoppedEvent>(CoinsPopped);
        }

        private void CoinsPopped(CoinsPoppedEvent coinsPoppedEvent)
        {
            Debug.Log("Got score");
            var scoreGained = coinsPoppedEvent.CoinAmount * coinsPoppedEvent.CoinValue * _scoreMultiplier;
            _currentScore += scoreGained;
            SetScoreBar();

            if (CheckIfReachedGoal())
            {
                //TODO: Win the game.
            }
        }

        private void SetScoreBar()
        {
            _scoreBar.fillAmount = _currentScore / _scoreGoal;
        }

        private bool CheckIfReachedGoal()
        {
            return _currentScore >= _scoreGoal;
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<CoinsPoppedEvent>(CoinsPopped);

        }
    }
}