using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LasterBlinking : MonoBehaviour
{
    public float onTime;  //灯灭的时间间隔
    public float offTime;  //灯亮的时间间隔

    private float timer;  //流逝的时间
    private Renderer laserRenerer;
    private Light laserLight;

    // Start is called before the first frame update
    void Start()
    {
        laserRenerer = GetComponent<Renderer>();
        laserLight = GetComponent<Light>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (laserRenerer.enabled && timer >= onTime)
        {
            laserRenerer.enabled = false;
            laserLight.enabled = false;
            timer = 0;
        }
        else if (!laserRenerer.enabled && timer >= offTime)
        {
            laserRenerer.enabled = true;
            laserLight.enabled = true;
            timer = 0;
        }
    }
}
