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
        foreach (var src in musics)
        {
            src.GetComponent<AudioSource>().volume = Mathf.Min(mainVol, BGMVol) / 100;
        }
        var sounds = GameObject.FindGameObjectsWithTag("SFX");
        foreach (var src in sounds)
        {
            src.GetComponent<AudioSource>().volume = Mathf.Min(mainVol, SFXVol) / 100;
        }
    }

    public void SetMainVol(float v)
    {
        mainVol = v;
    }
    public void SetMusicVol(float v)
    {
        BGMVol = v;
    }
    public void SetSFXVol(float v)
    {
        SFXVol = v;
    }
}
