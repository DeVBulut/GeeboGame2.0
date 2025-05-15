using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("ProceduralGenerationScene");
    }

    public void LoadSettings()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
