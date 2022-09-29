using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar_PointController : MonoBehaviour
{
    public LayerMask HitLayer;

    public float speed = 20f;
    private Rigidbody2D rg2d;

    // Start is called before the first frame update
    void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame


    void FixedUpdate()
    {
        rg2d.velocity = transform.right * speed;
    }


    public bool checkPointHit()
    {
        Collider2D coll = Physics2D.OverlapCircle((Vector2)transform.position, .005f, HitLayer);
        if (coll != null) return true;
        else return false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("!!");
        if (other.gameObject.CompareTag("Bar_Wall_Layer"))
        {
            speed = -speed;
           //transform.localRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 180f);
        }
    }
}
