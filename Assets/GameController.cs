using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public bool isOnLancet02;//2号手术刀
    public bool isOnLancet07;//7号手术刀
    public bool isOnScissor;//剪刀
    public bool isOnForceps;//镊子



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
