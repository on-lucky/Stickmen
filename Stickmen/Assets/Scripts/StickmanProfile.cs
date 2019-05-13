using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stats
{
    strength,
    dexterity,
    resilience,
    expertise,
    endurance
}

[System.Serializable]
public class StickmanProfile {

    public List<Power> powers;

    // stats

    private int MAXSTAT = 12;
    public int startingStat = 4;

    public int strength;                // Physical dmg and range
    public int dexterity;               // Movement speed  and chance to win initiative duels
    public int resilience;              // Health points and resistance to flinching
    public int expertise;               // Mana regen and total mana 
    public int endurance;               // Stamina regen and total stamina

    private int base_strength = 3;      // base value for the strength stat
    private int base_dexterity = 3;     // base value for the dexterity stat
    private int base_resilience = 3;    // base value for the resilience stat
    private int base_expertise = 3;     // base value for the expertise stat
    private int base_endurance = 3;     // base value for the endurance stat

    private List<Move> _moveList;

    public StickmanProfile()
    {
        strength = startingStat;
        dexterity = startingStat;
        resilience = startingStat;
        expertise = startingStat;
        endurance = startingStat;

        _moveList = new List<Move>();
    }

    public void SetMoveList(List<Move> moves)
    {
        _moveList = moves;
    }

    public Move FindMove(string move_name)
    {
        foreach(Move move in _moveList)
        {
            Debug.Log(move._moveName);
            if(move._moveName == move_name)
            {
                Debug.Log("FOUND IT");
                return move;
            }
        }
        return null;
    }

    public bool IncrementStat(Stats affectedStat)
    {
        switch (affectedStat)
        {
            case Stats.strength:
                if(strength < MAXSTAT)
                {
                    strength++;
                    return true;
                }
                break;
            case Stats.dexterity:
                if (dexterity < MAXSTAT)
                {
                    dexterity++;
                    return true;
                }
                break;
            case Stats.resilience:
                if (resilience < MAXSTAT)
                {
                    resilience++;
                    return true;
                }
                break;
            case Stats.expertise:
                if (expertise < MAXSTAT)
                {
                    expertise++;
                    return true;
                }
                break;
            case Stats.endurance:
                if (endurance < MAXSTAT)
                {
                    endurance++;
                    return true;
                }
                break;
            default:
                return false;
        }
        return false;
    }

    public bool DecrementStat(Stats affectedStat)
    {
        switch (affectedStat)
        {
            case Stats.strength:
                if (strength > 0)
                {
                    strength--;
                    return true;
                }
                break;
            case Stats.dexterity:
                if (dexterity > 0)
                {
                    dexterity--;
                    return true;
                }
                break;
            case Stats.resilience:
                if (resilience > 0)
                {
                    resilience--;
                    return true;
                }
                break;
            case Stats.expertise:
                if (expertise > 0)
                {
                    expertise--;
                    return true;
                }
                break;
            case Stats.endurance:
                if (endurance > 0)
                {
                    endurance--;
                    return true;
                }
                break;
            default:
                return false;
        }
        return false;
    }

    public int GetStat(Stats stat)
    {
        switch (stat)
        {
            case Stats.strength:
                return strength;
            case Stats.dexterity:
                return dexterity;
            case Stats.resilience:
                return resilience;
            case Stats.expertise:
                return expertise;
            case Stats.endurance:
                return endurance;
            default:
                return -1;
        }
    }

    public int GetStatValue(Stats stat)
    {
        switch (stat)
        {
            case Stats.strength:
                return strength + base_strength;
            case Stats.dexterity:
                return dexterity + base_dexterity;
            case Stats.resilience:
                return resilience + base_resilience;
            case Stats.expertise:
                return expertise + base_expertise;
            case Stats.endurance:
                return endurance + base_endurance;
            default:
                return -1;
        }
    }
}
