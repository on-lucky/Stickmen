using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController instance;

    public Camera UIcam;
    public Camera MenuCam;

    public List<Camera> Adjustable_cameras;
    public bool mouseHold = false;

    public float cameraLimitMin = 0;
    public float cameraLimitMax =10;

    public float fieldLimitX = 10;
    public float fieldLimitY = 10;

    private Camera mainCam;
    private float mouseTimer = 0f;
    private bool isPressed = false;
    private Vector3 startingMousePos;
    private Vector3 startingCamPos;
    private float startingCamSize;
    private float lastRatio =-1;
    private bool isEnabled = true;

    private void Awake()
    {
        if (CameraController.instance != null)
        {
            Debug.LogError("More than one CameraController in the scene!");
        }
        CameraController.instance = this;
        mainCam = Camera.main;
        startingCamSize = UIcam.orthographicSize;
    }

    private void Update()
    {
        if (isEnabled && Input.GetMouseButtonDown(0))
        {
            isPressed = true;
        }
        else if (isEnabled && Input.GetMouseButtonUp(0))
        {
            isPressed = false;
            mouseTimer = 0;
            mouseHold = false;
        }
        else if (isPressed)
        {
            UpdateTimer();
        }
        if(mouseHold)
        {
            UpdatePosition();
        }
        UpdateZoom();
    }

    private void UpdateTimer()
    {
        mouseTimer += Time.deltaTime;
        if(mouseTimer >= 0.1 && !mouseHold)
        {
            mouseHold = true;
            SetStartingPos();
        }
    }

    private void SetStartingPos()
    {
        startingMousePos = UIcam.ScreenToWorldPoint(Input.mousePosition);
        startingCamPos = this.transform.localPosition;
    }

    private void UpdatePosition()
    {
        transform.Translate(new Vector3(-Input.GetAxis("Mouse X") * UIcam.orthographicSize, -Input.GetAxis("Mouse Y") * UIcam.orthographicSize, 0));

        AdjustCamPos();
    }

    private void UpdateZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            //cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel");
            mainCam.transform.Translate(new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel")));

            AdjustCamZoom();

            float ratio = UIcam.orthographicSize / startingCamSize;
            if(lastRatio != -1)
            {
                float temp = ratio;
                ratio = ratio / lastRatio;
                lastRatio = temp;
            }
            else
            {
                lastRatio = ratio;
            }

            SideBarScaler.instance.ChangeScale(ratio);

            AdjustCamPos();
        }
    }

    public void AdjustCamPos()
    {
        float verticalFOV = mainCam.fieldOfView * Mathf.Deg2Rad;
        float horizontalFOV = 2 * Mathf.Atan(Mathf.Tan((verticalFOV) / 2) * mainCam.aspect);
        float cameraDist = Mathf.Abs(mainCam.transform.position.z);

        float camLimitX = fieldLimitX - (cameraDist * Mathf.Atan(horizontalFOV / 2));
        if(camLimitX < 0)
        {
            camLimitX = 0;
        }

        float camLimitY = fieldLimitY - (cameraDist * Mathf.Atan(verticalFOV / 2));
        if (camLimitY < 0)
        {
            camLimitY = 0;
        }

        if(mainCam.transform.position.x < -camLimitX)
        {
            mainCam.transform.position = new Vector3(-camLimitX, mainCam.transform.position.y, mainCam.transform.position.z);
        }
        if (mainCam.transform.position.x > camLimitX)
        {
            mainCam.transform.position = new Vector3(camLimitX, mainCam.transform.position.y, mainCam.transform.position.z);
        }
        if (mainCam.transform.position.y < -camLimitY)
        {
            mainCam.transform.position = new Vector3(mainCam.transform.position.x, -camLimitY, mainCam.transform.position.z);
        }
        if (mainCam.transform.position.y > camLimitY)
        {
            mainCam.transform.position = new Vector3(mainCam.transform.position.x, camLimitY, mainCam.transform.position.z);
        }
    }

    public void AdjustCamZoom()
    {
        if (mainCam.transform.position.z <= cameraLimitMin)
        {
            mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, cameraLimitMin);
        }
        else if (mainCam.transform.position.z >= cameraLimitMax)
        {
            mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, cameraLimitMax);
        }
    }

    public void SetCamRect(Rect rect) {
        foreach(Camera cam in Adjustable_cameras)
        {
            cam.rect = rect;
        }
    }

    public void EnableControler(bool shouldBeEnabled)
    {
        isEnabled = shouldBeEnabled;
    }
        
}
