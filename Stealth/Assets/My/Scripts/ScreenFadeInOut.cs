using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFadeInOut : MonoBehaviour
{
    public float fadeSpeed = 1.5f;

    private bool sceneStarting;
    private RawImage rawImage;

    // Start is called before the first frame update
    void Start()
    {
        sceneStarting = true;
        rawImage = this.GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneStarting)
        {
            rawImage.color = Color.Lerp(rawImage.color, Color.clear, fadeSpeed * Time.deltaTime);

            if (rawImage.color.a <= 0.05f)
            {
                rawImage.color = Color.clear;
                sceneStarting = false;
                rawImage.enabled = false;
            }
        }
    }

    public void EndScene()
    {
        rawImage.enabled = true;
        rawImage.color = Color.Lerp(rawImage.color, Color.black, fadeSpeed * Time.deltaTime);
        if (rawImage.color.a > 0.95f)
            SceneManager.LoadScene(0);
    }
}
