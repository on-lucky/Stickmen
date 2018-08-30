using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChangerButton : ButtonSelector
{

    public int goalSceneIndex;

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        SceneManager.LoadScene("Random Fight lobby");
    }
}