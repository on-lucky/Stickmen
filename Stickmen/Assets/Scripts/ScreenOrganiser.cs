using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScreenOrganiser : MonoBehaviour {

    public static ScreenOrganiser instance;

    public GameObject[] horizontalBars;
    public GameObject[] CharacterFrames;
    public List<GameObject> players;
    public MenuElement[] buttons;
    public MenuElement[] GUIElements;

    public float zoomSpeed = 1f;
    public float zoomFactor = 6f;
    public float slidingSpeed = 1f;

    private Camera cam;
    private GameObject timer;
    private float goalSize;

    private float distance1;
    private float distance2;
    private float goalDistance1;
    private float goalDistance2;

    private bool isZooming = false;
    private bool isSliding = false;

    private const float BORDEROFFSET = 0.5f;

    private void Awake()
    {
        if (ScreenOrganiser.instance != null)
        {
            Debug.LogError("More than one ScreenOrganiser in the scene!");
        }
        ScreenOrganiser.instance = this;
        cam = Camera.main;
        timer = GetComponentInChildren<ProfileTimer>().gameObject;
        players = new List<GameObject>();
    }

    private void Start()
    {
        AdjustFramesPositions();
        AdjustTimerPosition();
        RandomIconsManager.instance.AdjustIconsPositions();
    }

    public void FramePlayer(GameObject player, bool isMyplayer)
    {
        if (isMyplayer)
        {
            player.transform.position = CharacterFrames[0].transform.position;
            //player.transform.parent = CharacterFrames[0].transform;
        }
        else{
            player.transform.position = CharacterFrames[1].transform.position;
            //player.transform.parent = CharacterFrames[1].transform;
        }
    }

    public void AddPlayerObject(GameObject player)
    {
        players.Add(player);
        DontDestroyOnLoad(player);
        FramePlayer(player, player.GetComponent<NetworkIdentity>().isLocalPlayer);
    }

    public void ZoomToSize(float size)
    {
        goalSize = size;
        isZooming = true;
    }

    public void SlideBars(int player1PowerCount, int player2PowerCount)
    {
        float width = RandomIconsManager.instance.IconTemplate.GetComponent<RandomPowerIcon>().width;
        float spacing = RandomIconsManager.instance.iconSpacing;

        distance1 = (width + spacing) * player1PowerCount;
        distance2 = (width + spacing) * player2PowerCount;

        goalDistance1 = (width + spacing) * player1PowerCount;
        goalDistance2 = (width + spacing) * player2PowerCount;

        isSliding = true;
    }

    private void AdjustTimerPosition()
    {
        Vector3 posCorner= cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float posxFrame2 = posCorner.x - timer.transform.localScale.x / 2 - BORDEROFFSET;
        float posyFrame2 = posCorner.y - timer.transform.localScale.y / 2 - BORDEROFFSET;
        timer.transform.position = new Vector3(posxFrame2, posyFrame2, timer.transform.position.z);
    }

    private void AdjustFramesPositions()
    {
        Vector3 posCornerLeft = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        float posxFrame1 = posCornerLeft.x + CharacterFrames[0].transform.localScale.x / 2 + BORDEROFFSET;
        float posyFrame1 = posCornerLeft.y - CharacterFrames[0].transform.localScale.y / 2 - BORDEROFFSET;
        CharacterFrames[0].transform.position = new Vector3(posxFrame1, posyFrame1, CharacterFrames[0].transform.position.z);
        if (players.Count > 0)
        {
            players[0].transform.position = new Vector3(posxFrame1, posyFrame1, CharacterFrames[0].transform.position.z);
        }

        Vector3 posCornerRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        float posxFrame2 = posCornerRight.x - CharacterFrames[1].transform.localScale.x / 2 - BORDEROFFSET;
        float posyFrame2 = posCornerRight.y + CharacterFrames[1].transform.localScale.y / 2 + BORDEROFFSET;
        CharacterFrames[1].transform.position = new Vector3(posxFrame2, posyFrame2, CharacterFrames[1].transform.position.z);
        if (players.Count > 1)
        {
            players[1].transform.position = new Vector3(posxFrame2, posyFrame2, CharacterFrames[1].transform.position.z);
        }
    }

    private void UpdateZoom()
    {
        if (cam.orthographicSize < goalSize)
        {
            cam.orthographicSize += (Time.deltaTime * zoomSpeed);
            if (cam.orthographicSize >= goalSize)
            {
                cam.orthographicSize = goalSize;
                isZooming = false;
                MakeEveryThingAppear();
                timer.GetComponent<ProfileTimer>().StartTimer();
            }
        }
        else if (cam.orthographicSize > goalSize)
        {
            cam.orthographicSize -= (Time.deltaTime * zoomSpeed);
            if (cam.orthographicSize <= goalSize)
            {
                cam.orthographicSize = goalSize;
                isZooming = false;
                MakeEveryThingAppear();
                timer.GetComponent<ProfileTimer>().StartTimer();
            }
        }
    }

    private void MakeEveryThingAppear()
    {
        StartCoroutine(MakeButtonsAppear());
        MakeGUIElementsAppear();
    }

    private void MakeGUIElementsAppear()
    {
        foreach (MenuElement GUIElem in GUIElements)
        {
            GUIElem.Appear();
        }
    }

    private IEnumerator MakeButtonsAppear()
    {
        foreach(MenuElement button in buttons)
        {
            button.Appear();
            yield return StartCoroutine(WaitForButton());
        }

    }

    private IEnumerator WaitForButton()
    {
        yield return new WaitForSeconds(0.4f);
    }

    private void UpdateSlide()
    {
        float slideDistance1 = (distance1 / goalDistance1) * slidingSpeed;
        distance1 -= slideDistance1;
        horizontalBars[0].transform.Translate(new Vector3(slideDistance1, 0, 0));

        float slideDistance2 = (distance2 / goalDistance2) * slidingSpeed;
        distance2 -= slideDistance2;
        horizontalBars[1].transform.Translate(new Vector3(slideDistance2, 0, 0));

        if (distance2 <= 0.01)
        {
            isSliding = false;
        }

    }

    public float GetHeight(int playerIndex)
    {
        return CharacterFrames[playerIndex].transform.position.y;
    }

    public void SetProfileScreen()
    {
        ZoomToSize(zoomFactor);
    }

    private void Update()
    {
        if (isZooming)
        {
            UpdateZoom();
            AdjustFramesPositions();
            AdjustTimerPosition();
        }

        if (isSliding)
        {
            UpdateSlide();
        }
    }
}
