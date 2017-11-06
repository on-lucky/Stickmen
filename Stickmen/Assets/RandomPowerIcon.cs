using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomPowerIcon : MonoBehaviour {

    public float timeToWait = 3f;
    public float flashTime = 0.1f;
    public Power finalPower;

    private bool isChanging = false;
    public GameObject icon;
    private float currentTime = 0;
    private int index = 0;

    // Use this for initialization
    void Start () {
        StartCoroutine(StartTimer(timeToWait));
        icon = Instantiate(PowerManager.instance.allPowers[index].icon, this.transform);
	}
	
	// Update is called once per frame
	void Update () {
        if (isChanging)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= flashTime)
            {
                index++;
                Destroy(icon);
                icon = Instantiate(PowerManager.instance.allPowers[index % PowerManager.instance.allPowers.Length].icon, this.transform);
                currentTime = 0;
            }
        }
	}

    private IEnumerator StartTimer(float timeToWait)
    {
        isChanging = true;
        yield return new WaitForSeconds(timeToWait);
        isChanging = false;
        SetFinalPower();
    }

    private void SetFinalPower()
    {
        Destroy(icon);
        icon = Instantiate(finalPower.icon, this.transform);
        GetComponentInChildren<TextMeshPro>().text = finalPower.name;
        GetComponentInChildren<ParticleSystem>().Play();
        GetComponentInChildren<LightSwitch>().LightUP();
        GetComponentInChildren<LightSwitch>().Flash();
    }
}
