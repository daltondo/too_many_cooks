using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{

    private Image content;
    private float currentFill;
    [SerializeField]
    private Text statValue;
    [SerializeField]
    private float lerpSpeed;


    public float MyMaxValue { get; set; }
    private float currentValue;

    // Property for setting the current value, this has to be used every time we change the currentValue, so that everything updates correctly
    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            if (value > MyMaxValue)//Makes sure that we don't get too much health
            {
                currentValue = MyMaxValue;
            }
            else if (value < 0) //Makes sure that we don't get health below 0
            {
                currentValue = 0;
            }
            else //Makes sure that we set the current value withing the bounds of 0 to max health
            {
                currentValue = value;
            }

            //Calculates the currentFill, so that we can lerp
            currentFill = currentValue / MyMaxValue;

            if (statValue != null)
            {
                //Makes sure that we update the value text
                statValue.text = currentValue + " / " + MyMaxValue;
            }

        }
    }

    // Use this for initialization
    void Start()
    {
        //Creates a reference to the content
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //Makes sure that we update the bar
        HandleBar();
    }


    public void Initialize(float currentValue, float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();
        }

        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue;
    }
     
    private void HandleBar()
    {
        if (currentFill != content.fillAmount) //If we have a new fill amount then we know that we need to update the bar
        {
            //Lerps the fill amount so that we get a smooth movement
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }

    }
}
