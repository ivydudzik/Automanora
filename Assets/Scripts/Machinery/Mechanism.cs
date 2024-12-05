using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanism : MonoBehaviour
{
    private bool isPowered;
    private List<GameObject> batteriesInRange;
    private int currEnergy = 0;
    [SerializeField] int requiredEnergy;
    private Animator animator;
    public bool repeats = false;
    public bool hasExitAnim = false;
    public bool pauseOnExit = false;

    [SerializeField] private string powerOnSound;  // Sound when powered on
    [SerializeField] private string powerOffSound; // Sound when powered off
    [SerializeField] private string activeLoopSound; // Optional looping sound during operation

    void Start()
    {
        animator = GetComponent<Animator>();
        batteriesInRange = new List<GameObject>();
        if (!animator) { throw new Exception("Mechanism was not provided with an animator!"); }
        
        // Assign default sound names if not set in Inspector
        if (string.IsNullOrEmpty(powerOnSound))
        {
            powerOnSound = "DefaultPowerOn"; // Replace with your actual default sound name
        }
        if (string.IsNullOrEmpty(powerOffSound))
        {
            powerOffSound = "DefaultPowerOff"; // Replace with your actual default sound name
        }
        
        animator.SetBool("Repeats", repeats);
        animator.SetBool("HasExitAnim", hasExitAnim);
        StartCoroutine(PoweredUpdate());
    }

    IEnumerator PoweredUpdate()
    {
        currEnergy = Math.Clamp(currEnergy - requiredEnergy, 0, int.MaxValue);
        PurgeInactive();
        foreach (var batt in batteriesInRange)
        {
            currEnergy += batt.GetComponent<Battery>().DrawPower(requiredEnergy / batteriesInRange.Count);
        }

        Debug.Log(currEnergy);
        Powered = currEnergy >= requiredEnergy;
        Debug.Log("poweredupdate: powered  = " + Powered);
        if(pauseOnExit)
        {
            animator.speed = (Powered ? 1 : 0); //reg speed if powered, 0 if not
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(PoweredUpdate());
    }

    private void PurgeInactive()
    {
        List<GameObject> inactive = new();
        foreach (var batt in batteriesInRange)
        {
            if (!batt.activeInHierarchy)
            {
                inactive.Add(batt);
            }
        }
        foreach (var batt in inactive){
            batteriesInRange.Remove(batt);
        }
    }
    public bool Powered
    {
        get
        {
            return isPowered;
        }
        set
        {
            if (isPowered != value) // Only trigger audio if the state changes
            {
                isPowered = value;
                animator.SetBool("Powered", value);

                if (isPowered)
                {
                    // Play power-on sound
                    AudioManager.Instance.PlaySound(powerOnSound);

                    // Start a looping sound if specified
                    if (!string.IsNullOrEmpty(activeLoopSound))
                    {
                        AudioManager.Instance.PlayLoopingSound(activeLoopSound);
                    }
                }
                else
                {
                    // Stop looping sound
                    if (!string.IsNullOrEmpty(activeLoopSound))
                    {
                        AudioManager.Instance.StopSound(activeLoopSound);
                    }

                    // Play power-off sound
                    AudioManager.Instance.PlaySound(powerOffSound);
                }
            }
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Battery"))
        {
            batteriesInRange.Add(col.gameObject);
            //activate particle effects
            col.gameObject.GetComponent<ParticleSystem>().Play();
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Battery"))
        {
            batteriesInRange.Remove(col.gameObject);
            //de-activate particle effects
            col.gameObject.GetComponent<ParticleSystem>().Stop();
        }
    }

}
