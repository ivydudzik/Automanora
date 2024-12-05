using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CreditsTextScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new(0, 30f*Time.deltaTime, 0), Space.Self);
    }
}
