using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("StoryboardScene");
    }

    public void LoadSettings()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
