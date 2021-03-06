﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new move", menuName = "move")]
public class Move : ScriptableObject {

    public string _category;        //Category name used to classify this Move
    public string _moveName;        //Name of this Move
    public Sprite _icon;            //Sprite containing the picture of this Move

    private Move _parent;           //Parent Move of this Move
    private List<Move> _childMoves; //List containing the children Moves of this Move

    /// <summary>
    /// Verifies if the Move has a category
    /// </summary>
    /// <returns></returns>
    public bool HasCategory()
    {
        return (_category != "");
    }

    /// <summary>
    /// Verify if the Move has at least one child
    /// </summary>
    /// <returns></returns>
    public bool HasChild()
    {
        return (_childMoves.Count > 0);
    }

    /// <summary>
    /// get the list of children Moves
    /// </summary>
    /// <returns></returns>
    public List<Move> GetChildren()
    {
        return _childMoves;
    }

    /// <summary>
    /// Set a new parent Move to this Move
    /// </summary>
    /// <param name="parent">the new parent Move</param>
    public void SetParent( Move parent)
    {
        _parent = parent;
    }

    /// <summary>
    /// Add a new child Move to this Move
    /// </summary>
    /// <param name="child"> the new child move</param>
    public void AddChild(Move child)
    {
        if (_childMoves == null)
        {
            _childMoves = new List<Move>();
        }

        if (!_childMoves.Contains(child))
        {
            _childMoves.Add(child);
        }
    }

    /// <summary>
    /// Reinitialize the children of the Move
    /// </summary>
    public void ClearFamily()
    {
        _childMoves = new List<Move>();
    }

    public virtual void SetUp(GameObject stickman)
    {
        Debug.Log("Called set up on a basic move!");
    }

    public virtual void PhantomExecute(MouseFollower target)
    {
        Debug.Log("Called phantom exectute on a basic move!");
    }

    public virtual void Execute(MouseFollower target)
    {
        Debug.Log("Called execute on a basic move!");
    }
}
