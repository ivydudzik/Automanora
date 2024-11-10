using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Mechanism : MonoBehaviour
{
    private bool isPowered;
    private Animator animator;
    [SerializeField] bool PlayOnAwake = false;
    public bool repeats = false;
    public bool bounceMode = false;
    public bool hasExitAnim = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (!animator) { throw new Exception("Mechanism was not provided with an animator!"); }
        animator.Play("OFF");
        if (PlayOnAwake) { Powered = true; }
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
            if (value)
            {
                if (bounceMode)
                {
                    animator.Play("ON_BOUNCE");
                    return;
                }
                if (repeats)
                {
                    animator.Play("ON_REPEAT");
                }
                else
                {
                    animator.Play("ON_SINGLE");
                }
            } else {
                if (hasExitAnim){
                    animator.Play("ON_EXITING");
                } else {
                    animator.Play("OFF");
                }
            }

        }
    }
}
