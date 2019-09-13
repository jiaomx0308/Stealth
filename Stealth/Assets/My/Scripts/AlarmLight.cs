using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour
{
    public float fadeSpeed = 2f;  //灯光报警fade的速度(默认2s变化一次)
    public float hightIntensity = 4f; //最高最低亮度
    public float lowIntensity = 0.5f;
    public float changeMargin = 0.2f; //插值阈值
    public bool alarmOn;

    private float targetIntensity; //目标亮度值
    private Light alarmLight;
    private AudioSource audioSource;

    private void Awake()
    {
        alarmLight = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();
        alarmLight.intensity = 0;
        targetIntensity = hightIntensity;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (alarmOn)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();

            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, targetIntensity, fadeSpeed * Time.deltaTime);

            if (Mathf.Abs(targetIntensity - alarmLight.intensity) < changeMargin)
            {
                if (targetIntensity == hightIntensity)
                    targetIntensity = lowIntensity;
                else
                    targetIntensity = hightIntensity;
            }
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, 0, fadeSpeed * Time.deltaTime);
        }
    }
}
