using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : CookingStation
{

    #region Dish Variables
    //Check if a complete dish is cooked
    private bool foodCooked = false;
    private bool grabbed = false;
    #endregion

    #region Zombie Variables
    private bool hasZombie = false;
    private float timeInZone = 0f;
    #endregion

    protected override void Update()
    {
        cookTimer -= Time.deltaTime;
        //Debug.Log(cookTimer);
        if (cookTimer <= 0 && !foodCooked)
        {
            foodCooked = true;
            CookedStove();
            FindObjectOfType<AudioManager>().Play("Ding");
        }

        if (hasZombie) {
            timeInZone += Time.deltaTime;
            if (timeInZone > 4f) {
                DestroyFood();
                timeInZone = 0f;
            }
        }
    }

    #region Trigger Functions   

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        //Set the corresponding boolean value true for each contacted collider.
        string colTag = collider.transform.tag;

        switch (colTag)
        {
            case "Player":
                hasPlayer = true;
                player = collider.gameObject;
                //Debug.Log("Player entered");
                break;
            case "Zombie":
                hasZombie = true;
                break;

            default:
                break;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collider)
    {
        //The player can cook only when both he and the ingredient is in contact with the station.
        if (foodCooked && hasPlayer && !grabbed)
        {
            //Debug.Log("The player can now cook");
            //Press 'e' to cook the ingredient.
            if (Input.GetKeyDown(KeyCode.E))
            {
                GrabFood(ingredientName);
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
                player = null;
                //Debug.Log("Player exited");
                break;

            case "Zombie":
                hasZombie = false;
                break;

            default:
                break;
        }
    }
    #endregion

    #region Dish Functions
    //The player takes the dish and he can put the dish on the plate. The stove is replaced by the CookingStation.
    private void GrabFood(string dish)
    {
        if (!grabbed)
        {
            Debug.Log("GRABBED FOOD");
            grabbed = true;
            GameObject originalStation = Instantiate(Resources.Load("CookingStation"), transform.position, transform.rotation) as GameObject;
            ingredientName = "Cooked" + ingredientName;
            GameObject cooked = Instantiate(Resources.Load(ingredientName), transform.position, transform.rotation) as GameObject;
            player.GetComponent<PlayerInteract>().currObj = cooked;
            player.GetComponent<PlayerInteract>().grabbed = true;
            player.GetComponent<PlayerInteract>().pressedEelsewhere = true;
            Destroy(gameObject);
        }
    }

    private void CookedStove()
    {
        string stoveName = "Cooked" + ingredientName + "Stove";
        GameObject cookedObj = Instantiate(Resources.Load(stoveName), transform.position, transform.rotation) as GameObject;
        Stove cookedStove = cookedObj.GetComponent<Stove>();
        cookedStove.foodCooked = true;
        cookedStove.ingredientName = ingredientName;
        Destroy(gameObject);
    }

    // Function called when zombie disrupts the cooking
    // Spawns a regular CookingStation with no ingredient in it
    private void DestroyFood()
    {
        grabbed = true;
        GameObject originalStation = Instantiate(Resources.Load("CookingStation"), transform.position, transform.rotation) as GameObject;
        Destroy(gameObject);
        FindObjectOfType<AudioManager>().Play("ZombieAttack");
    }
    #endregion
}