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

        if (hasPlayer && Input.GetButtonDown("Interact")) { 

            if (objectBeingCut != null && playerInteract.grabbed == false)
            {
                Debug.Log("Entered this loop.");
                Destroy(objectBeingCut);
                this.objectBeingCut = null;
                this.cutScript = null; 
            }

            if (objectBeingCut == null && playerInteract.grabbed == true)
            {
                Debug.Log("Entered this one!"); 
                if (playerInteract.currObj.CompareTag("Ingredient"))
                {
                    if (playerInteract.currObj.GetComponent<Ingredient>().isCuttable)
                    {
                        PlaceItem();
                    }
                }
            }

        }

        if (hasPlayer && objectBeingCut != null && cutTiming > 0)
        {
            if (Input.GetKey("j"))
            {
                Cut();
            }

            else if (!Input.GetKey("j"))
            {
                playerAnim.SetBool("Cutting", false);
                GameObject.Find("Player").GetComponent<PlayerInteract>().isCutting = false;
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
        if (playerInteract.currObj.CompareTag("Ingredient"))
        {
            if  (playerInteract.currObj.GetComponent<Ingredient>().isCuttable)
            {
                GameObject temp = Instantiate(playerInteract.currObj, stationPosition, new Quaternion(0f, 0f, 0f, 0f), this.transform);
                Destroy(playerInteract.currObj);
                objectBeingCut = temp;
                cutScript = temp.GetComponent<CuttableIngredient>();
                playerInteract.grabbed = false;
            }
        }
    }

    private void Cut()
    {
        playerAnim.SetBool("Cutting", true);
        cutTiming -= 1;

        // set the PlayerInteract variable cutTiming
        GameObject.Find("Player").GetComponent<PlayerInteract>().isCutting = true;
        GameObject.Find("Player").GetComponent<PlayerInteract>().cutTiming = cutTiming;
    }

    private void Reset()
    {
        playerAnim.SetBool("Cutting", false);
        Instantiate(cutScript.cuttedIngredient, this.transform.position, new Quaternion(0f, 0f, 0f, 0f), this.transform);
        Destroy(objectBeingCut);
        cutScript = null;
        objectBeingCut = null;
        cutTiming = 100f;

        GameObject.Find("Player").GetComponent<PlayerInteract>().isCutting = false;
        GameObject.Find("Player").GetComponent<PlayerInteract>().cut.MyCurrentValue = 100f;
    }
}
