using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndVolume : MonoBehaviour
{
    [SerializeField]
    private string NextScene;
    [SerializeField]
    private SceneController SceneController;
    
    private void OnTriggerEnter(Collider other)
    {
        SceneController.ChangeScene(NextScene);
    }
}
