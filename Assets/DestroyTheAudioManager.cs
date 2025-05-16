using UnityEngine;

public class DestroyTheAudioManager : MonoBehaviour
{
    public void KillAudioManager()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            Destroy(audioManager.gameObject);
        }
    }

}
