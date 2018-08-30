using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconSpawner : MonoBehaviour {

    public GameObject _iconTemplate;    //Template icon object used for instanciation TODO:remove "_" on public attribute
    public List<Move> _moveList;        //Complete Move list to spawn TODO:remove "_" on public attribute
    public float menuScale = 0.5f;      //Scale of the Move menu

    /// <summary>
    /// Spawn the complete Move menu
    /// </summary>
    /// <param name="moveList">Move list containing the Move tree</param>
    public void SpawnIcons(List<Move> moveList)
    {
        _moveList = moveList;
        List<Move> roots = GetRoots();

        GameObject icon = Instantiate(_iconTemplate, this.transform);

        for (int i = 0; i < roots.Count; i++)
        {
            SpawnTreeRecursively(roots[i], icon, i);
        }

        icon.transform.localScale = new Vector3(menuScale, menuScale, 1);
        icon.transform.Translate(new Vector3(1, 0, 0));
    }

    /// <summary>
    /// Get the Root Moves of the Move tree
    /// </summary>
    /// <returns></returns>
    private List<Move> GetRoots()
    {
        List<Move> rootList = new List<Move>();
        foreach (Move mov in _moveList)
        {
            if (!mov.HasCategory())
            {
                rootList.Add(mov);
            }
        }
        return rootList;
    }

    /// <summary>
    /// Secursively spawn the Move menu
    /// </summary>
    /// <param name="move">Current move that requires an Icon object</param>
    /// <param name="parent">Parent Icon gameObject of the move</param>
    /// <param name="index">Index of the current move</param>
    private void SpawnTreeRecursively(Move move, GameObject parent, int index)
    {
        GameObject icon = SpawnIcon(move, parent);
        icon.GetComponent<Icon>()._index = index;

        //Make the icon disapear if it isn't at the top of the tree
        if(icon.GetComponent<Icon>()._parent._parent != null)
        {
            icon.GetComponent<Icon>().Disapear();
        }
        else
        {
            icon.GetComponent<Icon>().Appear();
        }

        if (move.HasChild())
        {
            for (int i = 0; i < move.GetChildren().Count; i++)
            {
                SpawnTreeRecursively(move.GetChildren()[i], icon, i);
            }
        }
    }

    /// <summary>
    /// Intantiate a Icon gameObject representing a certain Move
    /// </summary>
    /// <param name="move">Move of the instanciated gameObject</param>
    /// <param name="parent">Parent gameObject to the instantiated one</param>
    /// <returns></returns>
    private GameObject SpawnIcon(Move move, GameObject parent = null)
    {
        GameObject icon;

        if (parent == null)
        {
            icon = Instantiate(_iconTemplate, this.transform.position, this.transform.rotation);
            icon.GetComponent<Icon>().EnableCollider(true);
        }
        else {
            icon = Instantiate(_iconTemplate, parent.transform);
            icon.GetComponent<Icon>()._parent = parent.GetComponent<Icon>();
            parent.GetComponent<Icon>()._childIcons.Add(icon.GetComponent<Icon>());
        }

        icon.GetComponent<SpriteRenderer>().sprite = move._icon;
        icon.GetComponent<Icon>().SetName(move._moveName);

        return icon;
    }
}