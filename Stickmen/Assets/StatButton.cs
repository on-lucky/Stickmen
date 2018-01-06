using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatButton : ButtonSelector{

    public StatBar targetStatBar;
    public bool isPlus = true;
    public string affectedStat;
    public StatDiagram[] diagrams;
    public TextMeshPro text;

    private StickmanProfile profile;

    private void Start()
    {
        StickmanProfile profileTemp = new StickmanProfile();
        targetStatBar.SetBar(profileTemp.GetStat(affectedStat));
        text.text = affectedStat[0].ToString().ToUpper() + affectedStat.Substring(1) + ":\t" + profileTemp.GetStat(affectedStat);
    }

    protected override void OnMouseDown()
    {
        profile = ScreenOrganiser.instance.players[0].GetComponent<NetworkPlayer>()._profile;
        base.OnMouseDown();
        ChangeStat();
    }

    private void ChangeStat()
    {
        bool hasChanged = false;
        bool canAfford = true;
        if (isPlus)
        {
            canAfford = PotentialManager.instance.CanAfford(1);
            if (canAfford)
            {
                hasChanged = profile.IncrementStat(affectedStat);
                if(hasChanged)
                    PotentialManager.instance.SpendPotential(1);
            }
        }
        else{
            hasChanged = profile.DecrementStat(affectedStat);
            if (hasChanged)
                PotentialManager.instance.GainPotential(1);
        }

        if (hasChanged && canAfford)
        {
            targetStatBar.SetBar(profile.GetStat(affectedStat));
            UpdateDiagrams();
            UpdateText();
        }
    }

    private void UpdateDiagrams()
    {
        foreach(StatDiagram diagram in diagrams)
        {
            diagram.UpdateToProfile(profile);
        }
    }

    private void UpdateText()
    {
        text.text = affectedStat[0].ToString().ToUpper() + affectedStat.Substring(1) + ":\t" + profile.GetStat(affectedStat);
    }
}
