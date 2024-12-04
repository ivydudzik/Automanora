using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Animations;
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

    void Start()
    {
        animator = GetComponent<Animator>();
        batteriesInRange = new List<GameObject>();
        if (!animator) { throw new Exception("Mechanism was not provided with an animator!"); }
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
            isPowered = value;
            animator.SetBool("Powered", value);
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
