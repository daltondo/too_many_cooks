using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinGameScoreManager : MonoBehaviour
{
    [SerializeField]
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "$" + PlayerStats.Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "$" + PlayerStats.Score.ToString();
    }
}
