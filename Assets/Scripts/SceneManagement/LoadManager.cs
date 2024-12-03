using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;



[System.Serializable]
public class ObjectData
{
    public string name;       // The name of the object
    public Vector3 position;  // Position of the object
    public Vector3 rotation;  // Rotation of the object

    public ObjectData(string name, Vector3 position, Vector3 rotation)
    {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
    }
}

public class LoadManager : MonoBehaviour
{
    public List<GameObject> objectsToSave; // Assign objects to save in the Inspector
    private string baseFilePath;

    private void Awake()
    {
        // Set the base file path (scene-specific path is derived from this)
        baseFilePath = Path.Combine(Application.persistentDataPath, "SceneData_");

        // Load positions for the current scene
        LoadPositions();
    }

    public void SavePositions()
    {
        List<ObjectData> dataList = new List<ObjectData>();

        foreach (GameObject obj in objectsToSave)
        {
            if (obj != null)
            {
                ObjectData data = new ObjectData(
                    obj.name,
                    obj.transform.position,
                    obj.transform.eulerAngles // Save rotation as Euler angles
                );
                dataList.Add(data);
            }
        }

        string json = JsonUtility.ToJson(new Wrapper<List<ObjectData>> { Data = dataList }, true);
        File.WriteAllText(GetSceneFilePath(), json);

        Debug.Log("Positions saved to " + GetSceneFilePath());
    }

    private void LoadPositions()
    {
        string sceneFilePath = GetSceneFilePath();
        if (File.Exists(sceneFilePath))
        {
            string json = File.ReadAllText(sceneFilePath);
            Wrapper<List<ObjectData>> wrapper = JsonUtility.FromJson<Wrapper<List<ObjectData>>>(json);

            foreach (ObjectData data in wrapper.Data)
            {
                // Find the object by name and apply saved transform data
                GameObject obj = objectsToSave.Find(o => o.name == data.name);

                if (obj != null)
                {
                    obj.transform.position = data.position;
                    obj.transform.eulerAngles = data.rotation;
                }
                else
                {
                    Debug.LogWarning("Object with name " + data.name + " not found in the scene.");
                }
            }

            Debug.Log("Positions loaded from " + sceneFilePath);
        }
        else
        {
            Debug.LogWarning("Save file not found at " + sceneFilePath + ". Using default positions.");
        }
    }

    private string GetSceneFilePath()
    {
        // Include scene name in the file name for scene-specific saving
        string sceneName = SceneManager.GetActiveScene().name;
        return baseFilePath + sceneName + ".json";
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T Data;
    }
}
