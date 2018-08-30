using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomUIManager : MonoBehaviour {

    public static BottomUIManager instance;

    public GameObject TimeBar;
    public GameObject LeftProfile;
    public GameObject RightProfile;

    private Vector3 TimeBarStartingPos;
    private Vector3 LeftProfileStartingPos;
    private Vector3 RightProfileStartingPos;

    private Camera cam;

    private void Awake()
    {
        if (BottomUIManager.instance != null)
        {
            Debug.LogError("More than one BottomUIManager in the scene!");
        }
        BottomUIManager.instance = this;
    }

    // Use this for initialization
    void Start () {
        cam = CameraController.instance.UIcam;

        TimeBarStartingPos = TimeBar.transform.localPosition;
        LeftProfileStartingPos = LeftProfile.transform.localPosition;
        RightProfileStartingPos = RightProfile.transform.localPosition;
    }
	
	public void ScaleUI(float scale)
    {
        float camWidth = cam.orthographicSize * cam.aspect * 2f;

        float timeBarPosX = (scale - 1) * camWidth / 2;
        TimeBar.transform.localPosition = new Vector3(timeBarPosX, TimeBarStartingPos.y, TimeBarStartingPos.z);

        float rightProfilePosX = RightProfileStartingPos.x - (camWidth * (1 - scale));
        RightProfile.transform.localPosition = new Vector3(rightProfilePosX, RightProfileStartingPos.y, RightProfileStartingPos.z);
    }
}
