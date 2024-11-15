using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] UIDocument menuUIDocument;

    // Menu Container
    VisualElement menuContainer;

    private void Start()
    {
        menuContainer = menuUIDocument.rootVisualElement.contentContainer;
        Button startButton = menuContainer.Q<Button>("StartB");
        startButton.RegisterCallback<MouseUpEvent>((evt) => StartGame());
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
