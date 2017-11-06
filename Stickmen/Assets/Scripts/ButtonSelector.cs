using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonSelector : MonoBehaviour {

    private Collider _collider;
    private Material _material;
    private TextMeshPro _textMesh;
    private LightSwitch _lightswitch;
    private ParticleSystem _particleSystem;

    public Color baseColor;
    public Color clicColor;

	// Use this for initialization
	void Awake () {
        _collider = GetComponent<BoxCollider>();
        _material = GetComponent<MeshRenderer>().material;
        _textMesh = GetComponentInChildren<TextMeshPro>();
        _lightswitch = GetComponentInChildren<LightSwitch>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        _textMesh.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, 1f);
        _lightswitch.LightUP();
    }

    private void OnMouseExit()
    {
        _textMesh.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, 0f);
        _lightswitch.LightDOWN();
    }

    virtual protected void OnMouseDown()
    {
        _material.color = clicColor;
        _particleSystem.Play();
        _lightswitch.Flash();
    }

    private void OnMouseUp()
    {
        _material.color = baseColor;
    }
}
