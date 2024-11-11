using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Movement tutorial from Rytech: https://www.youtube.com/watch?v=TOPj3uHZgQk
public class PlayerMovement : MonoBehaviour
{
    public float MoveSmoothTime;
    public float GravityStrength;
    public float JumpStrength;
    public float WalkSpeed;
    public float RunSpeed;

    public GameObject Hand;

    public Inventory inventory;
    private IInventoryItem nearbyItem;
    private Vector3 originalItemScale; // Store the original item scale
    private bool canPickUp = false;
    private bool pickUpDebugFlag = false;

    private CharacterController Controller;
    private Vector3 CurrentMoveVelocity;
    private Vector3 MoveDampVelocity;

    private Vector3 CurrentForceVelocity;

    private GameObject equippedItem = null;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        inventory.ItemUsed += Inventory_ItemUsed;
        inventory.ItemRemoved += Inventory_ItemRemoved;   
    }

    private void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        IInventoryItem item = e.Item;

        GameObject goItem = (item as MonoBehaviour).gameObject;
        if (equippedItem != null)
        {
            UnequipItem();
        }

        EquipItem(goItem);
    }

    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        IInventoryItem item = e.Item;

        GameObject goItem = (item as MonoBehaviour).gameObject;
        if (goItem == equippedItem)
        {
            UnequipItem(); 
        }
        goItem.transform.position = transform.position + transform.forward * 2f;
        goItem.SetActive(true);
    }

    private void EquipItem(GameObject item)
    {
        item.SetActive(true);
        originalItemScale = item.transform.localScale;
        
        item.transform.parent = Hand.transform;
        item.transform.position = Hand.transform.position;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localScale = Vector3.one; 

        equippedItem = item; 
    }

    private void UnequipItem()
    {
        if (equippedItem != null)
        {
            equippedItem.transform.parent = null;
            equippedItem.transform.localScale = originalItemScale; // Reset to original scale
            equippedItem.SetActive(false);
            
            equippedItem = null; // Clear the equipped item
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupTrigger"))
        {
            // Go up to the parent to find the IInventoryItem component
            IInventoryItem item = other.transform.parent.gameObject.GetComponent<IInventoryItem>();
            
            if (item != null)
            {
                nearbyItem = item;    
                canPickUp = true;

                Debug.Log("nearbyItem has been set to: " + nearbyItem);
            }
            else
            {
                Debug.LogWarning("No IInventoryItem component found on the parent of PickupTrigger.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickupTrigger"))
        {
            IInventoryItem item = other.transform.parent.gameObject.GetComponent<IInventoryItem>();
            if (item != null && item == nearbyItem)
            {
                nearbyItem = null;   
                canPickUp = false;    
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerInput = new Vector3{
            x = Input.GetAxisRaw("Horizontal"), //Use GetAxisRaw to apply custom smoothing
            y = 0f,
            z = Input.GetAxisRaw("Vertical")
        };

        if (PlayerInput.magnitude > 1f){
            PlayerInput.Normalize();
        }

        Vector3 MoveVector = transform.TransformDirection(PlayerInput);
        float CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;    // Hold shift to run

        CurrentMoveVelocity = Vector3.SmoothDamp(
            CurrentMoveVelocity, 
            MoveVector * CurrentSpeed,
            ref MoveDampVelocity,
            MoveSmoothTime
        );

        Controller.Move(CurrentMoveVelocity * Time.deltaTime);  

        Ray groundCheckRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundCheckRay, 1.1f)){
            CurrentForceVelocity.y = -2f;   //constant force appplied to th player for slopes

            if (Input.GetKey(KeyCode.Space)){
                CurrentForceVelocity.y = JumpStrength;
            }
        }
        else{
            CurrentForceVelocity.y -= GravityStrength * Time.deltaTime;
        }

        Controller.Move(CurrentForceVelocity * Time.deltaTime);

        if (canPickUp && Input.GetKeyDown(KeyCode.E))  
        {
            inventory.AddItem(nearbyItem);    // Add the item to the inventory
            canPickUp = false;                // Reset the pickup state
            nearbyItem = null;                // Clear the item reference
            Debug.Log("Item Picked Up");
        }

        // To be implemented
        if (canPickUp)
        {
            // Code to show UI prompt
            // "Press E to pick up"
            if (!pickUpDebugFlag){
                Debug.Log("Press E to Pick Up");
                pickUpDebugFlag = true;
            }
        }
        else
        {
            // Hide the UI prompt if out of range
            pickUpDebugFlag = false;
        }
        
        // unequip key 
        if (Input.GetKeyDown(KeyCode.Q) && equippedItem != null)
        {
            UnequipItem(); 
            //Debug.Log("Hands are now empty.");
        }

    }
}
