using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        //�ܹ�����Ϸ�����������Ļ��
        //Gcontroller.camShake = GameObject.FindGameObjectWithTag("CameraShake").GetComponent<CameraShake>();//һ�����ҵ�tagΪcamshake��һ�����õ��ű�camshake��Ȩ��
        //target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void LateUpdate()//����ƶ�
    {
        if (target != null)//��Ŀ�겻�ǿյĵ�ʱ��
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
