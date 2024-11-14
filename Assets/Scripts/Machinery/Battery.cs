using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : InventoryitemBase
{
    public int maxCharge = 1000;
    public int flowCap = 50;
    public int currCharge = 0;

    void Start(){
        Recharge(maxCharge);
    }
    void Recharge(int amount){
        currCharge = Math.Clamp(currCharge + amount, 0, maxCharge);
    }
    public int DrawPower(int amount){
        var adj = amount > flowCap ? flowCap : amount;
        currCharge -= adj;
        return adj;
    }

        public override string Name
    {
        get
        {
            return "Battery Backpack";
        }
    }

    public override void OnUse()
    {
        base.OnUse();
    }
    
}
