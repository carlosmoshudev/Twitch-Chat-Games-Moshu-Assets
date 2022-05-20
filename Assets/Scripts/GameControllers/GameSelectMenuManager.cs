using UnityEngine;
namespace TwitchMiniGames.GameControllers
{
    public class GameSelectMenuManager : MonoBehaviour
    {
        [Header("Global")]
        [SerializeField] private GlobalVariables _globalVariables;
        [Header("Game Select")]
        [SerializeField] private GameObject _gameSelectPanel;
        [Space(10)]
        [SerializeField] private GameObject _ahorcadoPanel;
        [SerializeField] private GameObject _stopPanel;
        [SerializeField] private GameObject _laberynthPanel;

        public void StartAhorcado() => OnGameSeleted(_ahorcadoPanel, GameSelected.Ahorcado);
        public void StartStop() => OnGameSeleted(_stopPanel, GameSelected.Stop);
        public void StartLabyrinth() => OnGameSeleted(_laberynthPanel, GameSelected.Laberinto);

        public void OnClose() => _gameSelectPanel.SetActive(false);

        private void OnGameSeleted(GameObject panel, GameSelected gameSelected)
        {
            this.gameObject.SetActive(value: false);
            panel.SetActive(value: true);
            _globalVariables.gameSelected = gameSelected;
        }
    }
}
