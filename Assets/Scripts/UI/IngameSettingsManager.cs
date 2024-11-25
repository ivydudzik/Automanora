using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameSettingsManager : MonoBehaviour
{
    private float mainVol = 0;
    private float BGMVol = 0;

    private float SFXVol = 0;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.activeSceneChanged += DoSettingOnSceneChange;
        ApplySettings();
    }

    public void DoSettingOnSceneChange(Scene current, Scene next)
    {
        ApplySettings();

    }
    public void ApplySettings()
    {
        var musics = GameObject.FindGameObjectsWithTag("BGM");
        Debug.Log(musics);
        foreach (var src in musics)
        {
            src.GetComponent<AudioSource>().volume = Mathf.Min(mainVol, BGMVol);
        }
        var sounds = GameObject.FindGameObjectsWithTag("SFX");
        foreach (var src in sounds)
        {
            src.GetComponent<AudioSource>().volume = Mathf.Min(mainVol, SFXVol);
        }
    }

    public void SetMainVol(float v)
    {
        mainVol = v;
        Debug.Log(mainVol);
    }
    public void SetMusicVol(float v)
    {
        BGMVol = v;
        Debug.Log(BGMVol);

    }
    public void SetSFXVol(float v)
    {
        SFXVol = v;
        Debug.Log(SFXVol);

    }
}
