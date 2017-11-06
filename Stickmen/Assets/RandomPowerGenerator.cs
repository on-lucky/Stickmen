using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPowerGenerator : MonoBehaviour {

    public List<Power> Player1Powers;
    public List<Power> Player2Powers;

    public List<Power>[] testTable = new List<Power>[10000];

    public int maxPowerNumber = 4;
    public int totalPoints = 6;

    // Use this for initialization
    void Start () {

        for(int i = 0; i < 10000; i++)
        {
            testTable[i] = new List<Power>();
            GeneratePowers(totalPoints, testTable[i]);
        }
        CountPowers();

        GeneratePowers(totalPoints, Player1Powers);
        GeneratePowers(totalPoints, Player2Powers);
        RandomIconsManager.instance.SpawnRandomIcons(Player1Powers, Player2Powers);
    }

    private void GeneratePowers(int totalPoints, List<Power> powerList)
    {
        int remainingPoints = totalPoints;

        while(remainingPoints > 0)
        {
            Power chosenPower = GeneratePower(remainingPoints, powerList);
            powerList.Add(chosenPower);
            remainingPoints -= chosenPower.cost;
        }
    }

    private Power GeneratePower(int maxCost, List<Power> powerList)
    {
        bool isChoosable = false;
        int index = 0;

        while (!isChoosable) {
            index = Random.Range(0, PowerManager.instance.allPowers.Length);

            if (powerList.Count < maxPowerNumber - 1)
            {
                if (PowerManager.instance.allPowers[index].cost <= maxCost)
                {
                    if (!powerList.Contains(PowerManager.instance.allPowers[index]))
                    {
                        isChoosable = true;
                    }
                }
            }
            else
            {
                if (PowerManager.instance.allPowers[index].cost == maxCost)
                {
                    if (!powerList.Contains(PowerManager.instance.allPowers[index]))
                    {
                        isChoosable = true;
                    }
                }
            }
        }
       
        return PowerManager.instance.allPowers[index];
    }

    private void CountPowers()
    {
        int totalCounter = 0;
        foreach (List<Power> powerList in testTable)
        {
            totalCounter += powerList.Count;
        }


        foreach (Power power in PowerManager.instance.allPowers)
        {
            int counter = 0;
            foreach(List<Power> powerList in testTable)
            {
                foreach (Power powerFound in powerList)
                {
                    if(powerFound.name == power.name)
                    {
                        counter++;
                    }
                }
            }
            float probability = ((float)counter / (float)totalCounter) * 100f;
            Debug.Log("The power " + power.name + " appears " + counter + " times. Probability = " + probability + "%");
        }
    }
}
