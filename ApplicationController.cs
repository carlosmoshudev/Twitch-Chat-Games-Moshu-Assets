using UnityEngine;

public class ApplicationController : MonoBehaviour 
{
	private void Start() {
        //vsync and frame rate
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        //window and resolution
        Screen.SetResolution(720, 480, false);
    }
}
