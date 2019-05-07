using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    #region Plate Variables
    private bool hasPlayer;
    private bool hasCooked;
    private string cookedIngredient;
    private bool cutted;
    #endregion

    //Get the GameObject of the player, ingredient, and the next object.
    #region Cooking GameObjects
    private GameObject playerObj;
    private GameObject cookedObj;
    public string nextPlate;
    public string requiredIngredient;
    public string plateName;
    #endregion

    #region Trigger Functions   

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        //Set the corresponding boolean value true for each contacted collider.
        string colTag = collider.transform.tag;

        switch (colTag)
        {
            case "Player":
                hasPlayer = true;
                playerObj = collider.gameObject;
                Debug.Log("Player entered plate");
                break;
            case "Ingredient":
                cookedObj = collider.gameObject;
                cookedIngredient = cookedObj.GetComponent<Ingredient>().ingredient;
                cutted = !cookedObj.GetComponent<Ingredient>().isCuttable;
                hasCooked = cookedObj.GetComponent<Ingredient>().cooked;

                // Dalton changed
                if (hasCooked && cookedObj.GetComponent<NewPlate>() != null) {
                    nextPlate = cookedObj.GetComponent<NewPlate>().nextPlate;
                }

                Debug.Log("Ingredient entered plate");
                break;

            default:
                break;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collider)
    {
        //The player can cook only when both he and the ingredient is in contact with the station.
        if (hasCooked && hasPlayer && cookedObj != null && cutted)
        {
            //Press 'e' to cook the ingredient.
            if (Input.GetKey(KeyCode.E))
            {
                Assemble();
                Debug.Log("Assembled");
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        //Set the boolean value false when not colliding.
        string colTag = collider.transform.tag;

        switch (colTag)
        {
            case "Player":
                hasPlayer = false;
                playerObj = null;
                //Debug.Log("Player exited");
                break;

            case "Ingredient":
                hasCooked = false;
                cookedObj = null;
                nextPlate = null;
                Debug.Log("Ingredient exited");
                break;

            default:
                break;
        }
    }
    #endregion

    #region Assemble
    private void Assemble()
    {
        hasCooked = false;

        /* if (plateName.Contains("Dish") && cookedIngredient != null)
        {
            cookedIngredient = cookedIngredient.Replace("Cooked", "");
            plateName = plateName.Replace("Dish", cookedIngredient + "Dish");
        } else
        {
            plateName = cookedIngredient + "Dish";
        }
        Debug.Log(plateName); */

        if (string.Equals(requiredIngredient, cookedIngredient) && nextPlate != null && requiredIngredient != null)
        {
            Instantiate(Resources.Load(nextPlate), transform.position, transform.rotation);
            Debug.Log(nextPlate + "instantiated");
            PlayerInteract player = playerObj.GetComponent<PlayerInteract>();
            player.currObj = null;
            player.grabbed = false;
            player.pressedEelsewhere = false;
            Destroy(gameObject);
            Destroy(cookedObj);
        } else if ((cookedIngredient.Equals("CookedZombieLeg") || cookedIngredient.Equals("Bun")) && transform.name.Contains("Plate"))
        {
            Instantiate(Resources.Load(cookedIngredient + "Dish"), transform.position, transform.rotation);
            Debug.Log("dish instantiated");
            PlayerInteract player = playerObj.GetComponent<PlayerInteract>();
            player.currObj = null;
            player.grabbed = false;
            player.pressedEelsewhere = false;
            Destroy(gameObject);
            Destroy(cookedObj);
        }


        /* if (!plateName.Contains("Dish"))
        {
            Instantiate(Resources.Load(cookedIngredient + "Dish"), transform.position, transform.rotation);
            PlayerInteract player = playerObj.GetComponent<PlayerInteract>();
            player.currObj = null;
            player.grabbed = false;
            player.pressedEelsewhere = false;
            Destroy(gameObject);
            Destroy(cookedObj);
        } else if (plateName.Contains("Dish") && cookedIngredient != null)
        {
            cookedIngredient = cookedIngredient.Replace("Cooked", "");
            plateName = plateName.Replace("Dish", cookedIngredient + "Dish");
            Debug.Log(plateName);
            Instantiate(Resources.Load(plateName), transform.position, transform.rotation);
            PlayerInteract player = playerObj.GetComponent<PlayerInteract>();
            player.currObj = null;
            player.grabbed = false;
            player.pressedEelsewhere = false;
            Destroy(gameObject);
            Destroy(cookedObj);
        }
        if (newPlate != null)
        {
            GameObject newDish = Instantiate(newPlate, transform) as GameObject;
            if (newDish == null)
            {
                Debug.Log("newDish is null");
            }
            Debug.Log("new plate instantiated");
            PlayerInteract player = playerObj.GetComponent<PlayerInteract>();
            player.currObj = null;
            player.grabbed = false;
            player.pressedEelsewhere = false;
            Destroy(gameObject);
            Destroy(cookedObj);
        } */
    }
    #endregion
}
