using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    #region InteractableObjects
    public GameObject currObj = null;
    RaycastHit2D[] hits = new RaycastHit2D[0];
    #endregion

    #region GrabbedObjects
    public bool grabbed;
    public float distance = 3f;
    Transform holdPoint;
    public Transform rightHoldPoint;
    public Transform leftHoldPoint;
    public Transform upHoldPoint;
    public Transform downHoldPoint;
    #endregion


    void Update() {
        Vector2 currDir = GameObject.Find("Player").GetComponent<PlayerController>().currDirection;
        ChangeHoldPoint(currDir);
        CheckInteract();
    }


    #region InteractFunctions
    void CheckInteract() {
        // Check if the player is in the zone of an item and if the player clicks "E"/"Interact"
        if (Input.GetButtonDown("Interact") && currObj) {
            // Grabs the item and puts it in the player's hands
            if (!grabbed)
            {
                currObj.transform.position = holdPoint.position;
                currObj.GetComponent<SpriteRenderer>().sortingLayerName = "HeldIngredient";
            }
            else
            {
                // Cast a ray in the direction of the key pressed 
                // Check if the ray collides with a counter/cooking station/plate
                // If it does, change its position to the position of that
                if (Input.GetKey("a") || Input.GetKey("left"))
                {
                    hits = Physics2D.RaycastAll(holdPoint.position + Vector3.down, Vector2.left * transform.localScale.x, distance);
                }
                if (Input.GetKey("d") || Input.GetKey("right"))
                {
                    hits = Physics2D.RaycastAll(holdPoint.position + Vector3.down, Vector2.right * transform.localScale.x, distance);
                }
                if (Input.GetKey("w") || Input.GetKey("up"))
                {
                    hits = Physics2D.RaycastAll(holdPoint.position, Vector2.up * transform.localScale.x, distance);
                }
                if (Input.GetKey("s") || Input.GetKey("down"))
                {
                    hits = Physics2D.RaycastAll(holdPoint.position, Vector2.down * transform.localScale.x, distance);
                }

                // Drop item on the counter if the ray hit a counter
                bool hitCounter = false;
                if (hits.Length > 0) {
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null && hit.collider.gameObject.tag == "Counter" && !hitCounter)
                        {
                            currObj.transform.position = hit.collider.gameObject.transform.position;
                            hitCounter = true;
                            hits = new RaycastHit2D[0];
                            currObj.GetComponent<SpriteRenderer>().sortingLayerName = "HeldIngredient";
                        }
                    }
                }

                // Drops the item
                if (!hitCounter) {
                    currObj.transform.position = holdPoint.position + Vector3.down;
                    currObj.GetComponent<SpriteRenderer>().sortingLayerName = "Ingredient";
                }
            }
            grabbed = !grabbed;
        }

        // If the player is currently grabbing an object, have the object be held in their hands
        if (grabbed) {
            currObj.transform.position = holdPoint.position;
        }
    }


    void ChangeHoldPoint(Vector2 currDirection) {
        if (currDirection == Vector2.right) {
            holdPoint = rightHoldPoint;
        }
        if (currDirection == Vector2.left) {
            holdPoint = leftHoldPoint;
        }
        if (currDirection == Vector2.up) {
            holdPoint = upHoldPoint;
        }
        if (currDirection == Vector2.down) {
            holdPoint = downHoldPoint;
        }
    }
    #endregion


    #region TriggerFunctions
    void OnTriggerEnter2D(Collider2D obj) {
        // Enter collider of an ingredient
        if (obj.CompareTag("Ingredient")) {
            currObj = obj.gameObject;
        }
    }


    void OnTriggerExit2D(Collider2D obj) {
        // Exit collider of an ingredient
        if (obj.CompareTag("Ingredient")) {
            if (obj.gameObject == currObj) {
                currObj = null;
            }
        }
    }
    #endregion
}
