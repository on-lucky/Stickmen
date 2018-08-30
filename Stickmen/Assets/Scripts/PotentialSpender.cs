using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PotentialSpender : MonoBehaviour {

    public int potentialAmount = 1;

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public GameObject priceTagTemplate;

    public Color negativeColor;
    public Color positiveColor;

    private GameObject priceTag;

    private const float PRICEXOFFSET = 0.55f;
    private const float PRICEYOFFSET = -0.25f;

    private void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.x += PRICEXOFFSET;
        pos.y += PRICEYOFFSET;
        pos.z = -1f;
        priceTag = Instantiate(priceTagTemplate, pos, this.transform.rotation);
        SetPrice();
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
        Destroy(priceTag);
    }

    private void Update()
    {
        if(priceTag != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.x += PRICEXOFFSET;
            pos.y += PRICEYOFFSET;
            pos.z = -1f;
            priceTag.transform.position = pos;
        }
    }

    private void SetPrice()
    {
        if (potentialAmount < 0) {
            priceTag.GetComponent<TextMeshPro>().text = potentialAmount.ToString();
            priceTag.GetComponent<TextMeshPro>().faceColor = negativeColor;
        }
        else{
            priceTag.GetComponent<TextMeshPro>().text = "+" + potentialAmount;
            priceTag.GetComponent<TextMeshPro>().faceColor = positiveColor;
        }
    }
}
