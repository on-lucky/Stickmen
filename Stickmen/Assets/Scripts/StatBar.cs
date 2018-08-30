using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBar : MonoBehaviour {

    private float startingXScale;

    private float goalPosition;
    private float goalScale;

    private float currentPosition;
    private float currentScale;

    private float hopDistance;

    public float minimalPosition;
    public int divisionQuantity = 10;

    private void Awake()
    {
        startingXScale = transform.localScale.x;
        currentScale = startingXScale;
        goalScale = startingXScale;
        hopDistance = Mathf.Abs(minimalPosition) / (float)divisionQuantity;
        SetBar(4);
    }

    // Update is called once per frame
    void Update () {
		if(currentScale != goalScale)
        {
            UpdateScale();
        }

        if (currentPosition != goalPosition)
        {
            UpdatePosition();
        }
    }

    private void UpdateScale()
    {
        transform.localScale = new Vector3(goalScale, transform.localScale.y, transform.localScale.z);
        currentScale = goalScale;
        
    }

    private void UpdatePosition()
    {
        transform.localPosition = new Vector3(goalPosition, transform.localPosition.y, transform.localPosition.z);
        currentPosition = goalPosition;
    }

    public void SetBar(int value)
    {
        goalScale = startingXScale * ((float)value / (float)divisionQuantity);
        goalPosition = minimalPosition + (hopDistance * value);
    }
}
