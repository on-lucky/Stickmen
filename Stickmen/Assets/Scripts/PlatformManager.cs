using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    public static PlatformManager instance;

    public List<Platform> platforms;
    public GameObject collider_template;

    private void Awake()
    {
        if (PlatformManager.instance != null)
        {
            Debug.LogError("More than one PlatformManager in the scene!");
        }
        PlatformManager.instance = this;
    }

    // Use this for initialization
    void Start () {
        //InitPlatforms();
    }
	
    private void InitPlatforms()
    {
        foreach(Platform plat in platforms)
        {
            GameObject platform = Instantiate(collider_template, transform);
            BoxCollider collider = platform.AddComponent<BoxCollider>();

            collider.size = new Vector3(plat.x_max - plat.x_min, 0.5f, 5);
            collider.center = new Vector3((plat.x_max + plat.x_min) / 2, plat.y_top - 0.5f, 0);
        }
    }

    public Platform GetCurrentPlatform()
    {
        Vector3 player_pos = GameManager.instance.local_stickman.transform.position;

        float y_distance = 100;
        Platform chosen_platform = new Platform();

        foreach (Platform plat in platforms)
        {
            if(player_pos.x < plat.x_max && player_pos.x > plat.x_min)
            {
                if(Mathf.Abs(player_pos.y - plat.y_top) < y_distance)
                {
                    y_distance = Mathf.Abs(player_pos.y - plat.y_top);
                    chosen_platform = plat;
                }
            }
        }
        return chosen_platform;
    }
}
