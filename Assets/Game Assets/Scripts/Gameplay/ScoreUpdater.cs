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
            EventBus.Subscribe<PlayAgainEvent>(ResetScore);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<CoinsPoppedEvent>(CoinsPopped);
            EventBus.Unsubscribe<PlayAgainEvent>(ResetScore);
        }

        private void CoinsPopped(CoinsPoppedEvent coinsPoppedEvent)
        {
            var scoreGained = coinsPoppedEvent.CoinAmount * coinsPoppedEvent.CoinValue * _scoreMultiplier;
            _currentScore += scoreGained;
            SetScoreBar();

            if (CheckIfReachedGoal())
                EventBus.Raise(new GameWonEvent());
        }

        private void SetScoreBar()
        {
            _scoreBar.fillAmount = _currentScore / _scoreGoal;
        }

        private bool CheckIfReachedGoal()
        {
            return _currentScore >= _scoreGoal;
        }

        private void ResetScore(PlayAgainEvent playAgainEvent)
        {
            _currentScore = 0;
            SetScoreBar();
        }
    }
}