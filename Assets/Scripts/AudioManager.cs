using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider; 
    public Slider effectsSlider;
    public static AudioManager instance;
    private float musicVolume = 1f; 
    private float effectVolume = 1f;

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
        Transform parent = GameObject.Find("Canvas").transform;
        musicSlider = parent.transform.GetChild(1).transform.GetChild(2).gameObject.GetComponent<Slider>();
        effectsSlider = parent.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Slider>();


        // Load saved values or use default (1.0f if not set before)
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        effectVolume = PlayerPrefs.GetFloat("EffectVolume", 1.0f);

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

        // Apply loaded volume levels
        ApplyVolume();
    }

    void FixedUpdate()
    {
        OnSoundValueChanged();
    }

    public void OnSoundValueChanged()
    {
        if(musicSlider != null && effectsSlider != null)
        {
            // Update volumes from sliders
            musicVolume = musicSlider.value;
            effectVolume = effectsSlider.value;

            // Save values to PlayerPrefs
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("EffectVolume", effectVolume);
            PlayerPrefs.Save(); // Ensure data is written

            // Apply new volume levels
            ApplyVolume();
            audioMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
            audioMixer.SetFloat("Effects", Mathf.Log10(effectVolume) * 20);
        } 
        else
        {
             Transform parent = GameObject.Find("Canvas").transform;
             musicSlider = parent.transform.GetChild(1).transform.GetChild(2).gameObject.GetComponent<Slider>();
             effectsSlider = parent.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Slider>();
        }
    }

    private void ApplyVolume()
    {
        // Convert linear slider value to logarithmic volume scale (for audio mixer)
    }
}
