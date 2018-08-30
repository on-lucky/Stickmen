using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCursor : MonoBehaviour {

    public Camera UICam;
    public TimeBarSlider timeBar;
    private Vector2 XLimits;

    private bool dragging = false;
    

    private void Start()
    {
        XLimits[0] = transform.position.x;
        XLimits[1] = transform.position.x;
    }

    // Update is called once per frame
    void Update () {
        if (dragging)
        {
            DragCursor();
        }
	}

    private void OnMouseDown()
    {
        CameraController.instance.EnableControler(false);
        dragging = true;
        TimeController.instance.StartRewind();
    }

    private void OnMouseUp()
    {
        CameraController.instance.EnableControler(true);
        dragging = false;
        TimeController.instance.StopRewind();
    }

    private void DragCursor()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 worldPos = UICam.ScreenToWorldPoint(mousePos);
        
        transform.position = AdjustPosition(worldPos);
        timeBar.UpdateCursorTime(CalculateRatio(transform.position.x));

        ApplyRewind();
    }

    private Vector3 AdjustPosition(Vector3 worldPos)
    {
        worldPos.z = transform.position.z;
        worldPos.y = transform.position.y;
        if(worldPos.x < XLimits[0])
        {
            worldPos.x = XLimits[0];
        }
        else if (worldPos.x > XLimits[1])
        {
            worldPos.x = XLimits[1];
        }
        return worldPos;
    }

    public void ApplyRewind()
    {
        TimeController.instance.RewindTo(CalculateRatio(transform.position.x));
    }

    private float CalculateRatio(float posX)
    {
        if(XLimits[1] - XLimits[0] == 0)
        {
            return 0;
        }
        return (posX - XLimits[0]) / (XLimits[1] - XLimits[0]);
    }

    public void UpdateLimits()
    {
        XLimits[1] = transform.position.x;
    }

    public void PlaceCursor(float posX)
    {
        transform.localPosition = new Vector3(posX, transform.localPosition.y, transform.localPosition.z);
    }
}
