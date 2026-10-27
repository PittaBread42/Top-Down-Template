using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static AudioSource randomPitchAudioSource;
    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            AudioSouce[] audioSources = GetComponents<AudioSource>();
            audioSource = audioSources[0];
            randomPitchAudioSource = audioSources[1];
            soundEffectLibrary = GetComponent<SoundEffectLibrary>():
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
  
    public static void Play(string soundName, bool randomPitch = false)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if(audioClip != null)
        {
            if(randomPitch)
            {
                randomPitchAudioSource = Random.Range(1f, 1.5f);
                randomPitchAudioSource.PlayOneShot(audioClip);
            }
            else
            {
                audioSource.PlayOneShot(audioClip);
            }
            
        }
    }

    void Start() 
    {
        sfxSlider.OnValueChanged.AddListener(delegate {onValueChanged();});
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
        randomPitchAudioSource.volume = volume;
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}
