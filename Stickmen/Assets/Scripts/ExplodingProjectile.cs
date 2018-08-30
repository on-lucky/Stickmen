using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingProjectile : Projectile {

    private Collider _collider; // projectile colider
    private Renderer _Renderer; // projectile renderer
    private LightSwitch _light; // The projectile/explosion light
    private bool _destroying = false; // if the projectile should be destroyed

    public ParticleSystem[] _baseParticles; // particles that play at start
    public ParticleSystem[] _explodingParticles; // particles that play when exploding 

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<Collider>();
        _Renderer = GetComponentInChildren<Renderer>();
        _light = GetComponentInChildren<LightSwitch>();
    }

    protected override void Start()
    {
        base.Start();
        _light.LightUP();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_destroying)
        {
            Explode();
        }
    }

    private void Update()
    {
        if (_destroying)
        {
            if (!VerifyParticles(_explodingParticles))
            {
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// Manages the particles, then destroy the object
    /// </summary>
    private void Explode()
    {
        StopBasicParticles();
        StartExplosionParticles();

        base._rigidbody.isKinematic = true;

        if(_light != null)
        {
            _light.Flash();
        }

        StartDestruction();
    }

    /// <summary>
    /// Stops the usual particles
    /// </summary>
    private void StopBasicParticles()
    {
        foreach (ParticleSystem part in _baseParticles)
        {
            part.Stop();
        }
    }

    /// <summary>
    /// Starts the explosion particles
    /// </summary>
    private void StartExplosionParticles()
    {
        foreach (ParticleSystem part in _explodingParticles)
        {
            part.Play();
        }
    }

    /// <summary>
    /// Initiate the projectile destruction
    /// </summary>
    private void StartDestruction()
    {
        if(_Renderer != null)
        {
            _Renderer.enabled = false;
        }
        _destroying = true;
    }

    /// <summary>
    /// Verify if all the particleSystems in a group have stopped their animation
    /// </summary>
    /// <param name="particles">The group of particleSystem to verify</param>
    /// <returns></returns>
    private bool VerifyParticles(ParticleSystem[] particles)
    {
        bool isPlaying = false;
        foreach (ParticleSystem part in particles)
        {
            if (part.IsAlive())
            {
                isPlaying = true;
            }
        }
        return isPlaying;
    }
}
