using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonSelector : MonoBehaviour {

    private Collider _collider;
    private Material _material;
    private TextMeshPro _textMesh;

    public Color baseColor;
    public Color clicColor;

	// Use this for initialization
	void Awake () {
        _collider = GetComponent<BoxCollider>();
        _material = GetComponent<MeshRenderer>().material;
        _textMesh = GetComponentInChildren<TextMeshPro>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        _textMesh.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, 1f);
    }

    private void OnMouseExit()
    {
        _textMesh.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, 0f);
    }

    private void OnMouseDown()
    {
        _material.color = clicColor;
    }

    private void OnMouseUp()
    {
        _material.color = baseColor;
    }
}
