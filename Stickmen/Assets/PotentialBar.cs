using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PotentialBar : MonoBehaviour {

    public float minHeight;
    public float maxHeight;
    public float barSpeed = 1f;

    private float currentHeight;
    private float goalHeight;
    private float deltaHeight;
    private TextMeshPro text;

    // Use this for initialization
    void Start () {
        text = GetComponentInChildren<TextMeshPro>();
        deltaHeight = maxHeight - minHeight;
        currentHeight = minHeight;
        goalHeight = minHeight;
        transform.localPosition = new Vector3(transform.localPosition.x, currentHeight, transform.localPosition.z);
    }
	
	// Update is called once per frame
	void Update () {
        UpdateHeight();
	}

    private void UpdateHeight()
    {
        if (currentHeight != goalHeight)
        {
            if(currentHeight < goalHeight)
            {
                currentHeight += barSpeed * Time.deltaTime;
                if (currentHeight > goalHeight)
                    currentHeight = goalHeight;
            }
            else
            {
                currentHeight -= barSpeed * Time.deltaTime;
                if (currentHeight < goalHeight)
                    currentHeight = goalHeight;
            }
            transform.localPosition = new Vector3(transform.localPosition.x, currentHeight, transform.localPosition.z);
        }
    }

    public void AdjustHeight(float ratio)
    {
        goalHeight = minHeight + (ratio * deltaHeight);
        UpdateText(ratio);
    }

    private void UpdateText(float ratio)
    {
        int percentage = Mathf.FloorToInt(ratio * 100);
        text.text = percentage + "%";
    }
}
