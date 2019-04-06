using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AgingMove
{
    public Move move;
    public float age;
}

public class ShadeMoveManager : MonoBehaviour {

    public static ShadeMoveManager instance;
    public float phantomLifeSpan = 1f;

    private List<AgingMove> currentMoves;

    

    private void Awake()
    {
        if (ShadeMoveManager.instance != null)
        {
            Debug.LogError("More than one ShadeMoveManager in the scene!");
        }
        ShadeMoveManager.instance = this;
        currentMoves = new List<AgingMove>();
    }

    // Use this for initialization
    void Start () {
        //isUpdating = false;
	}
	
	// Update is called once per frame
	void Update () {
        //if (isUpdating)
        //{
            for(int i = 0; i < currentMoves.Count; i++)
            {
                if (!currentMoves[i].move.UpdateMethod(Time.deltaTime))
                {
                    if (currentMoves[i].move.CheckPhantom())
                    {
                        currentMoves[i].age += Time.deltaTime;
                        if (UpdateAge(currentMoves[i]))
                        {
                            i--;
                        }
                    }
                }
            }
        //}
	} 

    private bool UpdateAge(AgingMove aMove)
    {
        bool destroyed = false;
        if (aMove.move.CheckPhantom())
        {
            aMove.age += Time.deltaTime;
            if(aMove.age >= phantomLifeSpan)
            {
                currentMoves.Remove(aMove);
                aMove.move.DestroyStickman();
                destroyed = true;
            }
        }
        return destroyed;
    }

    public void AddMove(Move _move)
    {
        AgingMove aMove = new AgingMove();
        aMove.move = _move;
        aMove.age = 0;
        currentMoves.Add(aMove);
    }

    public void RemoveMove(Move _move)
    {
        AgingMove chosen_aMove = new AgingMove();
        foreach(AgingMove aMove in currentMoves)
        {
            if(aMove.move == _move)
            {
                chosen_aMove = aMove;
            }
        }
        currentMoves.Remove(chosen_aMove);
    }

    public void SetUpdating(bool shouldUpdate)
    {
        //isUpdating = shouldUpdate;
    }
}
