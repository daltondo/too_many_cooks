using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnManager : MonoBehaviour
{
    #region spawningVars
    System.Random rnd;
    public GameObject zombie;
    public GameObject[] zombies;

    public int maxNumOfZombies;
    public int currNumOfZombies;
    #endregion

    #region mapBounds
    public Transform topRight;
    public Transform bottomLeft;

    private float leftX;
    private float rightX;
    private float bottomY;
    private float topY;

    public Transform cookingTopRight;
    public Transform cookingBottomLeft;
    private float cookingLeftX;
    private float cookingRightX;
    private float cookingBottomY;
    private float cookingTopY;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
        maxNumOfZombies = 6;
        //currNumOfZombies = 0;

        leftX = bottomLeft.position[0];
        rightX = topRight.position[0];
        bottomY = bottomLeft.position[1];
        topY = topRight.position[1];

        cookingLeftX = cookingBottomLeft.position[0];
        cookingRightX = cookingTopRight.position[0];
        cookingBottomY = cookingBottomLeft.position[1];
        cookingTopY = cookingTopRight.position[1];

        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (true)
        {
            if (currNumOfZombies < maxNumOfZombies) {

                // Randomly pick the type of zombie to spawn
                int coinFlip = Random.Range(1, 11);
                GameObject currZombie;
                if (coinFlip <= 4)
                {
                    currZombie = zombies[0];
                }
                else if (coinFlip <= 7)
                {
                    currZombie = zombies[1];
                }
                else
                {
                    currZombie = zombies[2];
                }

                /* TODO: FIGURE OUT WHATS CRASHING*/
                // Everything above this line doesn't crash

                //bool cont = true;
                Vector2 randomPos = new Vector2();

                // Check if spawned location already has an object in it
                //while (cont)
                //{

                // Spawn the zombie in a random location within the bounds of the box
                float randomX = 0;
                float randomY = 0;
                if (currZombie.name == "MessyZombie")
                {
                    randomX = Random.Range(cookingLeftX, cookingRightX);
                    randomY = Random.Range(cookingBottomY, cookingTopY);
                }
                else
                {
                    randomX = Random.Range(leftX, rightX);
                    randomY = Random.Range(bottomY, topY);
                }

                randomPos = new Vector2(randomX, randomY);

                    //RaycastHit2D hitObj = Physics2D.CircleCast(randomPos, 3, new Vector2(0, 0));
                    //if (hitObj.collider == null)
                    //{
                    //    cont = false;
                    //}
                //}

                Instantiate(currZombie, randomPos, Quaternion.identity);
                currNumOfZombies += 1;
            }
            // pick a random time within the range to spawn the next zombie
            float randomTime = Random.Range(7f, 15f);
            yield return new WaitForSeconds(randomTime);
        }
    }
}
