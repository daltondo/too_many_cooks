using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    #region healthVariables
    public float maxHealth;
    float currHealth;
    [SerializeField]
    private Stat health;
    #endregion


    public void Awake()
    {
        health.Initialize(maxHealth, maxHealth);
        currHealth = maxHealth;
        health.MyCurrentValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Check to see if zombie is dead
        if (currHealth <= 0)
        {
            Die();
            /* TODO: Play death animation?? */
        }
    }


    #region healthFunctions
    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        health.MyCurrentValue = currHealth;
    }

    private void Die() {
        Destroy(gameObject);
        /* TODO: change game scenes */
        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().LoseGame();
    }
    #endregion
}
