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
    public List<GameObject> ingredients = new List<GameObject>();
    #endregion

    //Get the GameObject of the player and ingredient.
    #region Cooking GameObjects
    protected GameObject player;
    protected GameObject ingredient;
    #endregion

    #region Unused Parent Functions
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }
    #endregion

    #region collisionFunctions   

    protected void OnTriggerEnter2D(Collider2D collider)
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
                Debug.Log("Ingredient entered");
                break;

            default:
                break;
        }
    }

    protected void OnTriggerStay2D(Collider2D collider)
    {
        //The player can cook only when both he and the ingredient is in contact with the station.
        if (hasPlayer && hasIngredient)
        {
            Debug.Log("The player can now cook");
            //Press 'e' to cook the ingredient.
            if (Input.GetKey("e"))
            {
                Debug.Log("Is cooking");
                Debug.Log(ingredient.name);
                Cook(ingredient);
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collider)
    {
        //Set the boolean value false when not colliding.
        string colTag = collider.transform.tag;

        switch (colTag)
        {
            case "Player":
                hasPlayer = false;
                player = null;
                Debug.Log("Player exited");
                break;

            case "Ingredient":
                hasIngredient = false;
                ingredient = null;
                Debug.Log("Ingredient exited");
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
        //Set isCooking true, add the ingredient to the list, distroy the ingredient object, and transform the station.
        isCooking = true;
        //string ingredientName = ingredient.name;
        ingredients.Add(ingredient);

        string newStoveName = ingredient.name + "Stove";
        Vector3 position = this.transform.position;
        Quaternion rotation = this.transform.rotation;

        GameObject newStove = (GameObject)Instantiate(Resources.Load(newStoveName), position, rotation);

        Destroy(this.gameObject);
        Destroy(ingredient);

        // Destroy ingredient variables for player
        GameObject.Find("Player").GetComponent<PlayerInteract>().currObj = null;
        GameObject.Find("Player").GetComponent<PlayerInteract>().grabbed = false;

        //Instantiate(StoveIsCooking, transform.position, transform.rotation);
    }
    #endregion
}
