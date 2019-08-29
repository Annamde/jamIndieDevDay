using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menuCanvas, tutorialCanvas;

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    public void EnableInstructions()
    {
        tutorialCanvas.SetActive(true);
        menuCanvas.SetActive(false);
    }

    public void EnableMainMenu()
    {
        menuCanvas.SetActive(true);
        tutorialCanvas.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
