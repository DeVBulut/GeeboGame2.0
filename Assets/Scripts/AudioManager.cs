using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider effectsSlider;
    public Slider musicSlider; 
    public Slider buttonVolumeSlider; 
    public static AudioManager instance;
    private float effectVolume = 0.3f;
    private float musicVolume = 0.3f; 
    private float buttonEffectVolume = 1f; 
    [SerializeField] private bool isMainMenuAudioManager = false; 
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if(!isMainMenuAudioManager)
        {
            Transform parent = GameObject.Find("Canvas").transform;
            musicSlider = parent.transform.GetChild(1).transform.GetChild(2).gameObject.GetComponent<Slider>();
            effectsSlider = parent.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Slider>();
        }


        // Load saved values or use default (1.0f if not set before)
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
        effectVolume = PlayerPrefs.GetFloat("EffectVolume", 0.3f);
        buttonEffectVolume = PlayerPrefs.GetFloat("ButtonEffectVolume", 1f);

        //musicSlider = GameObject.FindGameObjectWithTag("MusicSlider").gameObject.GetComponent<Slider>();
        //effectsSlider = GameObject.FindGameObjectWithTag("EffectsSlider").gameObject.GetComponent<Slider>();

        // Set slider values
        if(musicSlider != null)
        {
            musicSlider.value = musicVolume;
        }
        if(effectsSlider != null)
        {
            effectsSlider.value = effectVolume;
        }
        if(buttonVolumeSlider != null)
        {
            buttonVolumeSlider.value = buttonEffectVolume;
        }

    }

    void FixedUpdate()
    {
        OnSoundValueChanged();
    }

    public void OnSoundValueChanged()
    {
        if (musicSlider != null && effectsSlider != null)
        {
            // Update volumes from sliders
            musicVolume = musicSlider.value;
            effectVolume = effectsSlider.value;
            buttonEffectVolume = buttonVolumeSlider.value;

            // Save values to PlayerPrefs
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("EffectVolume", effectVolume);
            PlayerPrefs.SetFloat("ButtonEffectVolume", buttonEffectVolume);
            PlayerPrefs.Save(); // Ensure data is written

            // Apply volume changes while handling zero values
            audioMixer.SetFloat("Music", (musicVolume > 0) ? Mathf.Log10(musicVolume) * 20 : -80);
            audioMixer.SetFloat("Effects", (effectVolume > 0) ? Mathf.Log10(effectVolume) * 20 : -80);
            audioMixer.SetFloat("UI", (buttonEffectVolume > 0) ? Mathf.Log10(buttonEffectVolume) * 20 : -80);
        } 
        else
        {
            Debug.LogWarning("Sliders are not found");
        }
    }

    public void PlayButtonSound()
    {

    }

}
