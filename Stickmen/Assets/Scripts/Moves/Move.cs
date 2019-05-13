using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new move", menuName = "move")]
public class Move : ScriptableObject {

    public string _category;                        // Category name used to classify this Move
    public string _moveName;                        // Name of this Move
    public Sprite _icon;                            // Sprite containing the picture of this Move
    public AnimState animState = AnimState.All;   // Animation state from which the move should be accessible 

    private Move _parent;                           // Parent Move of this Move
    private List<Move> _childMoves;                 // List containing the children Moves of this Move
    protected GameObject stickman;                  // The GameObject that should execute the Move 
    protected Animator animator;                    // animator animating the Move
    protected bool isPhantom;                       // if the object that is executing the move is a phantom
    protected int aimerIndex = 0;                   // index of the aimer of the move

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

    public virtual void SetUp(GameObject _stickman)
    {
        stickman = _stickman;
        animator = stickman.GetComponentInChildren<Animator>();
    }

    public virtual void SpawnPhantom(MouseFollower target)
    {
        Debug.Log("Called spawn phantom on a basic move!");
    }

    public virtual void PhantomExecute()
    {
        Debug.Log("Called PhantomExecute on a basic move!");
    }

    public virtual void Execute(MouseFollower target)
    {
        Debug.Log("Called execute on a basic move!");
    }

    public virtual bool UpdateMethod(float deltaTime)
    {
        Debug.Log("Called update method on a basic move!");
        return true;
    }

    protected StickmanProfile GetProfile()
    {
        NetworkPlayer stickmanPlayer = stickman.GetComponent<NetworkPlayer>();
        Shade shade = stickman.GetComponent<Shade>();
        Phantom phantom = stickman.GetComponent<Phantom>();

        if (stickmanPlayer != null)
        {
            return stickmanPlayer._profile;
        }
        else if(shade != null)
        {
            return shade._profile;
        }
        else
        {
            return phantom._profile;
        }
    }

    protected void SetIsPhantom()
    {
        NetworkPlayer stickmanPlayer = stickman.GetComponent<NetworkPlayer>();
        Shade shade = stickman.GetComponent<Shade>();
        Phantom phantom = stickman.GetComponent<Phantom>();

        if (stickmanPlayer != null)
        {
            isPhantom = false;
        }
        else if (shade != null)
        {
            isPhantom = false;
        }
        else
        {
            isPhantom = true;
        }
    }

    public virtual void SwitchAimer()
    {
        //TODO: does nothing?
    }

    public void SetAnimatorSpeed(float speed)
    {
        animator.speed = speed;
    }

    public bool CheckPhantom()
    {
        return isPhantom;
    }

    public void DestroyStickman()
    {
        Destroy(stickman);
    }
}
