using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RecipeSpawner : MonoBehaviour
{
    // The canvas prefab
    public GameObject canvas;
    // The actualy UI/Image prefab
    public GameObject[] recipeSlots;
    // Different source images
    public Sprite[] sprites;
    // Current recipe slot
    public int currRecipeSlot;
    private int maxRecipeSlots;

    // keeps track of first spawn
    private bool firstSpawn;

    // Keeps track of the current recipes spawned
    public Dictionary<string, int> numOfCurrRecipes = new Dictionary<string, int>();
    public Dictionary<int, GameObject> slotToRecipe = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        currRecipeSlot = 0;
        maxRecipeSlots = 4;
        firstSpawn = true;

        // Set up 0 recipes of each type spawned
        foreach (Sprite sprite in sprites) {
            numOfCurrRecipes[sprite.name] = 0;
        }

        StartCoroutine(SpawnRecipes());
    }

    IEnumerator SpawnRecipes()
    {
        while (true)
        {
            if (firstSpawn) {
                firstSpawn = false;
                yield return new WaitForSeconds(3);
            }

            if (currRecipeSlot < maxRecipeSlots) {
                // Pick next recipe to instantiate
                GameObject currRecipe = recipeSlots[currRecipeSlot];
                var currImage = Instantiate(currRecipe) as GameObject;

                // Pick a random sprite to fill the recipe with
                int rand = Random.Range(0, sprites.Length);
                Sprite currSprite = sprites[rand];

                // Set the current recipe's sprite
                currImage.GetComponent<Image>().sprite = currSprite;
                // Set the current recipe's parent as the canvas 
                currImage.transform.SetParent(canvas.transform, false);

                // Update variables
                slotToRecipe[currRecipeSlot] = currImage;
                currRecipeSlot += 1;
                numOfCurrRecipes[currSprite.name] += 1;
            }

            // pick a random time within the range to spawn the next recipe
            float randomTime = Random.Range(15f, 20f);
            yield return new WaitForSeconds(randomTime);
        }
    }

    public void DeleteRecipe(int slot) {
        Destroy(slotToRecipe[slot]);
        slotToRecipe[slot] = null;
    }

    public int GetSlot(string name) {
        for(int i = 0; i < slotToRecipe.Count; i++) { 
            if (slotToRecipe[i] != null) {
                if (slotToRecipe[i].GetComponent<Image>().sprite.name == name)
                {
                    return i;
                }
            }
        }
        return 0;
    }


    public void MoveSlotsOver(int deletedSlot) {
        int currSlot = deletedSlot;
        for (int i = deletedSlot + 1; i < slotToRecipe.Count; i++)
        {
            if (slotToRecipe[i] != null) {
                // 1. Get the current recipe and get the curr slot
                // This is correct
                GameObject recipeToBeDeleted = slotToRecipe[i];
                GameObject newRecipe = recipeSlots[currSlot];

                // 2. Instantiate the new recipe in its new slot with the old sprite
                var currImage = Instantiate(newRecipe) as GameObject;
                currImage.GetComponent<Image>().sprite = recipeToBeDeleted.GetComponent<Image>().sprite;
                currImage.transform.SetParent(canvas.transform, false);

                // 3. Delete the recipe
                Destroy(slotToRecipe[i]);
                slotToRecipe[currSlot] = currImage;
                currSlot += 1;
            }
        }
        currRecipeSlot = currSlot;
        slotToRecipe[currRecipeSlot] = null;

        // For Debugging
        Debug.Log("CurrRecipeSlot: " + currRecipeSlot);
        Debug.Log("SlotToRecipe.Count: " + slotToRecipe.Count);
        PrintSlotToRecipe();
    }

    public void PrintSlotToRecipe() { 
        for(int i = 0; i < slotToRecipe.Count; i++) {
            Debug.Log(i + " " + slotToRecipe[i]);
        }
    }
}
