using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Patient
{
    public List<Organ> organList = new List<Organ>();

    [TextArea]
    public string Description;

    public Patient(List<Organ> organList)
    {
        this.organList = organList;
    }
}

[System.Serializable]
public class Tool
{
    public Sprite image;
    public string toolName;

  

    public Tool(string name)
    {
        this.toolName = name;
    }
}