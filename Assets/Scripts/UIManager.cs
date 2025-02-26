using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PausePanel; 
    public Slider AudioSlider;
    public GameObject DeathPanel;
    public Slider musicSlider;
    public Slider effectsSlider; 

    void Start()
    {
        // DeathPanel.SetActive(false); 
        // PausePanel.SetActive(true);  

        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        float effectVolume = PlayerPrefs.GetFloat("EffectVolume", 1.0f);

        // Set slider values
        if (musicSlider != null)
            musicSlider.value = musicVolume;

        if (effectsSlider != null)
            effectsSlider.value = effectVolume; 
    }

    void Update()
    {   
        HandleSettingsPanel();
    }

    void HandleSettingsPanel()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && PausePanel != null)
        {
            if(PausePanel.activeSelf == true)
            {
                PausePanel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                PausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

}
