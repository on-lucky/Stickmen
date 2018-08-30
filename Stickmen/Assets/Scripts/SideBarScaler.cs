using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBarScaler : MonoBehaviour {

    public static SideBarScaler instance;

    public GameObject background;
    public GameObject selectionForeground;
    public GameObject libraryForeground;
    public GameObject slideButton;

    [Range(0, 1)]
    public float screenPortion = 0.2f;

    public float libraryHeight = 8;

    public float buttonHeight = -1;
    public float buttonScale = 1;

    private const float BORDERWIDTH = 0.05f;
    private float currentPortion = 0f;

    private Camera cam;

    private float startingScale = 1;

    private Slider _slider;

    private void Awake()
    {
        if (SideBarScaler.instance != null)
        {
            Debug.LogError("More than one SideBarScaler in the scene!");
        }
        SideBarScaler.instance = this;
        _slider = GetComponent<Slider>();
        cam = CameraController.instance.UIcam;
    }

    // Use this for initialization
    void Start () {
        PlaceSideBar();
        ScaleSideBar();
    }

    private void Update()
    {
        UpdateBottomUI();
        UpdateCamera();
    }

    public void PlaceSideBar()
    {
        float camWidth = cam.orthographicSize * cam.aspect;
        background.transform.localPosition = new Vector3(camWidth + (camWidth * screenPortion), 0, 0);

        float libraryPosY = -cam.orthographicSize + BORDERWIDTH + (libraryHeight / 2);
        libraryForeground.transform.localPosition = new Vector3(camWidth + (camWidth * screenPortion), libraryPosY, -0.5f);

        float selectionHeight = (2f * cam.orthographicSize - libraryHeight) - (BORDERWIDTH * 3);
        float selectionPosY = cam.orthographicSize - BORDERWIDTH - (selectionHeight / 2);
        selectionForeground.transform.localPosition = new Vector3(camWidth + (camWidth * screenPortion), selectionPosY, -0.5f);

        slideButton.transform.localPosition = new Vector3(camWidth - (buttonScale / 2), buttonHeight, 0);


        _slider.SetCurrentPos(0);
        // _slider.SetCurrentPos(camWidth * screenPortion * transform.localScale.x);
        //_slider.SetGoalPos(camWidth * screenPortion * transform.localScale.x);

        //CameraController.instance.SetCamRect(new Rect(0, 0, (1 - currentPortion), 1));
    }

	public void ScaleSideBar()
    {
        float height = 2f * cam.orthographicSize;
        float camWidth = height * cam.aspect;

        background.transform.localScale = new Vector3(camWidth * screenPortion, height, 1);
        libraryForeground.transform.localScale = new Vector3(camWidth * screenPortion - (BORDERWIDTH * 2), libraryHeight, 1);
        selectionForeground.transform.localScale = new Vector3(camWidth * screenPortion - (BORDERWIDTH * 2), (height - libraryHeight) - (BORDERWIDTH * 3), 1);
        slideButton.transform.localScale = new Vector3(buttonScale, buttonScale, buttonScale);
    }

    public void Slide(bool goingRight)
    {
        float camWidth = cam.orthographicSize * cam.aspect * 2f;
        if (goingRight)
        {
            _slider.SetGoalPos(0);
        }
        else{
            _slider.SetGoalPos(-camWidth * screenPortion * transform.localScale.x);
        }
    }

    private void UpdateBottomUI()
    {
        if (_slider.currentPosX != _slider.goalPosX)
        {
            BottomUIManager.instance.ScaleUI(1 - currentPortion);
        }
    }

        private void UpdateCamera()
    {
        if (_slider.currentPosX != _slider.goalPosX)
        {
            float changeInPos = _slider.slidingSpeed * Time.deltaTime * transform.localScale.x;
            float camWidth = cam.orthographicSize * cam.aspect * 2f;
            float changeInPortion = changeInPos / camWidth;

            if (_slider.currentPosX > _slider.goalPosX)
            {
                currentPortion += changeInPortion;
                CameraController.instance.AdjustCamPos();
            }
            else if (_slider.currentPosX < _slider.goalPosX)
            {
                currentPortion -= changeInPortion;
                CameraController.instance.AdjustCamPos();
            }

            if (currentPortion > screenPortion)
            {
                currentPortion = screenPortion;
            }
            else if (currentPortion < 0)
            {
                currentPortion = 0;
            }

            CameraController.instance.SetCamRect(new Rect(0, 0, (1 - currentPortion), 1));
        }
    }

    public void ChangeScale(float ratio)
    {
        transform.localScale = new Vector3(transform.localScale.x * ratio, transform.localScale.y * ratio, transform.localScale.z * ratio);
        transform.localPosition = new Vector3(transform.localPosition.x * ratio, transform.localPosition.y, transform.localPosition.z);

        _slider.SetCurrentPos(_slider.currentPosX * ratio);

        _slider.SetGoalPos(_slider.goalPosX * ratio);
    }
}
