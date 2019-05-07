using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region animationVariables
    float hitBoxTiming;
    float endAnimationTiming;
    Animator playerAnim;
    #endregion

    #region attackVariables

    Rigidbody2D playerRigidBody;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float attackSpeed;
    float attackTimer; 
    Vector2 currDirection;
    bool isAttacking;
    #endregion

    PlayerInteract playerInteractScript; 
    public GameObject playerObject; 
    // Start is called before the first frame update
    void Start()
    {
        //damage = 10f;
        //attackSpeed = 0.75f;
        attackTimer = 0f;
        playerAnim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInteractScript = GetComponent<PlayerInteract>(); 
    }


    // Update is called once per frame
    void Update()
    {
        // Continuously update currDirection - used in attacking 
        currDirection = GetComponent<PlayerController>().currDirection; 

        /** If the player presses down the J button and their attacktimer is less than or equal to 0  
         * meaning enough time has passed for them to attack */ 
        if (Input.GetKeyDown("j") && attackTimer <= 0)
        {
            // Call attack 
            AttackFunction();
            attackTimer = .5f;  
        }
        attackTimer -= Time.deltaTime;
    }

    void Attack()
    {

        // Check if the player is holding something
        if (playerInteractScript.currObj != null && playerInteractScript.grabbed)
        {
            playerInteractScript.currObj.transform.position = playerInteractScript.holdPoint.position + Vector3.down;
            playerInteractScript.currObj.GetComponent<SpriteRenderer>().sortingLayerName = "Ingredient";
            playerInteractScript.currObj = null;
            playerInteractScript.grabbed = false;
        }
        // Reset attackTimer 
        attackTimer = attackSpeed;

        // Starts the attack coroutine 
        StartCoroutine(AttackRoutine()); 
    }

    IEnumerator AttackRoutine()
    {
        // They are currently attacking 
        isAttacking = true;
        playerAnim.SetBool("IsAttacking", true);
        Debug.Log("isAttacking: " + isAttacking);
        Debug.Log("Currently attacking");
        // Play attack animation
        //playerAnim.SetTrigger("IsAttacking"); 
        yield return new WaitForSeconds(hitBoxTiming);

        // Create hitbox 
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRigidBody.position + currDirection / 8, Vector2.one / 2, 0f, Vector2.zero, 0);

        // For every hit inside of our "hits" 
        foreach (RaycastHit2D hit in hits)
        {
            // If the hit has a tag of "enemy" 
            if (hit.transform.CompareTag("Zombie"))
            {
                Debug.Log("WE HIT A ZOMBIE");
                // Call takeDamage
                hit.transform.GetComponent<Zombie>().TakeDamage(damage); 
            }
        }

        yield return new WaitForSeconds(endAnimationTiming);

        // Reset 
        isAttacking = false;
        playerAnim.SetBool("IsAttacking", false);
    }

    void AttackFunction()
    {
        isAttacking = true;
        playerAnim.SetBool("IsAttacking", true);
        playerAnim.SetTrigger("Attacking");
        Debug.Log("Currently attacking");

        if (playerInteractScript.currObj != null && playerInteractScript.grabbed)
        {
            playerInteractScript.currObj.transform.position = playerInteractScript.holdPoint.position + Vector3.down;
            playerInteractScript.currObj.GetComponent<SpriteRenderer>().sortingLayerName = "Ingredient";
            playerInteractScript.currObj = null;
            playerInteractScript.grabbed = false;
        }

        // Create hitbox 
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRigidBody.position + currDirection / 8, Vector2.one / 2, 0f, Vector2.zero, 0);

        // For every hit inside of our "hits" 
        foreach (RaycastHit2D hit in hits)
        {
            // If the hit has a tag of "enemy" 
            if (hit.transform.CompareTag("Zombie"))
            {
                Debug.Log("WE HIT A ZOMBIE");
                // Call takeDamage
                hit.transform.GetComponent<Zombie>().TakeDamage(damage);
            }
        }

        isAttacking = false;
        playerAnim.SetBool("IsAttacking", false);
    }
}
