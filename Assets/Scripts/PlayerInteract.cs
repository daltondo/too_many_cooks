using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInteract : MonoBehaviour
{
    #region InteractableObjects
    public GameObject currObj;
    RaycastHit2D[] hits = new RaycastHit2D[0];
    public float distance = 3f;
    public bool pressedEelsewhere;

    private string[] validObjTags = { "Plate", "Dish", "Uncookable", "Ingredient" };
    //public GameObject[] currObjList;
    public Dictionary<GameObject, float> currObjList;
    #endregion


    #region GrabbedObjects
    public bool grabbed;
    public Transform holdPoint;
    public Transform rightHoldPoint;
    public Transform leftHoldPoint;
    public Transform upHoldPoint;
    public Transform downHoldPoint;
    public Transform upRightHoldPoint;
    public Transform upLeftHoldPoint;
    public Transform downRightHoldPoint;
    public Transform downLeftHoldPoint;
    #endregion


    #region PlayerControllerVariables
    Animator playerAnim;
    public float moveSpeed;
    #endregion


    #region CutVariables
    public float cutTiming;
    public bool isCutting = false;
    public bool nearCuttingBoard = false;
    public bool canGrabOffBoard = false; 
    [SerializeField]
    public Stat cut;
    [SerializeField]
    public CanvasGroup cutGroup;
    #endregion



    #region InitialFunctions
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        moveSpeed = GameObject.Find("Player").GetComponent<PlayerController>().moveSpeed;
        holdPoint = downHoldPoint;

        // variables used to show cutting bar
        cut.Initialize(100f, 100f);
        cut.MyCurrentValue = 100f;
        cutGroup.alpha = 0;

        currObjList = new Dictionary<GameObject, float>();
    }


    void Update() {
        /* TODO: Find the closest object in your list and set it as currObj */
        float minDist = int.MaxValue;
        GameObject localCurrObj = null;

        if (currObjList.Count != 0 && grabbed == false) {
            foreach (GameObject obj in currObjList.Keys) {
                if (obj != null) {
                    float currDist = Vector3.Distance(holdPoint.position, obj.transform.position);
                    if (currDist < minDist)
                    {
                        localCurrObj = obj;
                        currObj = obj;
                        minDist = currDist;
                    }
                }
            }
        }


        float x = playerAnim.GetFloat("XDirection");
        float y = playerAnim.GetFloat("YDirection");

        if (currObj) {
            ChangeHoldPoint(x, y);
        }

        CheckInteract(x, y);

        // used to update cutting bar
        if (isCutting) {
            cutGroup.alpha = 1;
            cut.MyCurrentValue = cutTiming;
        } else {
            cutGroup.alpha = 0;
        }

        /* For debugging */
        //string stringBuilder = "";
        //foreach (GameObject obj in currObjList.Keys) {
        //    stringBuilder += obj.name;
        //}
        //Debug.Log(stringBuilder);
    }
    #endregion


    #region InteractFunctions
    void CheckInteract(float x, float y) {

        // Check if the player is in the zone of an item and if the player clicks "E"/"Interact"
        if (Input.GetButtonDown("Interact") && currObj && !pressedEelsewhere && (canGrabOffBoard || !nearCuttingBoard))
        {
            canGrabOffBoard = false;
            // Grabs the item and puts it in the player's hands
            if (!grabbed)
            {
                currObj.transform.position = holdPoint.position;
                currObj.GetComponent<SpriteRenderer>().sortingLayerName = "HeldIngredient";
                grabbed = true;
            }
            else
            {
                // Cast a ray in the direction of the key pressed 
                // Check if the ray collides with a counter/cooking station/plate
                // If it does, change its position to the position of that
                hits = CheckHits(x, y);

                // Drop item on the counter if the ray hit a counter and there isn't already an item there
                bool hitCounter = false;
                bool hitInteractable = false;
                Transform hitObj = null;
                if (hits.Length > 0) {
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null && hit.collider.gameObject.tag == "Counter" && !hitCounter)
                        {
                            hitObj = hit.collider.gameObject.transform;
                            hitCounter = true;
                            hits = new RaycastHit2D[0];
                        }
                        if (hit.collider != null && 
                            (hit.collider.gameObject.tag == "Ingredient" || hit.collider.gameObject.tag == "Dish" || hit.collider.gameObject.tag == "Uncookable") &&
                            hit.collider.gameObject.transform != currObj.transform) {
                            hitInteractable = true;
                        }
                    }
                }


                // Drops the item
                /* TODO: When you get a plate from plate box and drop it, it contiously does this */
                if (!hitCounter) {
                    currObj.transform.position = holdPoint.position + Vector3.down;
                } else if (hitObj != null && !hitInteractable) {
                    currObj.transform.position = hitObj.position;
                }

                grabbed = false;
                currObj.GetComponent<SpriteRenderer>().sortingLayerName = "Ingredient";
                //currObj.transform.parent = transform.root;
            }
        }

        // If the player is currently grabbing an object, have the object be held in their hands
        if (grabbed && currObj) {
            currObj.transform.position = holdPoint.position;
            pressedEelsewhere = false;
        }
    }


    void ChangeHoldPoint(float x, float y) {
        float compx = 1f * moveSpeed;
        float sprintCompx = 2f * moveSpeed;
        float compy = 1f * moveSpeed;
        float sprintCompy = 2f * moveSpeed;

        if (grabbed && (y == -compy || y == 0 || y == -sprintCompy))
        {
            currObj.GetComponent<SpriteRenderer>().sortingLayerName = "HeldIngredient";
        }
        else if (grabbed && (y != -compy || y != -sprintCompy))
        {
            currObj.GetComponent<SpriteRenderer>().sortingLayerName = "Ingredient";
        }

        // Player is facing right
        if ((x == compx || x == sprintCompx) && y == 0)
        {
            holdPoint = rightHoldPoint;
        }
        // Player is facing left
        if ((x == -compx || x == -sprintCompx) && y == 0)
        {
            holdPoint = leftHoldPoint;
        }
        // Player is facing up
        if (x == 0 && (y == compy || y == sprintCompy))
        {
            holdPoint = upHoldPoint;
        }
        // Player is facing down
        if (x == 0 && (y == -compy || y == -sprintCompy))
        {
            holdPoint = downHoldPoint;
        }
        // Player is facing up/right
        if ((x == compx && y == compy) || (x == sprintCompx && y == sprintCompy))
        {
            holdPoint = upRightHoldPoint;
        }
        // Player is facing up/left
        if ((x == -compx && y == compy) || (x == -sprintCompx && y == sprintCompy))
        {
            holdPoint = upLeftHoldPoint;
        }
        // Player is facing down/right
        if ((x == compx && y == -compy) || (x == sprintCompx && y == -sprintCompy))
        {
            holdPoint = downRightHoldPoint;
        }
        // Player is facing down/left
        if ((x == -compx && y == -compy) || (x == -sprintCompx && y == -sprintCompy))
        {
            holdPoint = downLeftHoldPoint;
        }
    }

    RaycastHit2D[] CheckHits(float x, float y) {
        float compx = 1f * moveSpeed;
        float compy = 1f * moveSpeed;
        RaycastHit2D[] hits = new RaycastHit2D[0];

        // Player is facing right
        if (x == compx && y == 0)
        {
            hits = Physics2D.RaycastAll(holdPoint.position + Vector3.down, Vector2.right * transform.localScale.x, distance);
        }
        // Player is facing left
        if (x == -compx && y == 0)
        {
            hits = Physics2D.RaycastAll(holdPoint.position + Vector3.down, Vector2.left * transform.localScale.x, distance);
        }
        // Player is facing up
        if (x == 0 && y == compy)
        {
            hits = Physics2D.RaycastAll(holdPoint.position, Vector2.up * transform.localScale.x, distance);
        }
        // Player is facing down
        if (x == 0 && y == -compy)
        {
            hits = Physics2D.RaycastAll(holdPoint.position, Vector2.down * transform.localScale.x, 1f + distance);
        }
        // Player is facing up/right
        if (x == compx && y == compy)
        {
            hits = Physics2D.RaycastAll(holdPoint.position, new Vector2(1, 1) * transform.localScale.x, distance);
        }
        // Player is facing up/left
        if (x == -compx && y == compy)
        {
            hits = Physics2D.RaycastAll(holdPoint.position, new Vector2(-1, 1) * transform.localScale.x, distance);
        }
        // Player is facing down/right
        if (x == compx && y == -compy)
        {
            hits = Physics2D.RaycastAll(holdPoint.position, new Vector2(1, -1) * transform.localScale.x, 1f + distance);
        }
        // Player is facing down/left
        if (x == -compx && y == -compy)
        {
            hits = Physics2D.RaycastAll(holdPoint.position, new Vector2(-1, -1) * transform.localScale.x, 1f + distance);
        }
        return hits;
    }

    #endregion


    #region TriggerFunctions
    /* TODO: Option 2: Keep track of a list of objects and then 
     * when you want to pick up an object pick up the closest one*/
    void OnTriggerEnter2D(Collider2D obj) {
        //// Enter collider of an ingredient
        //if (obj.CompareTag("Plate") && currObj == null)
        //{
        //    currObj = obj.gameObject;
        //}
        //if (obj.CompareTag("Dish") && currObj == null)
        //{
        //    currObj = obj.gameObject;
        //}
        //if (obj.CompareTag("Uncookable") && currObj == null)
        //{
        //    currObj = obj.gameObject;
        //}
        //if (obj.CompareTag("Ingredient") && currObj == null)
        //{
        //    currObj = obj.gameObject;
        //}

        //if (validObjTags.Contains(obj.tag) && currObj == null)
        if (validObjTags.Contains(obj.tag))
        {
            //currObj = obj.gameObject;

            /* TODO: Add it to a dictionary instead */
            currObjList.Add(obj.gameObject, 1);
            Debug.Log("Added: " + obj.name);
        }
    }


    void OnTriggerExit2D(Collider2D obj) {
        // Exit collider of an ingredient
        //if (obj.CompareTag("Ingredient")) {
        //    if (obj.gameObject == currObj) {
        //        currObj = null;
        //    }
        //}
        //if (obj.CompareTag("Dish"))
        //{
        //    if (obj.gameObject == currObj)
        //    {
        //        currObj = null;
        //    }
        //}
        //if (obj.CompareTag("Plate"))
        //{
        //    if (obj.gameObject == currObj)
        //    {
        //        currObj = null;
        //    }
        //}
        //if (obj.CompareTag("Uncookable"))
        //{
        //    if (obj.gameObject == currObj)
        //    {
        //        currObj = null;
        //    }
        //}

        //if (validObjTags.Contains(obj.tag) && obj.gameObject == currObj)
        if (validObjTags.Contains(obj.tag))
        {
            //currObj = null;

            /* TODO: Remove it from the dictionary instead */
            currObjList.Remove(obj.gameObject);
            Debug.Log("Removed: " + obj.name);

            bool flag = true;
            foreach(GameObject k in currObjList.Keys) {
                if (k != null) {
                    flag = false;
                }
            }

            if (currObjList.Count == 0 || flag) {
                currObj = null;
            }
        }
    }
    #endregion

    public void TakeItem(GameObject objectToBeTaken)
    {
        currObj = Instantiate(objectToBeTaken);
        grabbed = true; 

    }
}