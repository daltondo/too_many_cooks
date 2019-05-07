using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    #region Ingredient variables
    [SerializeField]
    [Tooltip("Assign cook time to the ingredient")]
    private int cookTime;
    #endregion


    public void PickUp() {
        // Pick up object and attach it to the player
        gameObject.SetActive(false);
    }
    public void Update()
    {
        //if (Input.GetKey("a") || Input.GetKey("left"))
        //{
        //    Debug.Log("HERE");
        //}
        //if (Input.GetKey("d") || Input.GetKey("right"))
        //{
        //    Debug.Log("HERE");
        //}
        //if (Input.GetKey("w") || Input.GetKey("up"))
        //{
        //    Debug.Log("HERE");
        //}
        //if (Input.GetKey("s") || Input.GetKey("down"))
        //{
        //    Debug.Log("HERE");
        //}
    }
}
