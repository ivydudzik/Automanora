using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Movement tutorial from Rytech: https://www.youtube.com/watch?v=TOPj3uHZgQk
public class PlayerMovement : MonoBehaviour
{
    public float MoveSmoothTime;
    public float GravityStrength;
    public float JumpStrength;
    public float WalkSpeed;
    public float RunSpeed;

    public GameObject Hand;
    public GameObject InventoryObjectPrefab;

    public Inventory inventory;
    private IInventoryItem nearbyItem;
    private Vector3 originalItemScale; // Store the original item scale
    private bool canPickUp = false;
    private bool pickUpDebugFlag = false;
    private bool isGrounded = false;

    private CharacterController Controller;
    private Vector3 CurrentMoveVelocity;
    private Vector3 MoveDampVelocity;

    private Vector3 CurrentForceVelocity;

    private GameObject equippedItem = null;
    public LoadManager loadManager;

    ParticleSystem movementParticles;
    public int particleCountWalk = 5;
    public int particleCountRun = 10;
    private float footstepInterval = 0.5f; // Walking interval
    private float runningFootstepInterval = 0.3f; // Running interval

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        inventory.ItemUsed += Inventory_ItemUsed;
        inventory.ItemRemoved += Inventory_ItemRemoved;

        movementParticles = GetComponent<ParticleSystem>();
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

    public void setInventory(int count)
    {
        Debug.LogWarning("Setting inventory to " + count);
        for (int i = 0; i < count; i++)
        {
            // Instantiate a new inventory item in front of the player
            Instantiate(InventoryObjectPrefab, transform.position + transform.forward * 2f, Quaternion.identity);
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
        float currentInterval = Input.GetKey(KeyCode.LeftShift) ? runningFootstepInterval : footstepInterval;

        if (PlayerInput.magnitude > 0.1f && isGrounded) 
        {
            // Adjust pitch for walking or running
            float pitch = Input.GetKey(KeyCode.LeftShift) ? 1.5f : 1.0f;
            AudioManager.Instance.SetPitch("WalkRun", pitch);

            // Start looping sound if not already playing
            AudioManager.Instance.PlayLoopingSound("WalkRun");
        }
        else
        {
            // Stop the sound when stationary
            AudioManager.Instance.StopSound("WalkRun");
        }


        //particle handling
        
        //old rotation code. I couldn't find a proper way to make it face the opposite direction in a short amount of time, so I modified it to just spawn in a circle area with no velocity instead
        /*
        var myShape = movementParticles.shape;
        //Set the rotation based on the angle determined by the moveVector
        //myShape.rotation = new Vector3(90,0,Mathf.Atan((MoveVector.z/(MoveVector.x==0 ? 1 : MoveVector.x)))*180/Mathf.PI);
        //myShape.rotation = -1 * MoveVector * 360;
        //myShape.rotation = new Vector3(90, -1 * MoveVector.x * 360);
        myShape.rotation = new Vector3(90,Mathf.Atan((MoveVector.z/(MoveVector.x==0 ? 1 : MoveVector.x)))*180/Mathf.PI,0);
        Debug.Log(MoveVector.magnitude);
        */
        var myEmission = movementParticles.emission;
        if (MoveVector.magnitude < 0.5f)
            myEmission.rateOverTime = 0;
        else
        {
            if(!Input.GetKey(KeyCode.LeftShift)) //not running, ie walking
                myEmission.rateOverTime = particleCountWalk;
            else
                myEmission.rateOverTime = particleCountRun;
        }


        CurrentMoveVelocity = Vector3.SmoothDamp(
            CurrentMoveVelocity, 
            MoveVector * CurrentSpeed,
            ref MoveDampVelocity,
            MoveSmoothTime
        );

        Controller.Move(CurrentMoveVelocity * Time.deltaTime);  

        Ray groundCheckRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundCheckRay, 1.1f)){
            //movementParticles.Play(); //on ground, run particles
            isGrounded = true;
            CurrentForceVelocity.y = -2f;   //constant force appplied to th player for slopes

            if (Input.GetKey(KeyCode.Space)){
                
                AudioManager.Instance.PlaySound("Jump"); 
                CurrentForceVelocity.y = JumpStrength;
            }
        }
        else{
            isGrounded = false;
            CurrentForceVelocity.y -= GravityStrength * Time.deltaTime;

            myEmission.rateOverTime = 0; //not on ground, stop particles. overrides previous lines setting rate
        }

        Controller.Move(CurrentForceVelocity * Time.deltaTime);

        if (canPickUp && Input.GetKeyDown(KeyCode.R))  
        {
            inventory.AddItem(nearbyItem);    // Add the item to the inventory
            canPickUp = false;                // Reset the pickup state
            nearbyItem = null;                // Clear the item reference
            AudioManager.Instance.PlaySound("BatteryPickup"); // Play the pickup sound
            Debug.Log("Item Picked Up");
        }

        // To be implemented
        if (canPickUp)
        {
            // Code to show UI prompt
            // "Press E to pick up"
            if (!pickUpDebugFlag){
                Debug.Log("Press R to Pick Up");
                pickUpDebugFlag = true;
            }
        }
        else
        {
            // Hide the UI prompt if out of range
            pickUpDebugFlag = false;
        }

        //* MANUAL SCENE CHANGE TESTING
        if (Input.GetKeyDown(KeyCode.O))
        {
            loadManager.SavePositions();
            loadManager.SaveInventory();
            Debug.Log("error");
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            loadManager.SavePositions();
            loadManager.SaveInventory();
            Debug.Log("error");
            SceneManager.LoadScene(2);
        }
        //*/

    }
}
