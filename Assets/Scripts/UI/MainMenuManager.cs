using UnityEngine;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using UnityEditor.UIElements;
using System.Threading;

public class MainMenuManager : MonoBehaviour
{
    UIDocument menuUIDocument;

    // Menu Container
    VisualElement menuContainer;

    private void Start()
    {
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


    }

    private IEnumerator StartGame()
    {
        yield return StartCoroutine(FBIn());
        // Start the scene in the 2nd spot in the build settings list
        AsyncOperation asyncload = SceneManager.LoadSceneAsync(1);
        while (!asyncload.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator FBIn()
    {
        VisualElement FadeBlocker = menuContainer.Q<VisualElement>("FadeBlocker");
        menuContainer.Q<VisualElement>("FadeBlocker").style.visibility = Visibility.Visible;
        Debug.Log(FadeBlocker.style.backgroundColor.value.a);
        while (FadeBlocker.style.backgroundColor.value.a < 1)
        {
            FadeBlocker.style.backgroundColor = new Color(0, 0, 0, FadeBlocker.style.backgroundColor.value.a + 0.005f);
            yield return new WaitForSeconds(0.01f);
        }
    }
    private IEnumerator FBOut()
    {
        VisualElement FadeBlocker = menuContainer.Q<VisualElement>("FadeBlocker");
        while (FadeBlocker.style.backgroundColor.value.a > 0f)
        {
            FadeBlocker.style.backgroundColor = new Color(0, 0, 0, FadeBlocker.style.backgroundColor.value.a - 0.005f);
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
