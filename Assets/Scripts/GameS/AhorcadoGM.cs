using System;
using UnityEngine;

[Serializable]
public class AhorcadoGM : MonoBehaviour 
{
    [SerializeField] private string _word = "test";
    private char[] _wordArray;
    private int hangSteps = 10;
    [SerializeField] private TMPro.TextMeshProUGUI tempTextDebugLifes;
    [SerializeField] private GameObject _canvasLetterPrefab;
    [SerializeField] private GameObject _canvasPanel;
    [SerializeField] private TMPro.TMP_InputField _wordInput;
    [SerializeField] System.Collections.Generic.List<TMPro.TextMeshProUGUI> _letrasCanvas;
    [SerializeField] System.Collections.Generic.List<Sprite> _hangSrpites;
    [SerializeField] UnityEngine.UI.Image _hangImage;
    private int stepsDone = 0;

    public string word { get { return _word; } set { _word = value; } }

    public void OnWordSetted()
    {
        word = _wordInput.text;
        SetupGame();
        TwitchChatSend.SendChat("Debug -> New word setted");
    }
    public void SetupGame() 
    {
        if(_letrasCanvas.Count != 0)
        {
            foreach(var letter in _letrasCanvas)
            {
                Destroy(letter.gameObject);
            }
            _letrasCanvas.Clear();
        }
        _wordArray = _word.ToCharArray();
        for (int i = 0; i < _wordArray.Length; i++)
        {
            GameObject letter = Instantiate(
                _canvasLetterPrefab, _canvasPanel.transform);
                _letrasCanvas.Add(letter.GetComponent<TMPro.TextMeshProUGUI>());
        }
    }
	public void TryLetter(char letter) 
    {
        TwitchChatSend.SendChat($"Debug -> Letra {letter}");
        int foundCount = 0;
        for(int i = 0; i < _wordArray.Length; i++)
        {
            if(_wordArray[i] == letter)
            {
                _letrasCanvas[i].text = letter.ToString().ToUpper();
                foundCount++;
                stepsDone++;
            }    
        }
        if(foundCount == 0) 
        {
            hangSteps--;
            tempTextDebugLifes.text = $"Vidas -> {hangSteps}";
            TwitchChatSend.SendChat($"Vidas -> {hangSteps}");
            _hangImage.sprite = _hangSrpites[10 - hangSteps];
            if(hangSteps !< 0)
            {
                TwitchChatSend.SendChat($"Debug -> Se ha perdido el juego");
            }
        }
        TwitchChatSend.SendChat($"Debug -> Se ha encontrado la letra {foundCount} veces");
        if(stepsDone == _wordArray.Length)
        {
            TwitchChatSend.SendChat($"Debug -> Se ha ganado el juego");
        }   
    }
    public void TryPalabra(string triedWord)
    {
        TwitchChatSend.SendChat($"Debug -> Palabra {triedWord}");
        if(triedWord == _word)
        {
            for(int i = 0; i < _letrasCanvas.Count; i++)
            {
                _letrasCanvas[i].text = _wordArray[i].ToString().ToUpper();
            }
            TwitchChatSend.SendChat($"Debug -> Se ha ganado el juego");
        }
        else
        {
            hangSteps--;
            tempTextDebugLifes.text = $"Vidas -> {hangSteps}";
            TwitchChatSend.SendChat($"Vidas -> {hangSteps}");
            _hangImage.sprite = _hangSrpites[10 - hangSteps];
        }
    }
}
