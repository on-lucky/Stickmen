using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExchangeFeedback : MonoBehaviour {

    public TextMeshPro _text;
    public float _timeToLive = 1f;
    public float _floatSpeed = 0.1f;
    public float _shrinkSpeed = 2f;

    private bool _dying;
    private float _scale;
    private float _lifeSpam;

    public void SetPrice(int cost)
    {
        if(cost < 0)
        {
            _text.text = cost.ToString();
        }
        else
        {
            _text.text = "+" + cost;
        }
    }

	// Use this for initialization
	void Start () {
        _text = GetComponentInChildren<TextMeshPro>();
        _scale = transform.localScale.x;
        _lifeSpam = _timeToLive;
	}
	
	// Update is called once per frame
	void Update () {
        FloatUp();
        if (_dying)
        {
            Shrink();
        }
        else
        {
            GetOlder();
        }
	}

    private void GetOlder()
    {
        _timeToLive -= Time.deltaTime;

        if(_timeToLive < 0)
        {
            _timeToLive = 0;
            //Die();
            Destroy(this.gameObject);
        }
    }

    private void Die()
    {
        _dying = true;
    }

    private void Shrink()
    {
        _scale -= (Time.deltaTime * _shrinkSpeed);
        if (_scale < 0)
        {
            Destroy(this);
        }
        transform.localScale = new Vector3(_scale, _scale, _scale);
        
    }

    private void FloatUp()
    {
        float speed = (_timeToLive / _lifeSpam) * _floatSpeed; 
        transform.Translate(new Vector3(0, speed, 0));
    }
}
