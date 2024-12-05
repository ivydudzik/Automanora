using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelEndVolume : MonoBehaviour
{
    [SerializeField]
    private string NextScene;
    [SerializeField]
    private SceneController SceneController;
    [SerializeField]
    private LoadManager loadManager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag+" entered end level");
        if (other.CompareTag("Player"))// Only the player can enter the end level transition
        {
            loadManager.SavePositions();
            loadManager.SaveInventory();
            if (NextScene == "StartScene")
            {
                string saveFolderPath = Application.persistentDataPath;

                if (Directory.Exists(saveFolderPath))
                {
                    Directory.Delete(saveFolderPath, true); // 'true' ensures all contents are deleted
                    Debug.Log($"Save folder deleted: {saveFolderPath}");
                }
                else
                {
                    Debug.LogWarning($"Save folder not found: {saveFolderPath}");
                }
            }
            SceneController.ChangeScene(NextScene);
        }
    }
}
