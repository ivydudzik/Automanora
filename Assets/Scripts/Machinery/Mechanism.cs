using System;
using System.Collections;
using System.Collections.Generic;
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
        foreach (var batt in batteriesInRange)
        {
            currEnergy += batt.GetComponent<Battery>().DrawPower(requiredEnergy / batteriesInRange.Count);
        }
        Debug.Log(currEnergy);
        Powered = currEnergy >= requiredEnergy;
        Debug.Log("poweredupdate: powered  = " + Powered);
        yield return new WaitForSeconds(1);
        StartCoroutine(PoweredUpdate());
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
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Battery"))
        {
            batteriesInRange.Remove(col.gameObject);
        }
    }

}
