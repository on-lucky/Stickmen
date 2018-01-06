using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StickmanProfile {

    private Power[] pvowers;

    // stats

    private int MAXSTAT = 12;
    public int startingStat = 4;

    public int strength; // Physical dmg and range
    public int dexterity; // Movement speed  and chance to win initiative duels
    public int resilience; // Health points and resistance to flinching
    public int expertise; // Mana regen and total mana 
    public int endurance; // Stamina regen and total stamina

    public StickmanProfile()
    {
        strength = startingStat;
        dexterity = startingStat;
        resilience = startingStat;
        expertise = startingStat;
        endurance = startingStat;
    }

    public bool IncrementStat(string affectedStat)
    {
        switch (affectedStat)
        {
            case "strength":
                if(strength < MAXSTAT)
                {
                    strength++;
                    return true;
                }
                break;
            case "dexterity":
                if (dexterity < MAXSTAT)
                {
                    dexterity++;
                    return true;
                }
                break;
            case "resilience":
                if (resilience < MAXSTAT)
                {
                    resilience++;
                    return true;
                }
                break;
            case "expertise":
                if (expertise < MAXSTAT)
                {
                    expertise++;
                    return true;
                }
                break;
            case "endurance":
                if (endurance < MAXSTAT)
                {
                    endurance++;
                    return true;
                }
                break;
            default:
                Debug.LogError("Stat name: (" + affectedStat + ") is invalid");
                return false;
        }
        return false;
    }

    public bool DecrementStat(string affectedStat)
    {
        switch (affectedStat)
        {
            case "strength":
                if (strength > 0)
                {
                    strength--;
                    return true;
                }
                break;
            case "dexterity":
                if (dexterity > 0)
                {
                    dexterity--;
                    return true;
                }
                break;
            case "resilience":
                if (resilience > 0)
                {
                    resilience--;
                    return true;
                }
                break;
            case "expertise":
                if (expertise > 0)
                {
                    expertise--;
                    return true;
                }
                break;
            case "endurance":
                if (endurance > 0)
                {
                    endurance--;
                    return true;
                }
                break;
            default:
                Debug.LogError("Stat name: (" + affectedStat + ") is invalid");
                return false;
        }
        return false;
    }

    public int GetStat(string stat)
    {
        switch (stat)
        {
            case "strength":
                return strength;
            case "dexterity":
                return dexterity;
            case "resilience":
                return resilience;
            case "expertise":
                return expertise;
            case "endurance":
                return endurance;
            default:
                Debug.LogError("Stat name: (" + stat + ") is invalid");
                return -1;
        }
    }
}
