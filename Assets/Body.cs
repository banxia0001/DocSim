using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public GameObject heart;
    public GameObject brain;
    public GameObject Lung;
    public GameController GC;
    // Update is called once per frame
    void LateUpdate()
    {
        if (GC.isOnHeart) heart.SetActive(true);
        else heart.SetActive(false);

        if (GC.isOnBrain) brain.SetActive(true);
        else brain.SetActive(false);

        if (GC.isOnLung) Lung.SetActive(true);
        else Lung.SetActive(false);
    }
}
