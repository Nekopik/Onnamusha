using UnityEngine;
using UnityEngine.SceneManagement;

public class EndgameMenu : MonoBehaviour
{
    public GameObject endgameUI;

    public void LoadEndMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitEndGame()
    {
        Application.Quit();
    }

}
