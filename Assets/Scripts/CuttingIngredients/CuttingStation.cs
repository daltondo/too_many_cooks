using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingStation : MonoBehaviour
{

    private Transform stationPosition; 
    private bool canCut = false;
    private bool isCutting = false; 
    public GameObject playerCurrObj;
    public GameObject player;

    public PlayerController playerMovement; 

    public float cutTiming;
    public float endAnimationTiming;
    private float cuttingTime = 100f; 
    float originalMove;


    public Animator playerAnim; 

    CuttableIngredient cutScript; // Holds the converted object 
    public bool hasPlayer;

    private void Start()
    {
        
        stationPosition = GetComponent<Transform>();

    }

    void Update()
    {
        if (Input.GetKeyDown("e") && hasPlayer)
        {
            originalMove = playerMovement.moveSpeed; 
            Interact(); 
        }

        //if (Input.GetKey("p") && isCutting && cuttingTime > 0f)
        if (Input.GetKey("j") && isCutting && cuttingTime > 0f)
        {
            Debug.Log("In this loop"); 
            cuttingTime -= 1f;
            Debug.Log(cuttingTime); 
        }

        if (cuttingTime <= 0f)
        {
            Reset();
            cuttingTime = 100f;
            playerMovement.moveSpeed = originalMove;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject coll = collider.gameObject; 
        
        if (coll.CompareTag("Player")) // If the collision that entered is the player... 
        {
            // The cutting station has a player currently there
            hasPlayer = true;
            // Gets the player and it's animator 
            player = coll;
            playerAnim = player.GetComponent<Animator>();
            playerMovement = player.GetComponent<PlayerController>(); 

            if (player.GetComponent<PlayerInteract>().currObj != null) // If the player is holding something - retrieve that item 
            {
                playerCurrObj = player.GetComponent<PlayerInteract>().currObj;
                Debug.Log("The player is holding an object"); 

                if (playerCurrObj.CompareTag("Ingredient"))
                {
                    canCut = playerCurrObj.GetComponent<Ingredient>().isCuttable;
                    cutScript = playerCurrObj.GetComponent<CuttableIngredient>();
                    Debug.Log("The player is holding an ingredient.");
                }
            }
            Debug.Log("The player has just entered");
        }

    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        GameObject coll = collider.gameObject; // I only want to do things if the object leaving is the player itself 

        if (coll.CompareTag("Player"))
        {
            PlayerInteract playerInteract = coll.GetComponent<PlayerInteract>();
            coll.GetComponent<Animator>().SetBool("Cutting", false); 
            Debug.Log("The player has just left"); 
            hasPlayer = false; // We don't have a player 
            playerCurrObj = null;
            player = null;
            canCut = false; 

        }
    }

    private void Interact()
    {
       if (canCut)
        {
            PlayerInteract playerScript = player.GetComponent<PlayerInteract>();
            playerScript.currObj = null;
            Destroy(playerCurrObj); 
            canCut = false;
            isCutting = true; 
            playerAnim.SetBool("Cutting", true);
        }

       if (canCut == false)
        {
            return; 
        }
    }

    private void Reset()
    {
        canCut = true;
        isCutting = false;
        playerAnim.SetBool("Cutting", false);
        Instantiate(cutScript.cuttedIngredient);
    }
}
