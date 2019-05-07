using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    private int step = 1;
    private int dishNum;
    private string finalDish;
    
    //Recipes
    public string[][] recipes = new string[][] {
        new string[2] { "CookedZombieLegDish", "ZombieLeg" },
        new string[6] { "ZombieBurger", "Bun", "CookedZombieBrain", "Cheese", "Tomato", "Lettuce"}
    };

    public bool GotNextIngredient(string ingredient)
    {
        if (finalDish == null)
        {
            for (int i = 0; i < recipes.Length; i++)
            {
                string[] recipe = recipes[i];
                if (string.Equals(ingredient, recipe[0])) {
                    finalDish = recipe[0];
                    dishNum = i;
                    step++;
                    return true;
                }
            }
        } else
        {
            if (string.Equals(ingredient, recipes[dishNum][step]))
            {
                step++;
            }
        }

        return false;
    }
}
