using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconOrganiser : ScriptableObject {

    /// <summary>
    /// Make a Move tree out of a Move List
    /// </summary>
    /// <param name="moveList"></param>
    static public void OrganiseIcons(List<Move> moveList)
    {
        ClearTree(moveList);

        foreach (Move move in moveList)
        {
            if (move.HasCategory())
            {
                SetParent(move, move._category, moveList);
            }
        }
    }

    /// <summary>
    /// Make a Move list out of a Power list
    /// </summary>
    /// <param name="powerList">the initial powerList to convert</param>
    /// <returns></returns>
    static public List<Move> MakeMoveList(List<Power> powerList)
    {
        List<Move> moveList = new List<Move>();
        foreach (Power pow in powerList)
        {
            foreach (Move mov in pow.moveList)
            {
                moveList.Add(mov);
            }
        }
        return moveList;
    }

    /// <summary>
    /// Search a Move by name in a Move List
    /// </summary>
    /// <param name="name"></param>
    /// <param name="moveList"></param>
    /// <returns></returns>
    static public Move FindMove(string name, List<Move> moveList)
    {
        Move foundMove = ScriptableObject.CreateInstance("Move") as Move;
        foreach(Move move in moveList)
        {
            if (move._moveName == name)
            {
                foundMove = move;
            }
        }
        return foundMove;
    }

    /// <summary>
    /// Set a new familly link between two Moves in the Move tree
    /// </summary>
    /// <param name="move"></param>
    /// <param name="parent"></param>
    /// <param name="moveList"></param>
    static private void SetParent(Move move, string parent, List<Move> moveList)
    {
        Move parentMove = FindMove(parent, moveList);

        move.SetParent(parentMove);
        parentMove.AddChild(move);
    }

    /// <summary>
    /// Clear all familly ties from the icon tree
    /// </summary>
    /// <param name="moveList">move list to clear</param>
    static void ClearTree(List<Move> moveList)
    {
        foreach(Move move in moveList)
        {
            move.ClearFamily();
        }
    }
}
