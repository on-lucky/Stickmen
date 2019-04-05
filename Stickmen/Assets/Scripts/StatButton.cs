using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatButton : ButtonSelector{

    public StatBar targetStatBar;
    public bool isPlus = true;
    public Stats affectedStat;
    public StatDiagram[] diagrams;
    public TextMeshPro text;

    private StickmanProfile profile;

    private void Start()
    {
        StickmanProfile profileTemp = new StickmanProfile();
        targetStatBar.SetBar(profileTemp.GetStat(affectedStat));
        text.text = GetStatName((int)affectedStat) + ":\t" + profileTemp.GetStat(affectedStat);
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
        text.text = GetStatName((int)affectedStat) + ":\t" + profile.GetStat(affectedStat);
    }

    private string GetStatName(int index)
    {
        switch (index)
        {
            case 0:
                return "Strength";
            case 1:
                return "Dexterity";
            case 2:
                return "Resilience";
            case 3:
                return "Expertise";
            case 4:
                return "Endurance";
            default:
                return "Unknown";
        }
    }
}
