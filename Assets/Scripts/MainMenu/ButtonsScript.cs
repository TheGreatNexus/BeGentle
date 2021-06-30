using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("ScaleMap", LoadSceneMode.Single);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
