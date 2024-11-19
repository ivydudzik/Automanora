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
        aud.volume = 0.0f;
        StartCoroutine(IntroToFull());
    }

    IEnumerator IntroToFull()
    {
        while (aud.isPlaying)
        {   yield return new WaitForSeconds(0.01f);
            aud.volume = Mathf.Clamp(aud.volume + 0.001f, 0, 0.3f);
            yield return null;
        }
        aud.clip = fullSong;
        aud.loop = true;
        aud.Play();
    }


}
