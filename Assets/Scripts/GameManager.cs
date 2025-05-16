using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject WinScreen;
    private float timer = 2f; // 1.5 minutes
    private bool gameEnded = false;
    public TMP_Text winText;

    void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            TriggerWin();
        }
    }

    void TriggerWin()
    {
        winText.text = GetComponent<CharacterController2D>().windowScore.ToString();
        gameEnded = true;
        Time.timeScale = 0f; // Optional: freeze gameplay
        WinScreen.SetActive(true);
        Debug.Log("You win! Time completed.");
    }
}
