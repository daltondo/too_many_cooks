using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCuttingStation : MonoBehaviour
{
    public Vector3 stationPosition;
    public float cutTiming = 100f;

    public GameObject objectBeingCut;
    public CuttableIngredient cutScript;

    public GameObject player;
    public Animator playerAnim;
    public PlayerInteract playerInteract;
    public bool hasPlayer = false;

    public GameObject playerItem;


    void Start()
    {
        stationPosition = GetComponent<Transform>().position;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        player = players[0];
        playerAnim = player.GetComponent<Animator>();
        playerInteract = player.GetComponent<PlayerInteract>(); 
    }

    void Update()
    {
        // Check if the player wants to interact with the object on the cutting board
        if (hasPlayer && Input.GetButtonDown("Interact") && objectBeingCut != null) {
            // Set the player's holding variables
            playerInteract.currObj = objectBeingCut;
            playerInteract.grabbed = true;
            playerInteract.pressedEelsewhere = true;

            // Reset the cutting board variables
            objectBeingCut = null;
            cutScript = null;
            
            return;
        }

        // Check if the player wants to place an ingredient on the cutting board
        if (hasPlayer && Input.GetButtonDown("Interact") && objectBeingCut == null)
        {
            //if (objectBeingCut != null && playerInteract.currObj == null)
            //{
            //    playerInteract.currObj = objectBeingCut;
            //    playerInteract.grabbed = true;
            //    objectBeingCut = null;
            //}

            if (playerInteract.currObj.CompareTag("Ingredient"))
            {
                if (playerInteract.currObj.GetComponent<Ingredient>().isCuttable)
                {
                    PlaceItem(); 
                }
            }
        }

        // Check if the player wants to cut the current ingredient on the cutting board
        if (hasPlayer && objectBeingCut != null && cutTiming > 0)
        {
            if (Input.GetKey("j"))
            {
                Cut();
            }

            else if (!Input.GetKey("j"))
            {
                playerAnim.SetBool("Cutting", false); 
            }
        }

        if (cutTiming <= 0)
        {
            Reset(); 
        }
    }

    private protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Player entered.");
            hasPlayer = true;
        }
    }

    private protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Player exited.");
            hasPlayer = false; 
        }
    }

    private void Interact()
    {

    }

    private void PlaceItem()
    {
        if (playerInteract.currObj != null && playerInteract.currObj.CompareTag("Ingredient"))
        {
            if  (playerInteract.currObj.GetComponent<Ingredient>().isCuttable)
            {
                objectBeingCut = playerInteract.currObj.gameObject;
                objectBeingCut.transform.position = stationPosition;
                cutScript = objectBeingCut.GetComponent<CuttableIngredient>();
                playerInteract.grabbed = false; 
                playerInteract.currObj = null; 
            }
        }
    }

    private void Cut()
    {
        playerAnim.SetBool("Cutting", true);
        cutTiming -= 1; 

    }

    private void Reset()
    {
        playerAnim.SetBool("Cutting", false);
        Instantiate(cutScript.cuttedIngredient, this.transform.position, new Quaternion(0f, 0f, 0f, 0f), this.transform);
        Destroy(objectBeingCut);
        cutScript = null;
        objectBeingCut = null;
        cutTiming = 100f;

    }
}
