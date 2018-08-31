using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    public bool isInfinite = false;

    public ParticleSystem[] particleSystems;
    public GameObject particleObject;

    public float duration = 3;
    public float deathDuration = 10;

    private List<float> startingRates = new List<float>();
    private List<float> startingSizes = new List<float>();
    private Vector3 statingObjectScale;

    private float currentTime;
    private float dying;
    private bool isDying;

	// Use this for initialization
	void Start () {
        currentTime = duration;
        SetStartingRates();
        SetStartingSizes();
        statingObjectScale = particleObject.transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isInfinite)
        {
            currentTime -= Time.deltaTime;
            if (!isDying && currentTime < 0)
            {
                isDying = true;
                currentTime = deathDuration;
            }
            else if (isDying)
            {
                AdjustParticleSystems();
                ScaleParticleObj();
                if (currentTime <= 0)
                {
                    DieOut();
                }
            }
        }
	}

    private void SetStartingRates()
    {
        foreach (ParticleSystem system in particleSystems)
        {
            startingRates.Add(system.emission.rateOverTime.constant);
        }
    }

    private void SetStartingSizes()
    {
        foreach (ParticleSystem system in particleSystems)
        {
            startingSizes.Add(system.main.startSize.constant);
        }
    }

    private void ScaleParticleObj()
    {
        float ratio = currentTime / deathDuration;

        particleObject.transform.localScale = new Vector3(statingObjectScale.x, Mathf.Max(statingObjectScale.y * ratio, 0.1f), statingObjectScale.z);
    }

    private void AdjustParticleSystems()
    {
        for(int i =0; i< particleSystems.Length; i++)
        {
            float ratio = currentTime / deathDuration;

            float size = startingSizes[i] * (ratio);
            float rate = startingRates[i] * (ratio);

            var main = particleSystems[i].main;
            var emission = particleSystems[i].emission;

            main.startSize = size;
            //emission.rateOverTime = rate;
        }
    }

    private void DieOut()
    {
        foreach (ParticleSystem system in particleSystems)
        {
            var emission = system.emission;
            emission.rateOverTime = 0;
            Destroy(this.gameObject, 5);
        }
    }
}
