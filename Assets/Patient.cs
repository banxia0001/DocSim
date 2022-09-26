using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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