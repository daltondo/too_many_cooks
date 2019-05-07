using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    // Called when something enters the trigger collider
    private void OnTriggerEnter2D(Collider2D coll) {
        // Check if coll is the player
        if (coll.CompareTag("Player")) {
            GetComponentInParent<Zombie>().player = coll.transform;
        }
    }
}