﻿using System.Collections;
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
    public bool hasColor = true;

	// Use this for initialization
	void Awake () {
        _collider = GetComponent<BoxCollider>();
        _material = GetComponent<MeshRenderer>().material;
        _textMesh = GetComponentInChildren<TextMeshPro>();
        _lightswitch = GetComponentInChildren<LightSwitch>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        if (hasColor)
            _material.color = baseColor;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        if(_textMesh != null)
            _textMesh.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, 1f);
        if (_lightswitch != null)
            _lightswitch.LightUP();
    }

    private void OnMouseExit()
    {
        if (_textMesh != null)
            _textMesh.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, 0f);
        if (_lightswitch != null)
            _lightswitch.LightDOWN();
    }

    virtual protected void OnMouseDown()
    {
        if (hasColor)
        {
            _material.color = clicColor;
        }
        if (_particleSystem != null)
            _particleSystem.Play();
        if (_lightswitch != null)
            _lightswitch.Flash();
    }

    private void OnMouseUp()
    {
        if (hasColor)
            _material.color = baseColor;
    }
}
