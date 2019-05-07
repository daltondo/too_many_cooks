using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region ScoreVariables
    public int dishesServed;
    public int totalTips;
    public Text scoreText;
    #endregion

    #region GameTimeVariables
    public float timeLeft = 10;
    public Text timeText;
    #endregion


    private void Start()
    {
        dishesServed = 0;
        totalTips = 0;
        //scoreText.text = dishesServed.ToString();
        scoreText.text = "$" + totalTips.ToString();
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        // update the time in UI
        timeText.text = Mathf.Round(timeLeft).ToString();

        // if time is over then end game
        if (timeLeft < 0) {
            PlayerStats.Score = totalTips;
            GameObject gm = GameObject.FindWithTag("GameController");
            gm.GetComponent<GameManager>().WinGame();
        }
    }

    public void updateDishesServed(int numDishes) {
        dishesServed += numDishes;
        //scoreText.text = dishesServed.ToString();
    }

    public void accumulateTips(int tip) {
        totalTips += tip;
        scoreText.text = "$" + totalTips.ToString();
    }
}
