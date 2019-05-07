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
    float damage;
    float attackSpeed;
    float attackTimer; 
    Vector2 currDirection;
    bool isAttacking; 
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        damage = 10f;
        attackSpeed = 5f;
        attackTimer = 0f;
        playerAnim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>(); 
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
            Attack(); 
        }

        else
        {
            // Otherwise, decrement attackTimer 
            attackTimer -= Time.deltaTime;
        }
    }

    void Attack()
    {
        // Reset attackTimer 
        attackTimer = attackSpeed;

        // Starts the attack coroutine 
        StartCoroutine(AttackRoutine()); 
    }

    IEnumerator AttackRoutine()
    {
        // They are currently attacking 
        isAttacking = true;
        Debug.Log("Currently attacking");
        // Play attack animation
        //playerAnim.SetTrigger("IsAttacking"); 
        yield return new WaitForSeconds(hitBoxTiming);

        // Create hitbox 
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRigidBody.position + currDirection, Vector2.one / 2, 0f, Vector2.zero, 0);

        // For every hit inside of our "hits" 
        foreach (RaycastHit2D hit in hits)
        {
            // If the hit has a tag of "enemy" 
            if (hit.transform.CompareTag("Zombie"))
            {
                // Call takeDamage
                hit.transform.GetComponent<Zombie>().takeDamage(damage); 
            }
        }

        yield return new WaitForSeconds(endAnimationTiming);

        // Reset 
        isAttacking = false; 
    }
}
