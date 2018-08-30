using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour {

    public float slidingSpeed = 1f;
    public bool Xoriented = true;

    public float goalPosX = 0;
    public float currentPosX= 0;

    public float goalPosY = 0;
    public float currentPosY = 0;

    private bool isSliding = false;
	
	// Update is called once per frame
	void Update () {
        if (isSliding)
        {
            UpdatePosition();
        }
    }

    public void SetGoalPos(float goal, char axis = 'x')
    {
        if(axis == 'x')
        {
            goalPosX = goal * transform.localScale.x;
            if (goalPosX != currentPosX)
            {
                isSliding = true;
            }
        }
        else
        {
            goalPosY = goal * transform.localScale.y;
            if (goalPosY != currentPosY)
            {
                isSliding = true;
            }
        }
    }

    public void SetCurrentPos(float pos)
    {
        currentPosX = pos;
    }

    private void UpdatePosition()
    {
        float changeInPos = slidingSpeed * Time.deltaTime * transform.lossyScale.x;

        if (currentPosX > goalPosX)
        {
            transform.Translate(new Vector3(-changeInPos, 0, 0));
            currentPosX -= changeInPos / transform.lossyScale.x;
            if (currentPosX <= goalPosX)
            {
                this.transform.localPosition = new Vector3(goalPosX, this.transform.localPosition.y, this.transform.localPosition.z);
                currentPosX = goalPosX;
                isSliding = false;
            }
        }
        else if (currentPosX < goalPosX)
        {
            transform.Translate(new Vector3(changeInPos, 0, 0));
            currentPosX += changeInPos / transform.lossyScale.x;
            if (currentPosX >= goalPosX)
            {
                this.transform.localPosition = new Vector3(goalPosX, this.transform.localPosition.y, this.transform.localPosition.z);
                currentPosX = goalPosX;
                isSliding = false;
            }
        }
        if (currentPosY > goalPosY)
        {
            transform.Translate(new Vector3(0, -changeInPos, 0));
            currentPosY -= changeInPos / transform.lossyScale.y;
            if (currentPosY <= goalPosY)
            {
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, goalPosY, this.transform.localPosition.z);
                currentPosY = goalPosY;
                isSliding = false;
            }
        }
        else if (currentPosY < goalPosY)
        {
            transform.Translate(new Vector3(0, changeInPos, 0));
            currentPosY += changeInPos / transform.lossyScale.y;
            if (currentPosY >= goalPosY)
            {
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, goalPosY, this.transform.localPosition.z);
                currentPosY = goalPosY;
                isSliding = false;
            }
        }
    }

    public void ResetSlider()
    {
        currentPosX = 0;
        currentPosY = 0;

        goalPosX = 0;
        goalPosY = 0;
    }

    public bool VerifyMovement()
    {
        return isSliding;
    }
}
