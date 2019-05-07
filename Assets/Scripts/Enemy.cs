using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    #region enemyVariables
    float health;
    List<GameObject> ingredientList;
    System.Random rnd; 
    #endregion 
    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
        ingredientList = new List<GameObject>(); 
        health = 100; 
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            int randomNum = rnd.Next(ingredientList.Count);
            Instantiate(ingredientList[randomNum]); 
            Destroy(this); 
        }
    }

    public void takeDamage(float damage)
    {
        health -= damage; 
    }
}
