using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIconsManager : MonoBehaviour {

    public static RandomIconsManager instance;

    public Vector3[] player1IconPositions;
    public Vector3[] player2IconPositions;

    public float timeBetweenReveals = 3f;
    public float horixontalOffset = 0f;
    public float iconSpacing = 0f;

    public GameObject IconTemplate;

    private Camera cam;

    private void Awake()
    {
        if (RandomIconsManager.instance != null)
        {
            Debug.LogError("More than one RandomIconsManager in the scene!");
        }
        RandomIconsManager.instance = this;
        cam = Camera.main;
    }

    public void SpawnRandomIcons(List<Power> Player1Powers, List<Power> Player2Powers)
    {
        for(int i = 0 ; i < Player1Powers.Count; i++)
        {
            RandomPowerIcon randomIcon = Instantiate(IconTemplate, player1IconPositions[i], this.transform.rotation).GetComponent<RandomPowerIcon>();
            randomIcon.transform.parent = ScreenOrganiser.instance.CharacterFrames[0].transform;
            randomIcon.finalPower = Player1Powers[i];
            randomIcon.timeToWait = timeBetweenReveals * (i + 1);
        }

        for (int i = 0; i < Player2Powers.Count; i++)
        {
            RandomPowerIcon randomIcon = Instantiate(IconTemplate, player2IconPositions[i], this.transform.rotation).GetComponent<RandomPowerIcon>();
            randomIcon.transform.parent = ScreenOrganiser.instance.CharacterFrames[1].transform;
            randomIcon.finalPower = Player2Powers[i];
            randomIcon.timeToWait = timeBetweenReveals * (i + 1);
        }
        ScreenOrganiser.instance.SlideBars(Player1Powers.Count, Player2Powers.Count);
        StartCoroutine(SetProfileScreen());
    }

    private IEnumerator SetProfileScreen()
    { 
        yield return StartCoroutine(WaitForReveal());
        ScreenOrganiser.instance.SetProfileScreen();
    }

        private IEnumerator WaitForReveal()
    {
        yield return new WaitForSeconds((timeBetweenReveals * 4f) + 1f);
    }

    public void AdjustIconsPositions()
    {
        Vector3 posCornerLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
        for (int i = 0; i < player1IconPositions.Length; i++)
        {
            player1IconPositions[i].x = posCornerLeft.x + (IconTemplate.GetComponent<RandomPowerIcon>().width * ((float)i * (1f + iconSpacing))) + horixontalOffset;
            player1IconPositions[i].y = ScreenOrganiser.instance.GetHeight(0);
        }

        Vector3 posCornerRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        for (int i = 0; i < player2IconPositions.Length; i++)
        {
            player2IconPositions[i].x = posCornerRight.x - (IconTemplate.GetComponent<RandomPowerIcon>().width * ((float)i * (1f + iconSpacing))) - horixontalOffset;
            player2IconPositions[i].y = ScreenOrganiser.instance.GetHeight(1);
        }
    }
}
