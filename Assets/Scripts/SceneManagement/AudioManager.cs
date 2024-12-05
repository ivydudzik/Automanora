using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class SoundEffect
    {
        public string name;      
        public AudioClip clip;   
    }

    public List<SoundEffect> soundEffects; 

    private Dictionary<string, AudioClip> soundDictionary;
    private Dictionary<string, AudioSource> activeAudioSources; // Active AudioSources for interruptible playback

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        soundDictionary = new Dictionary<string, AudioClip>();
        activeAudioSources = new Dictionary<string, AudioSource>();

        foreach (var sound in soundEffects)
        {
            if (!soundDictionary.ContainsKey(sound.name))
            {
                soundDictionary.Add(sound.name, sound.clip);
            }
        }
    }

    // Play a sound once (OneShot, non-interruptible)
    public void PlaySound(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            AudioSource tempSource = gameObject.AddComponent<AudioSource>();
            tempSource.clip = soundDictionary[name];
            tempSource.PlayOneShot(soundDictionary[name]);
            Destroy(tempSource, soundDictionary[name].length); // Clean up after playback
        }
        else
        {
            Debug.LogWarning($"Sound '{name}' not found in AudioManager!");
        }
    }

    // Play a looping or interruptible sound
    public void PlayLoopingSound(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            if (!activeAudioSources.ContainsKey(name)) // Prevent multiple instances of the same sound
            {
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = soundDictionary[name];
                newSource.loop = true;
                newSource.Play();
                activeAudioSources[name] = newSource;
            }
        }
        else
        {
            Debug.LogWarning($"Sound '{name}' not found in AudioManager!");
        }
    }

    // Stop a looping or interruptible sound
    public void StopSound(string name)
    {
        if (activeAudioSources.ContainsKey(name))
        {
            AudioSource source = activeAudioSources[name];
            source.Stop();
            Destroy(source); // Clean up the AudioSource
            activeAudioSources.Remove(name);
        }
    }

    // Adjust pitch for a looping sound
    public void SetPitch(string name, float pitch)
    {
        if (activeAudioSources.ContainsKey(name))
        {
            activeAudioSources[name].pitch = pitch;
        }
    }
}
