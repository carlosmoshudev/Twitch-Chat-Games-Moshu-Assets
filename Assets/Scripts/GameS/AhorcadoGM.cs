using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using TwitchChatConnect.Data;
namespace TwitchMiniGames.Games
{
    [Serializable]
    public class AhorcadoGM : MonoBehaviour
    {
        [SerializeField] private string _word = "test";
        private char[] _wordArray;
        private int hangSteps = 10;
        [SerializeField] private TextMeshProUGUI tempTextDebugLifes;
        [SerializeField] private GameObject _canvasLetterPrefab;
        [SerializeField] private GameObject _canvasPanel;
        [SerializeField] private TMP_InputField _wordInput;
        [SerializeField] private TextMeshProUGUI _currentUserCanvas;
        [SerializeField] private Image _hangImage;
        [SerializeField] private TwitchUser _userToken;
        [SerializeField] private List<Sprite> _hangSrpites;
        [SerializeField] private List<TextMeshProUGUI> _letrasCanvas;
        private List<TwitchUser> _users = new List<TwitchUser>();
        private int stepsDone = 0;
        [Header(header: "Debug")]
        [SerializeField] private int _currentUser = 0;

        public string word { get { return _word; } set { _word = value; } }

        public void OnWordSetted()
        {
            word = _wordInput.text.ToLower();
            SetupGame();
            hangSteps = 10;
            _users.Clear();
            _currentUserCanvas.text = "Usuario: ";
            _hangImage.sprite = _hangSrpites[0];
            TwitchChatSend.SendChat("[Ahorcado] Se ha seleccionado una nueva palabra! Usa !play para unirte.");
        }
        public void SetupGame()
        {
            if (_letrasCanvas.Count != 0)
            {
                foreach (var letter in _letrasCanvas) Destroy(letter.gameObject);
                _letrasCanvas.Clear();
            }
            _wordArray = _word.ToCharArray();
            for (int i = 0; i < _wordArray.Length; i++)
            {
                GameObject letter = Instantiate(
                    _canvasLetterPrefab, _canvasPanel.transform);
                _letrasCanvas.Add(letter.GetComponent<TextMeshProUGUI>());
            }
        }
        public void TryLetter(char letter, TwitchUser user)
        {
            if (hangSteps >= 0) return;
            if (!Char.IsLetter(letter))
            {
                TwitchChatSend.SendChat($"[Ahorcado] {user.DisplayName} ha ingresado un caracter no valido.");
                return;
            }
            if (user.DisplayName != _userToken.DisplayName) return;
            TwitchChatSend.SendChat($"Debug -> Letra {letter}");
            NextTurn();
            int foundCount = 0;
            for (int i = 0; i < _wordArray.Length; i++)
            {
                if (_wordArray[i] == letter)
                {
                    _letrasCanvas[i].text = letter.ToString().ToUpper();
                    foundCount++;
                    stepsDone++;
                }
            }
            if (foundCount == 0)
            {
                hangSteps--;
                tempTextDebugLifes.text = $"Vidas -> {hangSteps}";
                TwitchChatSend.SendChat($"Vidas -> {hangSteps}");
                _hangImage.sprite = _hangSrpites[10 - hangSteps];
                if (hangSteps <= 0)
                {
                    TwitchChatSend.SendChat($"[Ahorcado] Fin del juego! La palabra era: {_word}");
                    return;
                }
            }
            TwitchChatSend.SendChat($"[Ahorcado] {user.DisplayName} ha intentado la letra {letter.ToString().ToUpper()} y ha encontrado {foundCount} letras.");

            if (stepsDone == _wordArray.Length)
            {
                TwitchChatSend.SendChat($"[Ahorcado] Enhorabuena {user.DisplayName}! Has ganado!");
            }
        }
        public void TryPalabra(string triedWord, TwitchUser user)
        {
            if (user.Username != _userToken.Username) return;
            if (triedWord == _word)
            {
                for (int i = 0; i < _letrasCanvas.Count; i++)
                {
                    _letrasCanvas[i].text = _wordArray[i].ToString().ToUpper();
                }
                TwitchChatSend.SendChat($"[Ahorcado] Enhorabuena {user.DisplayName}! Has ganado!");
            }
            else
            {
                hangSteps--;
                tempTextDebugLifes.text = $"Vidas -> {hangSteps}";
                TwitchChatSend.SendChat($"Vidas -> {hangSteps}");
                _hangImage.sprite = _hangSrpites[10 - hangSteps];
                NextTurn();
            }
        }
        public void JoinUser(TwitchUser user)
        {
            if (_users.Count == 0)
            {
                _users.Add(user);
                _userToken = user;
                TwitchChatSend.SendChat($"[Ahorcado] @{user.Username} se ha unido al juego.");
                TwitchChatSend.SendChat($"[Ahorcado] Turno de @{_userToken.Username}");
                _currentUserCanvas.text = $"Usuario: {_userToken.DisplayName}";
                return;
            }
            foreach (var u in _users) if (u.Username == user.Username) return;
            _users.Add(user);
            TwitchChatSend.SendChat($"[Ahorcado] @{user.Username} se ha unido al juego.");
        }
        private void NextTurn()
        {
            Debug.Log("NextTurn");
            Debug.Log($"Usuarios: {_users.Count}");

            if (hangSteps < 0) return;
            _currentUser = (_currentUser + 1) % _users.Count;
            _userToken = _users[_currentUser];
            TwitchChatSend.SendChat($"[Ahorcado] Turno de @{_userToken.Username}");
            _currentUserCanvas.text = $"Usuario: {_userToken.DisplayName}";
            Debug.Log($"Turno: {_currentUser}");
        }
    }
}