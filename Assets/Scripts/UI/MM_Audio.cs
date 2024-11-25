using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Audio : MonoBehaviour
{
    private AudioSource aud;
    [SerializeField] AudioClip fullSong;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.volume = 0.5f;
        StartCoroutine(IntroToFull());
    }

    IEnumerator IntroToFull()
    {
        while (aud.isPlaying)
        {
            yield return null;
        }
        aud.clip = fullSong;
        aud.loop = true;
        aud.Play();
    }


}
