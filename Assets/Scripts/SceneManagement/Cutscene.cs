using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject HUD;

    public void EndCutscene()
    {
        Player.SetActive(true); HUD.SetActive(true); gameObject.SetActive(false);
    }
}
