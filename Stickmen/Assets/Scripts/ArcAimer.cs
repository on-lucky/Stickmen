using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcAimer : MonoBehaviour {

    public int numberOfPoints = 20;
    public GameObject objToFollow;

    private LineRenderer lineRenderer;

    public float maxHeight = 4;
    public float minHeight = 1;
    public float currentHeight = 0;

    public float maxLenght = 4;
    private float currentLenght = 4;

    public float paramA = 0;

    private Vector3 goalPos;

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();

        if (objToFollow == null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            mouseWorldPos.z = 0;
            goalPos = mouseWorldPos;
        }
        else
        {
            goalPos = objToFollow.transform.position;
        }

        currentHeight = transform.position.y + maxHeight;

        UpdateA();

        lineRenderer.positionCount = numberOfPoints + 1;
        CameraController.instance.EnableControler(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (GameManager.instance.game_state != GameManager.GameState.Aiming)
        {
            Destroy(this.gameObject);
        }

        if (objToFollow == null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            mouseWorldPos.z = 0;
            goalPos = mouseWorldPos;
        }
        else
        {
            goalPos = objToFollow.transform.position;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            currentHeight = Mathf.Clamp(currentHeight + Input.GetAxis("Mouse ScrollWheel"), Mathf.Clamp(goalPos.y + 0.0001f, transform.position.y + minHeight, transform.position.y + maxHeight), transform.position.y + maxHeight);
        }

        UpdateLenght();
        UpdateA();

        UpdatePoints();
    }

    private void UpdateA()
    {
        float denominator = Mathf.Pow((transform.position.x - currentLenght), 2);
        if(denominator == 0)
        {
            denominator = 0.001f;
        }

        paramA = (transform.position.y - currentHeight) / denominator;
    }

    private void UpdateLenght()
    {
        if (goalPos.x == 0)
        {
            currentLenght = 0;

            if (float.IsNaN(currentLenght) || float.IsInfinity(currentLenght))
            {
                Debug.Log("oops");
            }
        }
        else
        {

            float x = transform.position.x;
            float y = transform.position.y;

            float i = goalPos.x;
            float j = Mathf.Min(goalPos.y, currentHeight);

            float a = y - j;
            float b = 2 * (i * (currentHeight - y) + x * (j - currentHeight));
            float c = (Mathf.Pow(i, 2) * (y - currentHeight)) + (Mathf.Pow(x, 2) * (currentHeight - j));

            if ((Mathf.Pow(b, 2) - (4 * a * c)) >= 0)
            {
                float h = (-b + Mathf.Sqrt(Mathf.Pow(b, 2) - (4 * a * c))) / (2 * a);
                if (!VerifyH(h))
                {
                    h = (-b - Mathf.Sqrt(Mathf.Pow(b, 2) - (4 * a * c))) / (2 * a);
                }

                currentLenght = Mathf.Clamp(h, x - maxLenght, x + maxLenght);
            }

            if (float.IsNaN(currentLenght) || float.IsInfinity(currentLenght))
            {
                Debug.Log("j: " + j);
                Debug.Log("a: " + a);
                Debug.Log("b: " + b);
                Debug.Log("c: " + c);
                Debug.Log("paramA: " + paramA);
                Debug.Log("currentHeight: " + currentHeight);
                Debug.Log("currentLenght: " + currentLenght);
            }
        }
    }

    private void UpdatePoints()
    {
        Vector3[] points = new Vector3[numberOfPoints + 1];
        for (int i = 0; i <= numberOfPoints; i++)
        {
            float x = transform.position.x + (i * (goalPos.x - transform.position.x) / numberOfPoints);
            points[i] = CalculatePoint(x);
        }

        lineRenderer.SetPositions(points);
    }

    private Vector3 CalculatePoint(float x)
    {
        float y = paramA * Mathf.Pow((x - currentLenght), 2) + currentHeight;
        return new Vector3(x, y, 0);
    }

    private bool VerifyH(float x)
    {
        if(goalPos.x >= transform.position.x)
        {
            if(x >= transform.position.x && x <= goalPos.x)
            {
                return true;
            }
        }
        else
        {
            if (x <= transform.position.x && x >= goalPos.x)
            {
                return true;
            }
        }
        return false;
    }
}
