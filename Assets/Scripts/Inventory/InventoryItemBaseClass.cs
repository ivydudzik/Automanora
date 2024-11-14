using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// From https://youtu.be/Hj7AZkyojdo?si=VzbXce34wXVJBgGr - to create no inventory item
public class InventoryitemBase : MonoBehaviour, IInventoryItem
{
    public virtual string Name
    {
        get
        {
            return "_base item_";
        }
    }

    public Sprite _Image = null; 
    public Sprite Image
    {
        get
        {
            return _Image;
        }
    }

    public virtual void OnUse()
    {

    }

    public virtual void OnPickup()
    {
        // TODO: Logic for what happens when player picks up this object
        gameObject.SetActive(false);
    }

    public virtual void OnDrop()
    {
        // TODO: Move this logic to a base class or helper method
        
        // Commented implemention: drop on the surface the cursor is at
        
        // RaycastHit hit = new RaycastHit();
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // if(Physics.Raycast(ray, out hit, 1000))
        // {
        //     gameObject.SetActive(true);
        //     gameObject.transform.position = hit.point;
        // }

        Transform playerTransform = Camera.main.transform; 

        Vector3 dropPosition = playerTransform.position + playerTransform.forward * 2f; // 2f: desired drop distance

        gameObject.SetActive(true);
        gameObject.transform.position = dropPosition;

        // If item has a rigid body component, can add a "throwing" effect
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(playerTransform.forward * 5f, ForceMode.VelocityChange); 
        }
    }

}
