using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomSpawner : MonoBehaviour {

    public float time_between_spawns = 2f;

    private IEnumerator coroutine;

    public void StartSpawning()
    {
        Debug.Log("startSpawn");
        coroutine = WaitAndSpawn(time_between_spawns);
        StartCoroutine(coroutine);
    }

    public void StopSpawning()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator WaitAndSpawn(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            GameManager.instance.local_stickman.SpawnPhantom(GameManager.instance.target);
        }
    }
}
