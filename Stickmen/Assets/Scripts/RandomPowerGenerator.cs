using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RandomPowerGenerator : NetworkBehaviour {

    public static RandomPowerGenerator instance;

    public List<Power> Player1Powers;
    public List<Power> Player2Powers;

    public List<Power>[] testTable = new List<Power>[10000];

    public int maxPowerNumber = 4;
    public int totalPoints = 6;

    private void Awake()
    {
        if (RandomPowerGenerator.instance != null)
        {
            Debug.LogError("More than one RandomPowerGenerator in the scene!");
        }
        RandomPowerGenerator.instance = this;
    }

    public void StartGeneration()
    {
        Debug.Log("starting generation");
        if (isServer)
        {
            Debug.Log("i am the server");
            int[] indexes1 = GeneratePowers(totalPoints, Player1Powers);
            int[] indexes2 = GeneratePowers(totalPoints, Player2Powers);
            NetworkMessenger.instance.SendPowerListMessage(indexes1, indexes2);
        }
    }

    private int[] GeneratePowers(int totalPoints, List<Power> powerList)
    {
        int remainingPoints = totalPoints;

        while (remainingPoints > 0)
        {
            Power chosenPower = GeneratePower(remainingPoints, powerList);
            powerList.Add(chosenPower);
            remainingPoints -= chosenPower.cost;
        }
        return GenerateIndexList(powerList);
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

    private int[] GenerateIndexList(List<Power> powerList)
    {
        int[] indexes = new int[powerList.Count];
        int i = 0;
        foreach(Power power in powerList)
        {
            indexes[i++] = PowerManager.instance.FindIndex(power.name);
        }
        return indexes;
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
