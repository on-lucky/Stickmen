using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour {

    public static PowerManager instance;

    public Power[] allPowers;

    private void Awake()
    {
        if (PowerManager.instance != null)
        {
            Debug.LogError("More than one PowerManager in the scene!");
        }
        PowerManager.instance = this;
    }

    public int FindIndex(string powerName)
    {
        int foundIndex = 0;
        for(int i = 0; i < allPowers.Length; i++)
        {
            if (allPowers[i].name == powerName)
            {
                foundIndex = i;
                break;
            }
        }
        return foundIndex;
    }
    
    public List<Power> getPowerList(int[] indexes)
    {
        List<Power> powerList = new List<Power>();
        foreach(int index in indexes)
        {
            powerList.Add(allPowers[index]);
        }
        return powerList;
    }
}
