using FiberCase.Event;
using UnityEngine;

namespace FiberCase.Systems
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private GameObject _loseScreen;

        private void OnEnable()
        {
            EventBus.Subscribe<GameWonEvent>(WinTheGame);
            EventBus.Subscribe<GameLostEvent>(LoseTheGame);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<GameWonEvent>(WinTheGame);
            EventBus.Unsubscribe<GameLostEvent>(LoseTheGame);
        }

        private void WinTheGame(GameWonEvent gameWonEvent)
        {
            _winScreen.SetActive(true);
        }

        private void LoseTheGame(GameLostEvent  gameLostEvent)
        {
            _loseScreen.SetActive(true);
        }

        public void PlayAgain()
        {
            _winScreen.SetActive(false);
            _loseScreen.SetActive(false);
            EventBus.Raise(new PlayAgainEvent());
        }
    }
}