using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject LightSprite;
    public Light thisLight;
    public float lightNow;
    // Start is called before the first frame update
    void Start()
    {
        float random2 = Random.Range(0.6f, 1.2f);
        lightNow = random2;
        thisLight.intensity = lightNow;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lightNow -= 0.03f * Time.fixedDeltaTime;
        if (lightNow <= 0) lightNow = 0;
        if (lightNow > 1.2f) lightNow = 1.2f;

        float scaleNow = lightNow / 1.2f;
        LightSprite.transform.localScale = new Vector3(scaleNow, scaleNow, scaleNow);
        thisLight.intensity = lightNow;
    }

    public void GiveLight()
    {
        lightNow = lightNow + 0.02f;
    }

    public void EatLight()
    {
        lightNow = lightNow - 0.03f;
    }

}
