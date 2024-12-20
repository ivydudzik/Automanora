using UnityEngine;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using TMPro;
using System.IO;


public class MainMenuManager : MonoBehaviour
{
    UIDocument menuUIDocument;

    // Menu Container
    VisualElement menuContainer;
    IngameSettingsManager SettingsManager;
    [SerializeField] GameObject CreditsText;

    private bool optionsVisible = false;

    private void Start()
    {
        SettingsManager = GameObject.FindGameObjectWithTag("SettingsManager").GetComponent<IngameSettingsManager>();
        menuUIDocument = GetComponent<UIDocument>();
        // Get a reference to the UI container
        menuContainer = menuUIDocument.rootVisualElement.contentContainer;
        // Register each button to their functionality

        SetButtonAnimations();
        SetButtonFunctions();

    }
    private void SetButtonAnimations()
    {
        menuContainer.Query(className: "button").ForEach((element) =>
        {
            element.RegisterCallback<MouseMoveEvent>((evt) =>
            {
                //Ref: https://discussions.unity.com/t/quick-transition-tutorial/863467/3
                element.style.unityBackgroundImageTintColor = Color.cyan;
                element.style.transitionProperty = new List<StylePropertyName> { new StylePropertyName("unity-background-image-tint-color") };
                element.style.transitionDuration = new StyleList<TimeValue>(new List<TimeValue> { new TimeValue(0.5f, TimeUnit.Second) });
            });
            element.RegisterCallback<MouseLeaveEvent>((evt) =>
            {
                element.style.unityBackgroundImageTintColor = Color.white;
                element.style.transitionProperty = new List<StylePropertyName> { new StylePropertyName("unity-background-image-tint-color") };
                element.style.transitionDuration = new StyleList<TimeValue>(new List<TimeValue> { new TimeValue(0.2f, TimeUnit.Second) });
            });
        });

    }
    private void SetButtonFunctions()
    {
        menuContainer.Q<Button>("StartB").RegisterCallback<MouseUpEvent>((evt) => StartCoroutine(StartGame()));
        menuContainer.Q<VisualElement>("FadeBlocker").style.visibility = Visibility.Hidden;
        menuContainer.Q<Button>("OptionsB").RegisterCallback<MouseUpEvent>((evt) => StartCoroutine(ToggleOptions()));
        menuContainer.Q<Button>("CreditsB").RegisterCallback<MouseUpEvent>((evt) => StartCoroutine(DoCreditsScene()));
        menuContainer.Q<Button>("DoneB").RegisterCallback<MouseUpEvent>((evt) => StartCoroutine(StopCredits()));
        menuContainer.Q<Button>("ApplyB").RegisterCallback<MouseUpEvent>((evt) => SettingsManager.ApplySettings());


        menuContainer.Q<Slider>("MainVolume").value = 50;
        menuContainer.Q<Slider>("MusicVolume").value = 50;
        menuContainer.Q<Slider>("SFXVolume").value = 50;
        menuContainer.Q<Slider>("MainVolume").RegisterCallback<ChangeEvent<float>>((evt) => { SettingsManager.SetMainVol(menuContainer.Q<Slider>("MainVolume").value); });
        menuContainer.Q<Slider>("MusicVolume").RegisterCallback<ChangeEvent<float>>((evt) => { SettingsManager.SetMusicVol(menuContainer.Q<Slider>("MusicVolume").value); });
        menuContainer.Q<Slider>("SFXVolume").RegisterCallback<ChangeEvent<float>>((evt) => { SettingsManager.SetSFXVol(menuContainer.Q<Slider>("SFXVolume").value); });
        SettingsManager.SetMainVol(50);
        SettingsManager.SetMusicVol(50);
        SettingsManager.SetSFXVol(50);


    }

    private IEnumerator StartGame()
    {
        yield return StartCoroutine(FBIn());
        // Start the scene in the 2nd spot in the build settings list
        string sceneFilePath = Path.Combine(Application.persistentDataPath, "LastScene.txt");
        if (File.Exists(sceneFilePath))
        {
            string lastSceneName = File.ReadAllText(sceneFilePath);
            AsyncOperation asyncload = SceneManager.LoadSceneAsync(lastSceneName);
            while (!asyncload.isDone)
            {
                yield return null;
            }
        }
        else
        {
            AsyncOperation asyncload = SceneManager.LoadSceneAsync(1);
            while (!asyncload.isDone)
            {
                yield return null;
            }
        }
    }
    private IEnumerator DoCreditsScene()
    {
        yield return StartCoroutine(FBIn());
        menuContainer.Q<VisualElement>("MM").style.display = DisplayStyle.None;
        menuContainer.Q<VisualElement>("OPT").style.display = DisplayStyle.None;
        menuContainer.Q<VisualElement>("OptionsB").style.display = DisplayStyle.None;
        menuContainer.Q<VisualElement>("CRED").style.display = DisplayStyle.Flex;
        yield return StartCoroutine(FBOut());
        CreditsText.transform.localPosition = new(0, -480);
        CreditsText.SetActive(true);
    }
    private IEnumerator StopCredits()
    {
        yield return StartCoroutine(FBIn());
        CreditsText.SetActive(false);
        menuContainer.Q<VisualElement>("MM").style.display = DisplayStyle.Flex;
        menuContainer.Q<VisualElement>("OPT").style.display = DisplayStyle.None;
        menuContainer.Q<VisualElement>("OptionsB").style.display = DisplayStyle.Flex;
        menuContainer.Q<VisualElement>("CRED").style.display = DisplayStyle.None;
        yield return StartCoroutine(FBOut());
    }
    private IEnumerator ToggleOptions()
    {
        if (!optionsVisible)
        {
            yield return StartCoroutine(FBIn(0.01f));
            menuContainer.Q<VisualElement>("CRED").style.display = DisplayStyle.None;
            menuContainer.Q<VisualElement>("OptionsB").style.display = DisplayStyle.Flex;
            menuContainer.Q<VisualElement>("MM").style.display = DisplayStyle.None;
            menuContainer.Q<VisualElement>("OPT").style.display = DisplayStyle.Flex;
            yield return StartCoroutine(FBOut(0.01f));
        }
        else
        {
            yield return StartCoroutine(FBIn(0.01f));
            menuContainer.Q<VisualElement>("CRED").style.display = DisplayStyle.None;
            menuContainer.Q<VisualElement>("OptionsB").style.display = DisplayStyle.Flex;
            menuContainer.Q<VisualElement>("MM").style.display = DisplayStyle.Flex;
            menuContainer.Q<VisualElement>("OPT").style.display = DisplayStyle.None;
            yield return StartCoroutine(FBOut(0.01f));
        }
        optionsVisible = !optionsVisible;
    }

    private IEnumerator FBIn(float rate = 0.005f)
    {
        VisualElement FadeBlocker = menuContainer.Q<VisualElement>("FadeBlocker");
        menuContainer.Q<VisualElement>("FadeBlocker").style.visibility = Visibility.Visible;
        Debug.Log(FadeBlocker.style.backgroundColor.value.a);
        while (FadeBlocker.style.backgroundColor.value.a < 1)
        {
            FadeBlocker.style.backgroundColor = new Color(0, 0, 0, FadeBlocker.style.backgroundColor.value.a + rate);
            yield return new WaitForSeconds(0.01f);
        }
    }
    private IEnumerator FBOut(float rate = 0.005f)
    {
        VisualElement FadeBlocker = menuContainer.Q<VisualElement>("FadeBlocker");
        while (FadeBlocker.style.backgroundColor.value.a > 0f)
        {
            FadeBlocker.style.backgroundColor = new Color(0, 0, 0, FadeBlocker.style.backgroundColor.value.a - rate);
            yield return new WaitForSeconds(0.01f);
        }
        menuContainer.Q<VisualElement>("FadeBlocker").style.visibility = Visibility.Hidden;
    }

    public void Quit()
    {
        // You know what this means
        Application.Quit();
    }
}
