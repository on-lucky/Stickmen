using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuElement : MonoBehaviour {

    private MeshRenderer[] myRenderers;
    
    private bool isScaling = false;
    private float scale;
    private float timeElapsed = 0;
    private float a;

    public bool getChildRenderers = false;
    public float startingScale = 0.1f;
    public float timeToScale;
    public float maxScale = 1.3f;
    public LineRenderer lineRenderer;
    public StatDiagram statDiagram;

    private void Awake()
    {
        if (getChildRenderers)
        {
            myRenderers = this.GetComponentsInChildren<MeshRenderer>();
        }
        else
        {
            myRenderers = new MeshRenderer[1];
            myRenderers[0] = this.GetComponentInChildren<MeshRenderer>();
        }
        
    }

    private void Start()
    {
        EnableRenderers(false);
        if (lineRenderer != null)
            lineRenderer.enabled = false;
        transform.localScale = new Vector3(startingScale, startingScale, startingScale);
        scale = startingScale;
    }

    // Update is called once per frame
    void Update () {
        if (isScaling)
        {
            timeElapsed += Time.deltaTime;
            ScaleUp(timeElapsed);
        }
	}

    public void Appear()
    {
        SetA();
        EnableRenderers(true);
        if (lineRenderer != null)
            lineRenderer.enabled = true;
        isScaling = true;
    }

    private void SetA()
    {
        a = (-4 * (maxScale - startingScale)) / (Mathf.Pow(timeToScale, 2));
    }

    private void ScaleUp(float time)
    {
        scale = a * Mathf.Pow((time - (timeToScale / 2f)), 2) + maxScale;
        if(scale <= 1 && time > timeToScale / 2)
        {
            scale = 1;
            isScaling = false;
            if (statDiagram != null)
            {
                statDiagram.Init();
            }
        }
        transform.localScale = new Vector3(scale, scale, scale);
    }

    private void EnableRenderers(bool enabled)
    {
        foreach(MeshRenderer renderer in myRenderers)
        {
            renderer.enabled = enabled;
        }
    }

}
