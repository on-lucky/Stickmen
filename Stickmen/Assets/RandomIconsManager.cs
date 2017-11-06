using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIconsManager : MonoBehaviour {

    public static RandomIconsManager instance;

    public Vector3[] player1IconPositions;
    public Vector3[] player2IconPositions;

    public float timeBetweenReveals = 3f;

    public GameObject IconTemplate;

    private void Awake()
    {
        if (RandomIconsManager.instance != null)
        {
            Debug.LogError("More than one RandomIconsManager in the scene!");
        }
        RandomIconsManager.instance = this;
    }

    public void SpawnRandomIcons(List<Power> Player1Powers, List<Power> Player2Powers)
    {
        for(int i = 0 ; i < Player1Powers.Count; i++)
        {
            RandomPowerIcon randomIcon = Instantiate(IconTemplate, player1IconPositions[i], this.transform.rotation).GetComponent<RandomPowerIcon>();
            randomIcon.finalPower = Player1Powers[i];
            randomIcon.timeToWait = timeBetweenReveals * (i + 1);
        }

        for (int i = 0; i < Player2Powers.Count; i++)
        {
            RandomPowerIcon randomIcon = Instantiate(IconTemplate, player2IconPositions[i], this.transform.rotation).GetComponent<RandomPowerIcon>();
            randomIcon.finalPower = Player2Powers[i];
            randomIcon.timeToWait = timeBetweenReveals * (i + 1);
        }
    }
}
