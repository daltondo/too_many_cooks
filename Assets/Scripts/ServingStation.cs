using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingStation : MonoBehaviour
{
    #region PlayerVars
    public GameObject player;
    public bool hasPlayer;
    public bool isTutorial;
    #endregion

    #region DishVars
    public GameObject dish;
    public bool hasDish;
    #endregion


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

            case "Dish":
                hasDish = true;
                dish = collider.gameObject;
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

            case "Dish":
                hasDish = false;
                break;

            default:
                break;
        }
    }


    protected void OnTriggerStay2D(Collider2D collider)
    {
        //The player can only throw away an ingredient if he is in range and is holding an ingredient
        if (hasPlayer && hasDish && player.GetComponent<PlayerInteract>().grabbed)
        {
            if (isTutorial)
            {
                if (Input.GetButtonDown("Interact") && dish.name.Contains("BunZombieBrainCheeseTomatoLettuceDish"))
                {
                    // Destroy burger
                    FindObjectOfType<AudioManager>().Play("Ding");

                    // Maybe needed?
                    GameObject.Find("Player").GetComponent<PlayerInteract>().currObjList.Remove(dish);

                    // destroy the ingredient
                    Destroy(dish);
                    hasDish = false;
                    dish = null;

                    // destroy ingredient variables for player
                    player.GetComponent<PlayerInteract>().currObj = null;
                    player.GetComponent<PlayerInteract>().grabbed = false;

                    // Proceed to next main scene
                    GameObject gm = GameObject.FindWithTag("GameController");
                    gm.GetComponent<GameManager>().StartGame();
                }
            } else {
                // How much money the player gets for this dish
                int tip = 0;
                bool cont = false;
                string currRecipe = null;
                RecipeSpawner recipeSpawner = GameObject.Find("RecipeSpawnManager").GetComponent<RecipeSpawner>();

                if (dish.name.Contains("BunZombieBrainCheeseTomatoLettuceDish") &&
                    recipeSpawner.numOfCurrRecipes["zombie_burger_card"] > 0)
                {
                    cont = true;
                    tip = 30;
                    currRecipe = "zombie_burger_card";
                }
                else if (dish.name.Contains("CookedZombieLegDish") &&
                  recipeSpawner.numOfCurrRecipes["zombie_leg_card"] > 0)
                {
                    cont = true;
                    tip = 5;
                    currRecipe = "zombie_leg_card";
                }

                //Press 'e' to open the box
                if (Input.GetButtonDown("Interact") && cont)
                {
                    /* TODO: Destroy the recipe card that you serve */
                    // need to change the 3 to the first slot of the ingredient we put in
                    int currSlot = recipeSpawner.GetSlot(currRecipe);
                    recipeSpawner.DeleteRecipe(currSlot);
                    recipeSpawner.currRecipeSlot -= 1;
                    recipeSpawner.numOfCurrRecipes[currRecipe] -= 1;
                    /* TODO: Move all the other slots over */
                    recipeSpawner.MoveSlotsOver(currSlot);

                    FindObjectOfType<AudioManager>().Play("Ding");

                    // Maybe needed?
                    GameObject.Find("Player").GetComponent<PlayerInteract>().currObjList.Remove(dish);

                    // destroy the ingredient
                    Destroy(dish);
                    hasDish = false;
                    dish = null;

                    // destroy ingredient variables for player
                    player.GetComponent<PlayerInteract>().currObj = null;
                    player.GetComponent<PlayerInteract>().grabbed = false;

                    // accumulate points for player
                    GameObject.Find("ScoreManager").GetComponent<ScoreManager>().updateDishesServed(1);
                    GameObject.Find("ScoreManager").GetComponent<ScoreManager>().accumulateTips(tip);
                }
                /*TODO: Have a box opening animation and ingredient being thrown away animation?*/
            }
        }
    }
}
