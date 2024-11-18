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
        // Get a reference to the UI container
        menuContainer = menuUIDocument.rootVisualElement.contentContainer;
        // Register each button to their functionality
        Button startButton = menuContainer.Q<Button>("StartB");
        startButton.RegisterCallback<MouseUpEvent>((evt) => StartGame());
    }

    public void StartGame()
    {
        // Start the scene in the 2nd spot in the build settings list
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        // You know what this means
        Application.Quit();
    }
}
