using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{
    void Start(){
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }
    public void PlayButton()
    {
        SceneManager.LoadScene("ScaleMap", LoadSceneMode.Single);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
