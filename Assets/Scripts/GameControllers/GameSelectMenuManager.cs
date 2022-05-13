using System;
using UnityEngine;

[Serializable]
public class GameSelectMenuManager : MonoBehaviour 
{
    [SerializeField] GlobalVariables _globalVariables;
	[SerializeField] GameObject _ahorcadoPanel;

    public void StartAhorcado()
    {
        this.gameObject.SetActive(value: false);
        _ahorcadoPanel.SetActive(value: true);
        _globalVariables.gameSelected = GameSelected.Ahorcado;
    }
}
