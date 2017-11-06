using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StickmanProfile {

    private Power[] pvowers;

    // stats

    public int strength = 0; // Physical dmg and range
    public int dexterity = 0; // Movement speed chance to win initiative duels
    public int resilience = 0; // Health points and resistance to flinching
    public int expertise = 0; // Mana regen and total mana 
    public int endurance = 0; // Stamina regen and total stamina
}
