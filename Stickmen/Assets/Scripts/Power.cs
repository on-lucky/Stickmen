using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new power", menuName = "power")]
public class Power: ScriptableObject{

    public new string name;
    public GameObject icon;
    public int cost;

    public List<Move> moveList;
}
