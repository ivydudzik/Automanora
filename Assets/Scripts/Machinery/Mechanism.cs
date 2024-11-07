using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanism : MonoBehaviour
{
    private bool isPowered;
    [SerializeField] bool PlayOnAwake = false;

    void Start()
    {
        if (PlayOnAwake) { Powered = true; }
    }

    void Update()
    {
        if (this.Powered)
        {
            PoweredUpdate();
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
        }
    }

    private void PoweredUpdate()
    {
        throw new NotImplementedException();
    }
}
