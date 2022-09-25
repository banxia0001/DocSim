using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        //能够在游戏里任意调用屏幕震动
        //Gcontroller.camShake = GameObject.FindGameObjectWithTag("CameraShake").GetComponent<CameraShake>();//一个是找到tag为camshake，一个是拿到脚本camshake的权限
        //target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void LateUpdate()//相机移动
    {
        if (target != null)//当目标不是空的的时候
        {
            if (transform.position != target.position)
            {
                Vector3 targetPos;
                if (target.transform.position.z < -3.2f)
                {
                   targetPos = new Vector3(target.position.x, target.position.y, -3.2f);
                }
                else targetPos = target.position;
              
                transform.position = Vector3.Lerp(transform.position, targetPos, 1);
            }
        }
    }
}
