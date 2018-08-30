using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildConsole : MonoBehaviour {

    public static BuildConsole instance;

    public bool isVisible;

    private TextMeshPro myText;
    private Queue<string> texts;

    private void Awake()
    {
        if (BuildConsole.instance != null)
        {
            Debug.LogError("More than one BuildConsole in the scene!");
        }
        BuildConsole.instance = this;
        myText = GetComponent<TextMeshPro>();
        texts = new Queue<string>();
    }

    public static void Print(string message)
    {
        BuildConsole.instance.ChangeText(message);
    }

    public void ChangeText(string message)
    {
        texts.Enqueue(message);
        if (texts.Count > 3)
            texts.Dequeue();
        ShowMessages();
    }

    private void ShowMessages()
    {
        string console = "";
        if (isVisible)
        {
            foreach (string message in texts)
            {
                console += message + "\n";
            }
        }
        myText.text = console;
    }

}
