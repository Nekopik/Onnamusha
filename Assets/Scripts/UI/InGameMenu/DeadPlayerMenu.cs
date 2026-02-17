using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadPlayerMenu : MonoBehaviour
{
    [SerializeField] Boss boss;
    [SerializeField] TMP_Text modifierText;
    [SerializeField] TMP_Text skillText;

    public GameObject deadPlayerMenu;
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

    public void LoadEndMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitEndGame()
    {
        Application.Quit();
    }

    void Update()
    {
        float multiplier = boss.entityData.damageMultiplier;
        modifierText.text = multiplier.ToString("F2");

        if (multiplier >= 0.8f && multiplier < 0.9f)
        {
            skillText.text = "bad";
            skillText.color = new Color32(170, 170, 170, 255);
        }
        else if (multiplier >= 0.9f && multiplier < 1.2f)
        {
            skillText.text = "decent";
            skillText.color = new Color32(75, 210, 75, 255);
        }
        else if (multiplier >= 1.2f && multiplier < 1.85f)
        {
            skillText.text = "good";
            skillText.color = new Color32(0, 160, 255, 255);
        }
        else if (multiplier >= 1.85f && multiplier <= 2f)
        {
            skillText.text = "excellent";
            skillText.color = new Color32(255, 128, 0, 255);
        }
        else
        {
            skillText.text = "what";
        }
    }
}
