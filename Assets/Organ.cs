using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Organ")]
public class Organ : ScriptableObject
{
    public string Name;
    public Sprite sprite;
    public int loop;
    public int loopMax;
    public int workOnTimeMax;
    public int workOnTimeMin;
}



