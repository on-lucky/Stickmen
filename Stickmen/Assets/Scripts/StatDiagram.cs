using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatDiagram : MonoBehaviour {

    public StickmanProfile profile;

    public Material backMat;
    public Material frontMat;
    public float updateSpeed = 2f;

    private bool isChanging = false;
    private float[] currentStats;
    private float[] goalStats;
    private MeshFilter filter;


    public void Init () {
        profile = new StickmanProfile();

        currentStats = new float[]{
            profile.strength,
            profile.dexterity,
            profile.resilience,
            profile.endurance,
            profile.expertise
        };

        goalStats = new float[]{
            profile.strength,
            profile.dexterity,
            profile.resilience,
            profile.endurance,
            profile.expertise
        };

        Vector2 position = new Vector2(transform.position.x, transform.position.y);

        // Create Vector2 vertices
        Vector2[] vertices2D = new Vector2[] {
            new Vector2(0, profile.strength + 1),
            new Vector2((profile.dexterity + 1) * Mathf.Sin(2 * Mathf.PI / 5f), (profile.dexterity + 1) * Mathf.Cos( 2 * Mathf.PI / 5f)),
            new Vector2((profile.resilience + 1) * Mathf.Sin(4 * Mathf.PI / 5f), (profile.resilience + 1) * Mathf.Cos(4 * Mathf.PI / 5f)),
            new Vector2((profile.endurance + 1) * Mathf.Sin(6 * Mathf.PI / 5f), (profile.endurance + 1) * Mathf.Cos(6 * Mathf.PI / 5f)),
            new Vector2((profile.expertise + 1) * Mathf.Sin(8 * Mathf.PI / 5f), (profile.expertise + 1) * Mathf.Cos(8 * Mathf.PI / 5f))
        };

        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        // Set up game object with mesh;
        gameObject.AddComponent(typeof(MeshRenderer));
        filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        filter.mesh = msh;

        GetComponent<MeshRenderer>().material = frontMat;

        TextMeshPro[] texts = GetComponentsInChildren<TextMeshPro>();
        foreach(TextMeshPro text in texts)
        {
            text.GetComponent<MeshRenderer>().enabled = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isChanging)
        {
            UpdateStats();
        }
	}

    private void UpdateStats()
    {
        bool shouldChange = false;
        for(int i = 0; i < currentStats.Length; i++)
        {
            if(currentStats[i] != goalStats[i])
            {
                UpdateSingleStat(i);
                shouldChange = true;
            }
        }
        if (shouldChange)
        {
            UpdateDiagram();
        }
        else
        {
            isChanging = false;
        }
        
    }

    private void UpdateSingleStat( int index)
    {
        if (currentStats[index] > goalStats[index])
        {
            currentStats[index] -= Time.deltaTime * updateSpeed;
            if (currentStats[index] < goalStats[index])
                currentStats[index] = goalStats[index];
        }
        else if (currentStats[index] < goalStats[index])
        {
            currentStats[index] += Time.deltaTime * updateSpeed;
            if (currentStats[index] > goalStats[index])
                currentStats[index] = goalStats[index];
        }
    }

    private void UpdateDiagram()
    {
        // Create Vector2 vertices
        Vector2[] vertices2D = new Vector2[] {
            new Vector2(0, (currentStats[0] + 1)),
            new Vector2((currentStats[1] + 1) * Mathf.Sin(2 * Mathf.PI / 5f), (currentStats[1] + 1) * Mathf.Cos( 2 * Mathf.PI / 5f)),
            new Vector2((currentStats[2] + 1) * Mathf.Sin(4 * Mathf.PI / 5f), (currentStats[2] + 1) * Mathf.Cos(4 * Mathf.PI / 5f)),
            new Vector2((currentStats[3] + 1) * Mathf.Sin(6 * Mathf.PI / 5f), (currentStats[3] + 1) * Mathf.Cos(6 * Mathf.PI / 5f)),
            new Vector2((currentStats[4] + 1) * Mathf.Sin(8 * Mathf.PI / 5f), (currentStats[4] + 1) * Mathf.Cos(8 * Mathf.PI / 5f))
        };

        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        filter.mesh = msh;
    }

    public void ChangeStat(int index, int value)
    {
        isChanging = true;
        goalStats[index] = value;
    }

    public void UpdateToProfile(StickmanProfile newProfile)
    {
        profile = newProfile;
        goalStats[0] = profile.strength;
        goalStats[1] = profile.dexterity;
        goalStats[2] = profile.resilience;
        goalStats[3] = profile.expertise;
        goalStats[4] = profile.endurance;
        isChanging = true;
    }
}
