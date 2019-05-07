using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingStation : MonoBehaviour
{
    //Uses boolean variables to check if the player and ingredient collides with the station.
    #region CookingVariables
    //The player must carry an ingredient to cook.
    protected bool hasIngredient;
    //The player must be near the station.
    protected bool hasPlayer;
    //Indicate if it is cooking.
    protected bool isCooking;
    //Time counter for ingredients.
    protected float cookTimer;
    //A list of ingredients to keep track of the recipe/cooking process.
    protected string ingredientName;
    //Check if new stove object is created.
    protected bool stoveCreated = false;
    #endregion

    //Get the GameObject of the player and ingredient.
    #region Cooking GameObjects
    protected GameObject player;
    protected GameObject ingredient;
    protected Ingredient ingredientComp;
    #endregion

    //Recipes
    #region
    protected string[][] recipes = new string[][] { new string[] { "ZombieLegDish", "ZombieLeg" } };
    #endregion

    #region Unused Parent Functions

    protected virtual void Update()
    {

    }
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
                player = collider.gameObject;
                Debug.Log("Player entered");
                break;
            case "Ingredient":
                hasIngredient = true;
                ingredient = collider.gameObject;
                ingredientComp = ingredient.GetComponent<Ingredient>();
                cookTimer += this.ingredientComp.GetCookTime();
                //Debug.Log("Ingredient entered");
                break;
            default:
                break;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collider)
    {
        //The player can cook only when both he and the ingredient is in contact with the station.
        if (hasIngredient)
        {
            //Debug.Log("The player can now cook");
            //Press 'e' to cook the ingredient.
            if (Input.GetKeyDown(KeyCode.E) && !ingredientComp.cooked && cookTimer > 0)
            {
                Debug.Log("Is cooking");
                Cook(ingredient);
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

            case "Ingredient":
                hasIngredient = false;
                ingredient = null;
                //Debug.Log("Ingredient exited");
                break;

            default:
                break;
        }
    }
    #endregion

    #region cookFunction
    //The player cooks. 
    protected void Cook(GameObject ingredient)
    {
        FindObjectOfType<AudioManager>().Play("Splash");
        //Set isCooking true, add the ingredient to the list, distroy the ingredient object, and transform the station.
        isCooking = true;
        hasIngredient = false;

        string newStoveName = ingredientComp.ingredient + "Stove";
        Vector3 position = this.transform.position;
        Quaternion rotation = this.transform.rotation;

        //Create a new stove object and copy the ingredients and recipes to it.
        if (!stoveCreated)
        {
            stoveCreated = true;
            GameObject newStoveObj = Instantiate(Resources.Load(newStoveName), position, rotation) as GameObject;
            Stove newStove = newStoveObj.GetComponent<Stove>();
            CopyStove(newStove, this);
            Debug.Log("Created stove with name: " + newStoveName);
        }

        Destroy(this.gameObject);
        Destroy(ingredient);

        // Destroy ingredient variables for player
        player.GetComponent<PlayerInteract>().currObj = null;
        player.GetComponent<PlayerInteract>().grabbed = false;

        //Instantiate(StoveIsCooking, transform.position, transform.rotation);
    }
    #endregion

    #region Copy Stove Function

    private void CopyStove(Stove newStove, CookingStation old)
    {
        newStove.cookTimer = cookTimer;
        newStove.ingredientName = ingredientComp.ingredient;

        Debug.Log(newStove.ingredientName);

        newStove.recipes = new string[recipes.Length][];
        for (int i = 0; i < recipes.Length; i++)
        {
            newStove.recipes[i] = new string[recipes[i].Length];
            for (int j = 0; j < recipes[i].Length; j++)
            {
                newStove.recipes[i][j] = string.Copy(recipes[i][j]);
            }
        }
    }

    #endregion
}
