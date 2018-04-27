using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Icon : MonoBehaviour{

    public string _iconName;
    public Icon _parent;
    public TextMeshPro _text;
    public List<Icon> _childIcons;
    public int _index = 1;
    public MeshRenderer _shade;

    private Slider _slider;
    private float _slideDistance;
    private Collider2D _boxCollider;
    private bool _pressed = false;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider = GetComponent<Slider>();
        _text.text = _iconName;
        _slideDistance = transform.localScale.x * 1.1f;
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.enabled = false;
        _childIcons = new List<Icon>();
    }

    private void Start()
    {
        RemoveText();       
    }

    private void Update()
    {
        _text.fontSize = Mathf.Abs(Camera.main.transform.position.z) * 4;
    }

    protected virtual void OnMouseUp()
    {
        if(_childIcons.Count > 0)
        {
            if (!_pressed)
            {
                HideNephews();
                Shade(false);
                ShadeBrothers(true);
                ShowChildren();
                _pressed = true;
            }
            else{
                HideChildren();
                ShadeBrothers(false);
                _pressed = false;
            }
        }
        else
        {
            //execute the chosen move
            GameManager.instance.local_stickman.SetUpMove(_iconName);
        }
    }

    public void Appear()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        EnableCollider(true);
        _slider.SetGoalPos(_slideDistance, 'x');
        StartCoroutine(WaitAndSlide());
    }

    private IEnumerator WaitAndSlide()
    {
        yield return StartCoroutine(WaitForSlider());
        yield return new WaitForSeconds(0.2f);
        _slider.SetGoalPos((-_slideDistance * _index), 'y');
        StartCoroutine(WaitAndAddText());
    }

    private IEnumerator WaitAndAddText()
    {
        yield return StartCoroutine(WaitForSlider());
        if (GetComponent<SpriteRenderer>().enabled)
        {
            AddText();
        }
    }

    private IEnumerator WaitForSlider()
    {
        while (_slider.VerifyMovement())
        {
            yield return null;
        }
        yield return 0;
    }

    private void AddText()
    {
        _text.enabled = true;
    }

    public void RemoveText()
    {
        _text.enabled = false;
    }
    
    public void RemoveTexts()
    {
        if (_parent != null)
        {
            foreach (Icon icon in _parent._childIcons)
            {
                icon.RemoveText();
            }
        }
    }

    public void RestoreTexts()
    {
        if (_parent != null)
        {
            foreach (Icon icon in _parent._childIcons)
            {
                icon.AddText();
            }
        }
    }

    public virtual void TakeChildrenText()
    {
        Debug.LogWarning("Trying to access the children of a non-catergory Icon!");
    }

    public virtual void Addchild(Icon icon)
    {
        Debug.LogWarning("Trying to add children to a non-catergory Icon!");
    }

    public void EnableCollider(bool shoulEnable)
    {
        _boxCollider.enabled = shoulEnable;
    }

    public void SetName(string name)
    {
        _iconName = name;
        _text.text = name;
        this.name = name;
    }

    private void ShowChildren()
    {
        RemoveTexts();
        foreach (Icon icon in _childIcons)
        {
            icon.Appear();
        }
    }

    private void HideChildren()
    {
        RestoreTexts();
        foreach (Icon icon in _childIcons)
        {
            icon.Disapear();
        }
    }

    public void Shade(bool shouldEnable)
    {
        _shade.enabled = shouldEnable;
    }

    private void ShadeBrothers(bool shouldShade){
        if (_parent != null)
        {
            foreach (Icon icon in _parent._childIcons)
            {
                if (icon != this)
                {
                    icon.Shade(shouldShade);
                }
            }
        }
    }

    private void HideNephews()
    {
        if (_parent != null)
        {
            foreach (Icon icon in _parent._childIcons)
            {
                if(icon != this)
                {
                    icon.HideChildren();
                    icon._pressed = false;
                }
            }
        }
    }

    public void Disapear()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        _boxCollider.enabled = false;
        _slider.ResetSlider();
        transform.localPosition = new Vector3(0, 0, 0);
        RemoveText();
        _pressed = false;
        Shade(false);
        
        if(_childIcons.Count > 0)
        {
            foreach(Icon child in _childIcons)
            {
                child.Disapear();
            }
        }
    }
}
