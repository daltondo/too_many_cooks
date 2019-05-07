using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBox : MonoBehaviour
{
    public GameObject ingredient;
    public bool hasPlayer;
    public bool hasThrowawayAble;
    public GameObject player;


    protected void OnTriggerEnter2D(Collider2D collider)
    {
        // set the corresponding boolean value true for each contacted collider
        string colTag = collider.transform.tag;
        switch (colTag)
        {
            case "Player":
                hasPlayer = true;
                player = collider.gameObject;
                break;

            case "Ingredient":
                hasThrowawayAble = true;
                ingredient = collider.gameObject;
                break;

            case "Uncookable":
                hasThrowawayAble = true;
                ingredient = collider.gameObject;
                break;

            case "Dish":
                hasThrowawayAble = true;
                ingredient = collider.gameObject;
                break;

            default:
                break;
        }
    }


    protected void OnTriggerExit2D(Collider2D collider)
    {
        // set the boolean value false when not colliding
        string colTag = collider.transform.tag;
        switch (colTag)
        {
            case "Player":
                hasPlayer = false;
                player = null;
                break;

            case "Ingredient":
                hasThrowawayAble = false;
                break;

            case "Uncookable":
                hasThrowawayAble = false;
                break;

            case "Dish":
                hasThrowawayAble = false;
                break;

            default:
                break;
        }
    }


    protected void OnTriggerStay2D(Collider2D collider)
    {
        //The player can only throw away an ingredient if he is in range and is holding an ingredient
        if (hasPlayer && hasThrowawayAble && player.GetComponent<PlayerInteract>().grabbed)
        {
            //Press 'e' to open the box
            if (Input.GetButtonDown("Interact"))
            {
                // Maybe needed?
                GameObject.Find("Player").GetComponent<PlayerInteract>().currObjList.Remove(ingredient);

                // destroy the ingredient
                Destroy(ingredient);
                hasThrowawayAble = false;

                // destroy ingredient variables for player
                player.GetComponent<PlayerInteract>().currObj = null;
                player.GetComponent<PlayerInteract>().grabbed = false;
            }
            /*TODO: Have a box opening animation and ingredient being thrown away animation?*/
            FindObjectOfType<AudioManager>().Play("Trash");
        }
    }
}
