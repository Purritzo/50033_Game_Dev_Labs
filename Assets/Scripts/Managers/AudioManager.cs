using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup bgm1MixerGroup;  // Assign BGM1 group in inspector
    [SerializeField] private AudioMixerGroup bgm2MixerGroup;  // Assign BGM2 group in inspector
    [SerializeField] private string bgm1VolumeParameter = "BGM1Volume";
    [SerializeField] private string bgm2VolumeParameter = "BGM2Volume";
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource1;
    [SerializeField] private AudioSource bgmSource2;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip playerPhaseTheme;
    [SerializeField] private AudioClip enemyPhaseTheme;
    [SerializeField] private AudioClip allyPhaseTheme;

    [Header("Settings")]
    [SerializeField] private float fadeTime = 0.25f;
    [SerializeField] private float maxVolume = 0f;  // in decibels
    [SerializeField] private float minVolume = -80f; // in decibels
    
    private AudioSource currentSource;
    private AudioSource nextSource;
    private string currentVolumeParam;
    private string nextVolumeParam;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        Instance = this;
        
        // Initialize audio sources if not set
        if (bgmSource1 == null)
        {
            bgmSource1 = gameObject.AddComponent<AudioSource>();
            bgmSource1.loop = true;
        }
        if (bgmSource2 == null)
        {
            bgmSource2 = gameObject.AddComponent<AudioSource>();
            bgmSource2.loop = true;
        }

        // Assign mixer groups
        bgmSource1.outputAudioMixerGroup = bgm1MixerGroup;
        bgmSource2.outputAudioMixerGroup = bgm2MixerGroup;

        // Initial setup
        currentSource = bgmSource1;
        nextSource = bgmSource2;
        currentVolumeParam = bgm1VolumeParameter;
        nextVolumeParam = bgm2VolumeParameter;

        // Set initial volumes
        audioMixer.SetFloat(bgm1VolumeParameter, maxVolume);
        audioMixer.SetFloat(bgm2VolumeParameter, minVolume);
    }

    public void ChangeBGMForPhase(string phaseName)
    {
        AudioClip nextClip = null;
        
        switch (phaseName.ToLower())
        {
            case "player phase":
                nextClip = playerPhaseTheme;
                break;
            case "enemy phase":
                nextClip = enemyPhaseTheme;
                break;
            case "ally phase":
                nextClip = allyPhaseTheme;
                break;
        }

        if (nextClip != null && (currentSource.clip != nextClip || !currentSource.isPlaying))
        {
            CrossFadeBGM(nextClip);
        }
    }

    private void CrossFadeBGM(AudioClip nextClip)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(CrossFadeCoroutine(nextClip));
    }

    private IEnumerator CrossFadeCoroutine(AudioClip nextClip)
    {
        // Setup next source
        nextSource.clip = nextClip;
        nextSource.Play();

        float elapsed = 0;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime;

            float currentVolume = Mathf.Lerp(maxVolume, minVolume, t);
            float nextVolume = Mathf.Lerp(minVolume, maxVolume, t);
            
            // Set volumes through mixer using correct parameters
            audioMixer.SetFloat(currentVolumeParam, currentVolume);
            audioMixer.SetFloat(nextVolumeParam, nextVolume);

            yield return null;
        }

        currentSource.Stop();

        // Swap sources and parameters
        (currentSource, nextSource) = (nextSource, currentSource);
        (currentVolumeParam, nextVolumeParam) = (nextVolumeParam, currentVolumeParam);
    }
}