using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public bool isOnLancet02;//2��������
    public bool isOnLancet07;//7��������
    public bool isOnScissor;//����
    public bool isOnForceps;//����



    public void Update()
    {
        isOnLancet02 = false;
        isOnLancet07 = false;
        isOnScissor = false;
        isOnForceps = false;


        if (Input.GetKey(KeyCode.A)) isOnLancet02 = true;
        if (Input.GetKey(KeyCode.S)) isOnLancet07 = true;
        if (Input.GetKey(KeyCode.W)) isOnScissor = true;
        if (Input.GetKey(KeyCode.D)) isOnForceps = true;





    }


}
