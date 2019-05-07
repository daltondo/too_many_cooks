using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    #region targetVariables
    public Transform player;
    public bool attackPlayer;
    protected float timeNearPlayer;
    #endregion

    #region movementVariables
    Rigidbody2D enemyRB;
    public float moveSpeed;
    protected float timeToChangeDirection;
    protected Vector2 direction;
    protected Vector2 nearestCookingStation;
    protected bool moveTowardsCookingStation;
    Animator enemyAnim;
    #endregion

    #region healthVariables
    public float maxHealth;
    public float currHealth;
    [SerializeField]
    private Stat health;
    [SerializeField]
    private CanvasGroup healthGroup;
    private bool isDying;
    #endregion

    #region attackVariables
    public float attackPower;
    public float attackRadius;
    public float attackTime;
    protected float lastDamage;
    #endregion

    #region spawningIngredientVariables
    System.Random rnd;
    public List<GameObject> ingredientList;
    #endregion

    #region zombieTypes
    public bool isMessyZombie;
    public bool isStrongZombie;
    public bool isTutorial;
    #endregion


    #region updateFunctions
    protected void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        rnd = new System.Random();

        timeToChangeDirection = 0;
        direction = new Vector2(0, 0);
        moveTowardsCookingStation = false;
        timeNearPlayer = 0;

        // Init health variables
        health.Initialize(maxHealth, maxHealth);
        currHealth = maxHealth;
        health.MyCurrentValue = maxHealth;
        healthGroup.alpha = 0;

        // Set animation variables
        enemyAnim.SetBool("IsMoving", false);
        enemyAnim.SetBool("IsAttacking", false);
        enemyAnim.SetBool("IsDying", false);
        enemyAnim.SetFloat("Y-Dir", -1);
    }

    // Use this for initialization
    protected void Update()
    {
        // Check to see if zombie is dead
        if (currHealth <= 0) {
            // Play death animation
            enemyAnim.SetBool("IsDying", true);

            if (!isDying) {
                Die();
            }
            isDying = true;
        }


        // Check if zombie is near a cooking station with an ingredient in it, if so move towards it
        if (isMessyZombie) {
            if (!moveTowardsCookingStation) {
                nearestCookingStation = CheckIfNearCookingStation();
                if (nearestCookingStation != Vector2.zero) {
                    moveTowardsCookingStation = true;
                }
            }
            if (moveTowardsCookingStation) {
                if (CheckIfNearCookingStation() == Vector2.zero) {
                    nearestCookingStation = Vector2.zero;
                    moveTowardsCookingStation = false;
                } else {
                    Move(nearestCookingStation);
                    return;
                }
            }
        }

        // Check to see if the zombie knows where the player is
        if (player != null) {
            if (attackPlayer)
            {
                Move(player.position);
                return;
            }
            else if (timeNearPlayer < 4)
            {
                Move(player.position);
                timeNearPlayer += Time.deltaTime;
                return;
            }
        }

        // If there is no player, wander around randomly
        timeToChangeDirection -= Time.deltaTime;

        // Change directions at every time interval or stopped moving
        if (timeToChangeDirection <= 0 || enemyRB.velocity == Vector2.zero)
        {
            direction = ChangeDirection();
        }

        enemyRB.velocity = direction * moveSpeed;

        if (enemyRB.velocity != Vector2.zero)
        {
            // Play movement animation
            enemyAnim.SetBool("IsMoving", true);
        }
        else
        {
            // Stop movement animation
            enemyAnim.SetBool("IsMoving", false);
        }

    }

    protected void FixedUpdate()
    {
        if (lastDamage >= attackTime) {
            lastDamage = 0;
        }
        lastDamage += Time.fixedDeltaTime;
    }
    #endregion


    #region movementFunctions
    protected void Move(Vector3 destination)
    {
        // Calculate the movement vector. Player pos - Enemy pos = Direction of player relative to enemy
        Vector2 currDirection = destination - transform.position;
        enemyRB.velocity = currDirection.normalized * moveSpeed;

        // Set direction for animation
        enemyAnim.SetFloat("X-Dir", currDirection.x);
        enemyAnim.SetFloat("Y-Dir", currDirection.y);

        if (enemyRB.velocity != Vector2.zero)
        {
            // Play movement animation
            enemyAnim.SetBool("IsMoving", true);
        }
        else
        {
            // Stop movement animation
            enemyAnim.SetBool("IsMoving", false);
        }
        return;
    }

    protected Vector2 ChangeDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        // Set direction for animation
        enemyAnim.SetFloat("X-Dir", x);
        enemyAnim.SetFloat("Y-Dir", y);

        Vector2 dirr = new Vector2(x, y);
        timeToChangeDirection = 1.5f;
        return dirr.normalized;
    }

    protected Vector2 CheckIfNearCookingStation() {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 10f);
        foreach (Collider2D hit in hitColliders) {
            if (hit.gameObject.CompareTag("CookingStationWithIngredient")) {
                return hit.gameObject.transform.position;
            }
        }

        return Vector2.zero;
    }
    #endregion


    #region healthFunctions
    public void TakeDamage(float damage) {
        healthGroup.alpha = 1;
        currHealth -= damage;
        //Debug.Log(currHealth);
        health.MyCurrentValue = currHealth;

        FindObjectOfType<AudioManager>().Play("ZombieHurt");
    }

    protected void Die() {
        // Strong Zombie has the chance of dropping 2 loots
        int numDrops = 1;
        if (isStrongZombie) {
            numDrops = 2;
        }

        while (numDrops > 0) {
            // 0.66 chance of dropping zombie brain, 0.33 chance of dropping zombie leg
            int i = rnd.Next(0, ingredientList.Count);

            // 0.80 chance of dropping an ingredient
            int coinFlip = Random.Range(0, 5);
            if (isTutorial) {
                coinFlip = 1;
            }

            if (true) {
                GameObject droppedObj = Instantiate(ingredientList[i], this.transform.position, Quaternion.identity);
                Debug.Log("DROPPED INGREDIENT: " + ingredientList[i].name);
                droppedObj.name = ingredientList[i].name;
            }

            numDrops -= 1;
        }

        // Play death audio
        FindObjectOfType<AudioManager>().Play("ZombieDie");
        // Decrement current number of zombies in the spawn manager
        GameObject.Find("ZombieSpawnManager").GetComponent<ZombieSpawnManager>().currNumOfZombies -= 1;
        // Destroy the zombie object
        Destroy(gameObject, .5f);
    }

    #endregion


    #region attackFunctions
    protected void Attack()
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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            // Play attack animation
            enemyAnim.SetBool("IsAttacking", true);
            attackPlayer = true;
            lastDamage = 0.5f;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            // Stop attack animation
            enemyAnim.SetBool("IsAttacking", false);
            attackPlayer = false;
            timeNearPlayer = 0f;
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (attackPlayer && lastDamage >= attackTime) {
            Debug.Log("STAYED IN COLLISION ZONE");
            Attack();
        }
    }
    #endregion
}