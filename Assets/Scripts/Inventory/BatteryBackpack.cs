using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// From https://youtu.be/Hj7AZkyojdo?si=VzbXce34wXVJBgGr - to create no inventory item
public class BatteryBackpack : InventoryitemBase
{
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
