using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThirdCopy : MonoBehaviour
{
    public Vector3 stationPosition;
    public bool IsObjectCuttable;
    public GameObject ObjectBeingCut;
    public CuttableIngredient ObjectBeingCutScript;

    public GameObject player;
    public Animator playerAnimator;
    public PlayerInteract playerInteractScript;

    public bool hasPlayer;
    public GameObject playerObject;

    void Start()
    {
        /** Instantiations. */
        GameObject[] allThings = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < allThings.Length; i++)
        {
            if (allThings[i].GetComponent<PlayerInteract>() != null)
            {
                player = allThings[i];
                break;
            }
        }

        stationPosition = GetComponent<Transform>().position;
        playerAnimator = player.GetComponent<Animator>();
        playerInteractScript = player.GetComponent<PlayerInteract>();
    }

    void Update()
    {
        /** Constantly check what the player is holding. */
        CheckPlayerItem();

        /** Constantly check if the player's current object is cuttable.*/
        IsObjectCuttable = CheckIfCuttable();

        /** Check if the player inputs the interact button and they are currently near this cutting board*/
        if (Input.GetButtonDown("Interact") && hasPlayer)
        {
            /** Check if there's currently an object, and if the player is not currently holding something.*/
            if (playerInteractScript.grabbed == false && ObjectBeingCut != null)
            {
                Debug.Log("We've entered this loop!!!");
                playerInteractScript.pressedEelsewhere = true;
                playerInteractScript.grabbed = true;
                ObjectBeingCut = null;
                ObjectBeingCutScript = null;
                playerInteractScript.cutTiming = 100f;
            }

            /** Check if the object is a cuttable object. */
            if (IsObjectCuttable)
            {
                /** Check if there is currently an object on the board. */
                if (ObjectBeingCut == null)
                {
                    /** If there's not, we place the item.*/
                    PlaceItem();
                }
            }
        }

        /** Check if the player is near the board and is attempting to cut.*/
        if (Input.GetKey("j") && hasPlayer)
        {
            /** Check if there is an object here. */
            if (ObjectBeingCut != null)
            {
                /** If the cut timing is not yet 0, we cut.*/
                if (playerInteractScript.cutTiming >= 0)
                {
                    Cut();
                }

                /** Otherwise, we spawn the object and reset our variables. */
                else if (playerInteractScript.cutTiming < 0)
                {
                    Reset();
                }
            }
        }

        else if (!Input.GetKey("j"))
        {
            playerInteractScript.cutTiming = 100f;
            playerAnimator.SetBool("Cutting", false);
            playerInteractScript.isCutting = false;
            playerInteractScript.cutGroup.alpha = 0f;
        }
    }

    /** Check what the player is holding and updates it accordingly.*/
    public void CheckPlayerItem()
    {
        if (playerInteractScript.currObj == null)
        {
            return;
        }
        if (playerInteractScript.grabbed == true)
        {
            playerObject = playerInteractScript.currObj;
        }
    }

    /** Check if what the player's holding is a cuttable ingredient. */
    public bool CheckIfCuttable()
    {
        GameObject temp = playerInteractScript.currObj;
        bool isGrabbed = playerInteractScript.grabbed;

        if (temp != null && isGrabbed)
        {
            if (temp.GetComponent<Ingredient>().isCuttable)
            {
                return true;
            }
        }
        return false;
    }

    /** Changing our hasPlayer boolean to true if the player enters.*/
    private protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            hasPlayer = true;
        }
    }

    /** Changing our hasPlayer boolean to false if the player leaves.*/
    private protected virtual void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            hasPlayer = false;
        }
    }

    /** Places the item down onto the table. */
    public void PlaceItem()
    {
        /** Create a copy of the object. */
        GameObject temp = Instantiate(playerObject, stationPosition, new Quaternion(0f, 0f, 0f, 0f), this.transform);

        /** Set whatever the player's currently holding to none and destroying it - also set their grabbed variable to false. */
        Destroy(playerInteractScript.currObj);
        playerInteractScript.grabbed = false;

        /** Set the appropriate variables. */
        ObjectBeingCut = temp;
        ObjectBeingCutScript = temp.GetComponent<CuttableIngredient>();
    }

    /** Cut the object.*/
    private void Cut()
    {
        // Debug.Log("We've entered the cut() function"); 
        playerAnimator.SetBool("Cutting", true);
        //set the PlayerInteract variable cutTiming
        GameObject.Find("Player").GetComponent<PlayerInteract>().isCutting = true;
        playerInteractScript.cutTiming -= 1;
    }

    /** Spawn the cutted ingredient when you're done. */
    private void Reset()
    {
        playerAnimator.SetBool("Cutting", false);
        Instantiate(ObjectBeingCutScript.cuttedIngredient, this.transform.position, new Quaternion(0f, 0f, 0f, 0f), this.transform);
        Destroy(ObjectBeingCut);
        ObjectBeingCutScript = null;
        ObjectBeingCut = null;
        playerInteractScript.cutTiming = 100f;

        GameObject.Find("Player").GetComponent<PlayerInteract>().isCutting = false;
        GameObject.Find("Player").GetComponent<PlayerInteract>().cut.MyCurrentValue = 100f;
    }
}
