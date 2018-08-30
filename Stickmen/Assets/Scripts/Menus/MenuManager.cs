using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public static MenuManager instance;                 // Singleton of the MenuManager

    public Transform[] screens;

    private Camera mainCam;

    private void Awake()
    {
        if(MenuManager.instance != null)
        {
            Debug.LogError("More than one MenuManager in the scene!");
        }
        MenuManager.instance = this;
        mainCam = Camera.main;
    }

    public IEnumerator ChangeScreen(int screenIndex)
    {
        yield return WaitForTime(0.5f);
        mainCam.GetComponent<SmoothFollower>().objective = screens[screenIndex];
    }

    private IEnumerator WaitForTime(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }
}
