using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region playerMoveVariables 

    Rigidbody2D playerRigidBody;
    public float moveSpeed; // We can increase this later on with regards to buffs, sprinting? etc.
    public Vector2 currDirection; // Used for animation
    Animator playerAnim;

    // Is the player sprinting? 
    public bool isSprinting = false;
    public float maxSprint = 20f;
    public float sprintGauge = 20f;
    [SerializeField]
    private Stat sprint;

    #endregion

    void Start()
    {
        Debug.Log("Player tag: " + gameObject.tag); 
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerAnim.SetBool("IsMoving", false);
        playerAnim.SetFloat("YDirection", -1);

        sprint.Initialize(maxSprint, maxSprint);
        sprint.MyCurrentValue = maxSprint;
    }

    // Update is called once per frame
    void Update()
    {
        checkSprint();

        // If the WASD or arrow keys are being pressed... 
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            // If the player is sprinting...
            if (isSprinting)

            {
                Debug.Log("Sprinting");
                // Set the animator's IsMoving variable to true 
                playerAnim.SetBool("IsMoving", true);
                // Call PlayerMove - passing in our inputs 
                PlayerMove(Input.GetAxisRaw("Horizontal") * 2 * moveSpeed, Input.GetAxisRaw("Vertical") * 2 * moveSpeed);
                sprintGauge -= Time.deltaTime * 5f;
                sprint.MyCurrentValue = sprintGauge;

                if (sprintGauge < 0f)
                {
                    isSprinting = false;
                }
            }

            else if (isSprinting == false)
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    if (sprintGauge < maxSprint)
                    {
                        sprintGauge += Time.deltaTime * 1f;
                        sprint.MyCurrentValue = sprintGauge;
                    }
                }

                // Set the animator's IsMoving variable to true 
                playerAnim.SetBool("IsMoving", true);
                // Call PlayerMove - passing in our inputs 
                PlayerMove(Input.GetAxisRaw("Horizontal") * moveSpeed, Input.GetAxisRaw("Vertical") * moveSpeed);

            }
        }

        else
        {
            // Otherwise, we're not moving and not attacking 
            playerAnim.SetBool("IsMoving", false);
            // Call PlayerMove by passing in 0, 0 - no movement 
            PlayerMove(0, 0);
            if (sprintGauge < maxSprint)
            {
                sprintGauge += Time.deltaTime * 1f;
                sprint.MyCurrentValue = sprintGauge;
            }

        }

    }

    void PlayerMove(float x, float y)
    {
        // If we're passing things in - i.e. there's movement 
        if (x != 0 || y != 0)
        {
            // Set the animator's floats to whatever the current values are 
            playerAnim.SetFloat("XDirection", x);
            playerAnim.SetFloat("YDirection", y);

            // Move the player's rigidbody - if you don't normalize the vector, playerMove's faster on the diagonal 
            playerRigidBody.velocity = new Vector2(x, y).normalized;

            // Setting the player's current direction 
            currDirection = new Vector2(x, y);
        }

        // Otherwise, it's going to be just 0, 0
        playerRigidBody.velocity = new Vector2(x, y);

        // change hold point
    }

    void checkSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (sprintGauge > 1.0f)
            {
                isSprinting = true;
            }
        }

        else
        {
            isSprinting = false;
        }
    }
}
