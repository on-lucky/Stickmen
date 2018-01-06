using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialManager : MonoBehaviour {

    public static PotentialManager instance;

    public int maxPotential; // Total points to spend
    public PotentialBar bar; // Potential bar to fill;
    public GameObject feedbackTemplate; // the object that spawn on the mouse when potential is spent or gained

    private int potential; // Remaining points to be spent

    private void Awake()
    {
        if (PotentialManager.instance != null)
        {
            Debug.LogError("More than one PotentialManager in the scene!");
        }
        PotentialManager.instance = this;
    }

    private void Start()
    {
        potential = maxPotential;
        SetStartingStats();
    }

    public bool CanAfford(int cost)
    {
        return (potential >= cost);  
    }

    public void SpendPotential(int cost)
    {
        if(potential >= cost)
        {
            potential -= cost;
            float ratio = ((float)maxPotential - (float)potential) / (float)maxPotential;
            bar.AdjustHeight(ratio);
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = -1f;
            GameObject feedback = Instantiate(feedbackTemplate, pos, transform.rotation);
            feedback.GetComponent<ExchangeFeedback>().SetPrice(cost * -1);
        }
        else
        {
            Debug.LogError("Not enough potential!");
        }
    }

    public void GainPotential(int cost)
    {
        potential += cost;
        float ratio = ((float)maxPotential - (float)potential) / (float)maxPotential;
        bar.AdjustHeight(ratio);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = -1f;
        GameObject feedback = Instantiate(feedbackTemplate, pos, transform.rotation);
        feedback.GetComponent<ExchangeFeedback>().SetPrice(cost);
    }

    private void SetStartingStats()
    {
        StickmanProfile profileTemp = new StickmanProfile();
        int cost = profileTemp.startingStat * 5;
        potential -= cost;
        float ratio = ((float)maxPotential - (float)potential) / (float)maxPotential;
        bar.AdjustHeight(ratio);
    }
}
