using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    #region targetVariables
    public Transform player;
    public bool attackPlayer;
    #endregion

    #region movementVariables
    Rigidbody2D enemyRB;
    public float moveSpeed;
    private float timeToChangeDirection;
    private Vector2 direction;
    #endregion

    #region healthVariables
    public float maxHealth;
    public float currHealth;
    #endregion

    #region attackVariables
    public float attackPower;
    public float attackRadius;
    public float attackTime;
    private float lastDamage;
    #endregion

    #region spawningIngredientVariables
    System.Random rnd;
    public List<GameObject> ingredientList;
    #endregion


    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
        rnd = new System.Random();

        timeToChangeDirection = 0;
        direction = new Vector2(0, 0);
    }

    // Use this for initialization
    void Update()
    {
        // Check to see if zombie is dead
        if (currHealth <= 0) {
            Die();
            /* TODO: Play death animation?? */
        }

        // Check to see if we know where player is
        if (player == null)
        {
            // If there is no player, wander around randomly
            timeToChangeDirection -= Time.deltaTime;

            if (timeToChangeDirection <= 0)
            {
                direction = ChangeDirection();
            }

            enemyRB.velocity = direction * moveSpeed;
            return;
        } else {
            Move();
        }
    }

    private void FixedUpdate()
    {
        if (lastDamage >= attackTime) {
            lastDamage = 0;
        }
        lastDamage += Time.fixedDeltaTime;
    }


    #region movementFunctions
    private void Move()
    {
        // Calculate the movement vector. Player pos - Enemy pos = Direction of player relative to enemy
        Vector2 direction = player.position - transform.position;
        enemyRB.velocity = direction.normalized * moveSpeed;
    }

    private Vector2 ChangeDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        timeToChangeDirection = 1.5f;
        Vector2 dirr = new Vector2(x, y);
        return dirr.normalized;
    }
    #endregion


    #region healthFunctions
    public void TakeDamage(float damage) {
        currHealth -= damage;
    }

    private void Die() {
        int rand = rnd.Next(0, ingredientList.Count);
        int coinFlip = rnd.Next(0, 1);

        // 0.50 chance of dropping an ingredient
        if (coinFlip == 1) {
            GameObject droppedObj = Instantiate(ingredientList[rand], this.transform.position, Quaternion.identity);
            droppedObj.name = ingredientList[rand].name;
        }

        FindObjectOfType<AudioManager>().Play("ZombieDie");
        Destroy(gameObject);
    }
    #endregion


    #region attackFunctions
    private void Attack()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attackRadius, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                FindObjectOfType<AudioManager>().Play("ZombieAttack");
                // deal damage
                hit.transform.GetComponent<PlayerHealth>().TakeDamage(attackPower);
                lastDamage = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            /* TODO: Play attack animation? */
            attackPlayer = true;
            lastDamage = 0.5f;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            /* TODO: Play attack animation? */
            attackPlayer = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (attackPlayer && lastDamage >= attackTime) {
            Debug.Log("STAYED IN COLLISION ZONE");
            Attack();
        }
    }
    #endregion
}