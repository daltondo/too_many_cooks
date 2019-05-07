using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    #region Ingredient variables
    //public string name = Object.name;
    [SerializeField]
    [Tooltip("Assign cook time to the ingredient")]
    private int cookTime;
    public bool isCuttable;
    public bool cooked;
    public string ingredient;
    #endregion

    public float GetCookTime()
    {
        return cookTime;
    }
}
